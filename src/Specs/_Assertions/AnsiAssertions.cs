using AwesomeAssertions.Execution;

namespace AwesomeAssertions;

public sealed class AnsiAssertions(Ansi subject)
{
    public Ansi Subject { get; } = subject;

    private readonly AssertionChain Chain = AssertionChain.GetOrCreate();

    public void Be(string expected, string because="", params object[] becauseArgs) => Chain
        .ForCondition(Subject.ToString() == expected)
        .BecauseOf(because, becauseArgs)
        .WithDefaultIdentifier("ASNI")
        .FailWith("Expected {context} to refer to {0}{reason}, but found {1}.", Format(expected), Format(Subject));

    private static string Format(object o) => o.ToString()!.Replace("\x1b", """\x1b""");
}
