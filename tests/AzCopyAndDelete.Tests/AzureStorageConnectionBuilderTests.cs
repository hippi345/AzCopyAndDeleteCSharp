using AzCopyAndDelete.Storage;
using Xunit;

namespace AzCopyAndDelete.Tests;

public class AzureStorageConnectionBuilderTests
{
    [Fact]
    public void TryBuildConnectionString_uses_explicit_connection_string()
    {
        bool ok = AzureStorageConnectionBuilder.TryBuildConnectionString(
            "DefaultEndpointsProtocol=https;AccountName=test;AccountKey=key;EndpointSuffix=core.windows.net",
            null,
            null,
            out string result,
            out string? error);

        Assert.True(ok);
        Assert.Null(error);
        Assert.Contains("AccountName=test", result);
    }

    [Fact]
    public void TryBuildConnectionString_builds_from_account_name_and_key()
    {
        bool ok = AzureStorageConnectionBuilder.TryBuildConnectionString(
            null,
            "myaccount",
            "mykey",
            out string result,
            out string? error);

        Assert.True(ok);
        Assert.Null(error);
        Assert.Equal(
            "DefaultEndpointsProtocol=https;AccountName=myaccount;AccountKey=mykey;EndpointSuffix=core.windows.net",
            result);
    }

    [Fact]
    public void TryBuildConnectionString_fails_when_credentials_missing()
    {
        bool ok = AzureStorageConnectionBuilder.TryBuildConnectionString(
            null,
            null,
            null,
            out _,
            out string? error);

        Assert.False(ok);
        Assert.NotNull(error);
    }
}
