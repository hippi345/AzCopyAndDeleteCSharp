using AzCopyAndDelete.Configuration;
using AzCopyAndDelete.Storage;

namespace AzCopyAndDelete;

public sealed class BackupWorkflow
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILocalFileService _localFileService;
    private readonly IConsolePrompter _prompter;
    private readonly TextWriter _output;

    public BackupWorkflow(
        IBlobStorageService blobStorageService,
        ILocalFileService localFileService,
        IConsolePrompter prompter,
        TextWriter output)
    {
        _blobStorageService = blobStorageService;
        _localFileService = localFileService;
        _prompter = prompter;
        _output = output;
    }

    public async Task RunAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        _output.WriteLine("Welcome to the simple data backup app w/ option to delete");

        string container = settings.ContainerName ?? _prompter.ReadLine("Coolio and what is the container/path you are wanting to target?");
        string sourceFile = settings.SourcePath ?? _prompter.ReadLine("Oh and one last thing. Where is the file or folder you want to upload to Storage?");

        if (!File.Exists(sourceFile))
        {
            _output.WriteLine($"Source path does not exist: {sourceFile}");
            return;
        }

        _output.WriteLine("Cool let's get this party started");

        string blobName = settings.BlobName ?? sourceFile;
        _output.WriteLine($"Uploading to Blob storage as blob '{blobName}'");

        await _blobStorageService.UploadFileAsync(container, blobName, sourceFile, cancellationToken);

        bool listBlobs = ResolveListBlobs(settings);
        if (listBlobs)
        {
            await ListBlobsAsync(container, cancellationToken);
        }
        else
        {
            _output.WriteLine("I feel that yo. We can move on to whether you want to delete the files so they don't take up disk space");
        }

        bool deleteLocal = ResolveDeleteLocal(settings);
        if (deleteLocal)
        {
            _localFileService.DeleteIfExists(sourceFile);
        }
        else if (settings.DeleteLocalAfterUpload is null)
        {
            _output.WriteLine("I feel that yo. We can move on to whether you want to delete the files so they don't take up disk space");
        }

        _output.WriteLine("well looks like we reached the end good sir or madam. Have a good one");
    }

    private bool ResolveListBlobs(AppSettings settings)
    {
        if (settings.ListBlobsAfterUpload is bool configured)
        {
            return configured;
        }

        _output.WriteLine("Right on bro if you made it this far. You want to list the blobs just to make sure you got it in storage safely? y/n");
        string answer = _prompter.ReadLine();
        if (PromptParser.IsAffirmative(answer))
        {
            return true;
        }

        if (!PromptParser.IsNegative(answer))
        {
            _output.WriteLine("I did not quite catch that but we will move on to whether you want to delete files");
        }

        return false;
    }

    private bool ResolveDeleteLocal(AppSettings settings)
    {
        if (settings.DeleteLocalAfterUpload is bool configured)
        {
            return configured;
        }

        _output.WriteLine("Hey so since you got your files in storage, you may not want em on your local machine anymore. Want to delete them? y/n");
        string deleteReply = _prompter.ReadLine();
        if (PromptParser.IsAffirmative(deleteReply))
        {
            return true;
        }

        if (!PromptParser.IsNegative(deleteReply))
        {
            _output.WriteLine("I did not quite catch that but we will move on to whether you want to delete files");
        }

        return false;
    }

    private async Task ListBlobsAsync(string container, CancellationToken cancellationToken)
    {
        _output.WriteLine("List blobs in container.");
        IReadOnlyList<string> uris = await _blobStorageService.ListBlobUrisAsync(container, cancellationToken);
        foreach (string uri in uris)
        {
            _output.WriteLine(uri);
        }
    }
}
