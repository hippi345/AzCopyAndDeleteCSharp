namespace AzCopyAndDelete.Configuration;

public sealed class AppSettings
{
    public string? ConnectionString { get; init; }

    public string? AccountName { get; init; }

    public string? AccountKey { get; init; }

    public string? ContainerName { get; init; }

    public string? SourcePath { get; init; }

    public string? BlobName { get; init; }

    public bool? ListBlobsAfterUpload { get; init; }

    public bool? DeleteLocalAfterUpload { get; init; }

    public static AppSettings FromEnvironment()
    {
        return new AppSettings
        {
            ConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"),
            AccountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_NAME"),
            AccountKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_KEY"),
            ContainerName = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONTAINER"),
            SourcePath = Environment.GetEnvironmentVariable("AZURE_SOURCE_PATH"),
            BlobName = Environment.GetEnvironmentVariable("AZURE_BLOB_NAME"),
            ListBlobsAfterUpload = ParseOptionalBool(Environment.GetEnvironmentVariable("AZURE_LIST_BLOBS")),
            DeleteLocalAfterUpload = ParseOptionalBool(Environment.GetEnvironmentVariable("AZURE_DELETE_LOCAL")),
        };
    }

    private static bool? ParseOptionalBool(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return PromptParser.IsAffirmative(value) ? true :
            PromptParser.IsNegative(value) ? false : null;
    }
}
