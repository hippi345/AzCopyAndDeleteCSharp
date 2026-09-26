namespace AzCopyAndDelete;

public sealed class ConsolePrompter : IConsolePrompter
{
    private readonly TextWriter _output;
    private readonly TextReader _input;

    public ConsolePrompter(TextWriter output, TextReader input)
    {
        _output = output;
        _input = input;
    }

    public string ReadLine(string? prompt = null)
    {
        if (!string.IsNullOrEmpty(prompt))
        {
            _output.WriteLine(prompt);
        }

        return _input.ReadLine() ?? string.Empty;
    }
}
