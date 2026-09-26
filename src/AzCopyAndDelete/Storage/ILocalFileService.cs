namespace AzCopyAndDelete.Storage;

public interface ILocalFileService
{
    bool DeleteIfExists(string path);
}
