using System.Runtime.CompilerServices;

namespace AwesomeAssertions;

public static class Shoulds
{
    [OverloadResolutionPriority(64)]
    public static AnsiAssertions Should(this Ansi ansi) => new(ansi);
}
