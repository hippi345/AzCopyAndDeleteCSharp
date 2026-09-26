namespace AzCopyAndDelete.Storage;

public static class AzureStorageConnectionBuilder
{
    public static bool TryBuildConnectionString(
        string? connectionString,
        string? accountName,
        string? accountKey,
        out string result,
        out string? error)
    {
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            result = connectionString.Trim();
            error = null;
            return true;
        }

        if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(accountKey))
        {
            result = string.Empty;
            error = "Provide AZURE_STORAGE_CONNECTION_STRING or both AZURE_STORAGE_ACCOUNT_NAME and AZURE_STORAGE_ACCOUNT_KEY.";
            return false;
        }

        result =
            $"DefaultEndpointsProtocol=https;AccountName={accountName.Trim()};AccountKey={accountKey.Trim()};EndpointSuffix=core.windows.net";
        error = null;
        return true;
    }
}
