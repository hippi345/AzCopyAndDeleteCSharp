namespace AzCopyAndDelete.Configuration;

public static class PromptParser
{
    public static bool IsAffirmative(string? answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            return false;
        }

        return answer.Equals("y", StringComparison.OrdinalIgnoreCase)
            || answer.Equals("yes", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsNegative(string? answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            return false;
        }

        return answer.Equals("n", StringComparison.OrdinalIgnoreCase)
            || answer.Equals("no", StringComparison.OrdinalIgnoreCase);
    }
}
