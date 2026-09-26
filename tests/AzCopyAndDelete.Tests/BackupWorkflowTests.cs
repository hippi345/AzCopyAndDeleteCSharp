using AzCopyAndDelete.Configuration;
using AzCopyAndDelete.Storage;
using Moq;
using Xunit;

namespace AzCopyAndDelete.Tests;

public class BackupWorkflowTests
{
    [Fact]
    public async Task RunAsync_uploads_lists_and_deletes_when_configured()
    {
        string tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, "payload");

        var blobMock = new Mock<IBlobStorageService>();
        blobMock
            .Setup(s => s.UploadFileAsync("container", tempFile, tempFile, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        blobMock
            .Setup(s => s.ListBlobUrisAsync("container", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { "https://example.blob.core.windows.net/container/blob" });

        var localMock = new Mock<ILocalFileService>();
        localMock.Setup(s => s.DeleteIfExists(tempFile)).Returns(true);

        var output = new StringWriter();
        var prompter = new QueuePrompter([]);

        var workflow = new BackupWorkflow(blobMock.Object, localMock.Object, prompter, output);
        var settings = new AppSettings
        {
            ConnectionString = "DefaultEndpointsProtocol=https;AccountName=a;AccountKey=b;EndpointSuffix=core.windows.net",
            ContainerName = "container",
            SourcePath = tempFile,
            ListBlobsAfterUpload = true,
            DeleteLocalAfterUpload = true,
        };

        await workflow.RunAsync(settings);

        blobMock.Verify(s => s.UploadFileAsync("container", tempFile, tempFile, It.IsAny<CancellationToken>()), Times.Once);
        blobMock.Verify(s => s.ListBlobUrisAsync("container", It.IsAny<CancellationToken>()), Times.Once);
        localMock.Verify(s => s.DeleteIfExists(tempFile), Times.Once);
        Assert.Contains("https://example.blob.core.windows.net/container/blob", output.ToString());

        File.Delete(tempFile);
    }

    [Fact]
    public async Task RunAsync_skips_upload_when_source_missing()
    {
        var blobMock = new Mock<IBlobStorageService>(MockBehavior.Strict);
        var localMock = new Mock<ILocalFileService>(MockBehavior.Strict);
        var output = new StringWriter();
        var workflow = new BackupWorkflow(blobMock.Object, localMock.Object, new QueuePrompter([]), output);

        await workflow.RunAsync(new AppSettings
        {
            ConnectionString = "DefaultEndpointsProtocol=https;AccountName=a;AccountKey=b;EndpointSuffix=core.windows.net",
            ContainerName = "c",
            SourcePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")),
        });

        Assert.Contains("does not exist", output.ToString());
    }

    private sealed class QueuePrompter : IConsolePrompter
    {
        private readonly Queue<string> _responses;

        public QueuePrompter(IEnumerable<string> responses)
        {
            _responses = new Queue<string>(responses);
        }

        public string ReadLine(string? prompt = null) => _responses.Count > 0 ? _responses.Dequeue() : string.Empty;
    }
}
