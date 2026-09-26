using Azure.Storage.Blobs;

namespace AzCopyAndDelete.Storage;

public sealed class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task UploadFileAsync(
        string containerName,
        string blobName,
        string localFilePath,
        CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(localFilePath, overwrite: true, cancellationToken);
    }

    public async Task<IReadOnlyList<string>> ListBlobUrisAsync(
        string containerName,
        CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        List<string> uris = [];

        await foreach (Azure.Storage.Blobs.Models.BlobItem blob in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            uris.Add(containerClient.GetBlobClient(blob.Name).Uri.ToString());
        }

        return uris;
    }
}
