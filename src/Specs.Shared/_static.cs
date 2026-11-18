using System.Text.RegularExpressions;

namespace Specs;

public static class Statics
{
    [Pure]
    public static Regex regex([StringSyntax("Regex")] string pattern, RegexOptions options = RegexOptions.CultureInvariant) => new(pattern, options, TimeSpan.FromSeconds(10));
}
