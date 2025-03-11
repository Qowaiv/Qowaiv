namespace Qowaiv.TestTools;

internal static class Should
{
    /// <summary>Verifies if the two objects are equal.</summary>
    /// <remarks>
    /// This method exists to minimize the burden of converting the legacy
    /// tests. It should not be used for new tests.
    /// </remarks>
    public static void BeEqual<T>(T expected, T actual, string because = "")
        => actual.Should().Be(expected, because);
}
