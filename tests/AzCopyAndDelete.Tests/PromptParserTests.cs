using AzCopyAndDelete.Configuration;
using Xunit;

namespace AzCopyAndDelete.Tests;

public class PromptParserTests
{
    [Theory]
    [InlineData("y", true)]
    [InlineData("Y", true)]
    [InlineData("yes", true)]
    [InlineData("YES", true)]
    [InlineData("n", false)]
    [InlineData("maybe", false)]
    public void IsAffirmative_recognizes_yes_variants(string input, bool expected)
    {
        Assert.Equal(expected, PromptParser.IsAffirmative(input));
    }

    [Theory]
    [InlineData("n", true)]
    [InlineData("no", true)]
    [InlineData("y", false)]
    public void IsNegative_recognizes_no_variants(string input, bool expected)
    {
        Assert.Equal(expected, PromptParser.IsNegative(input));
    }
}
