namespace AzCopyAndDelete.Storage;

public sealed class LocalFileService : ILocalFileService
{
    public bool DeleteIfExists(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        File.Delete(path);
        return true;
    }
}
