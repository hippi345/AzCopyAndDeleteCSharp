using AzCopyAndDelete.Storage;
using Xunit;

namespace AzCopyAndDelete.Tests;

public class LocalFileServiceTests
{
    [Fact]
    public void DeleteIfExists_removes_existing_file()
    {
        string path = Path.GetTempFileName();
        var service = new LocalFileService();

        Assert.True(service.DeleteIfExists(path));
        Assert.False(File.Exists(path));
    }

    [Fact]
    public void DeleteIfExists_returns_false_when_missing()
    {
        var service = new LocalFileService();
        Assert.False(service.DeleteIfExists(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"))));
    }
}
