namespace AzCopyAndDelete.Storage;

public interface IBlobStorageService
{
    Task UploadFileAsync(string containerName, string blobName, string localFilePath, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> ListBlobUrisAsync(string containerName, CancellationToken cancellationToken = default);
}
