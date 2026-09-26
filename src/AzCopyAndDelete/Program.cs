using AzCopyAndDelete.Configuration;
using AzCopyAndDelete.Storage;
using Azure.Storage.Blobs;

namespace AzCopyAndDelete;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            AppSettings settings = AppSettings.FromEnvironment();
            var prompter = new ConsolePrompter(Console.Out, Console.In);

            string accountName = settings.AccountName ?? string.Empty;
            string accountKey = settings.AccountKey ?? string.Empty;

            if (!AzureStorageConnectionBuilder.TryBuildConnectionString(
                    settings.ConnectionString,
                    accountName,
                    accountKey,
                    out string connectionString,
                    out _))
            {
                accountName = prompter.ReadLine("What is your storage account name?");
                accountKey = prompter.ReadLine("Cool deal and what is your storage account key?");
                if (!AzureStorageConnectionBuilder.TryBuildConnectionString(
                        settings.ConnectionString,
                        accountName,
                        accountKey,
                        out connectionString,
                        out string? connectionError))
                {
                    Console.WriteLine("Big bummer bro. That connection string did not come through for us.");
                    Console.WriteLine(connectionError);
                    return 1;
                }
            }

            settings = new AppSettings
            {
                ConnectionString = connectionString,
                AccountName = accountName,
                AccountKey = accountKey,
                ContainerName = settings.ContainerName,
                SourcePath = settings.SourcePath,
                BlobName = settings.BlobName,
                ListBlobsAfterUpload = settings.ListBlobsAfterUpload,
                DeleteLocalAfterUpload = settings.DeleteLocalAfterUpload,
            };

            IBlobStorageService blobStorage = new AzureBlobStorageService(new BlobServiceClient(connectionString));
            var workflow = new BackupWorkflow(blobStorage, new LocalFileService(), prompter, Console.Out);
            await workflow.RunAsync(settings);
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return 1;
        }
        finally
        {
            if (!Console.IsInputRedirected)
            {
                Console.WriteLine("Press any key to exit");
                _ = Console.ReadLine();
            }
        }
    }
}
