namespace AzCopyAndDelete;

public interface IConsolePrompter
{
    string ReadLine(string? prompt = null);
}
