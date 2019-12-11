using Qowaiv.Formatting;
using System.Text.RegularExpressions;

namespace Qowaiv
{
    internal static class Parsing
    {
        private static Regex SpacePattern = new Regex(@"\s", RegexOptions.Compiled);
        private static Regex SpaceAndMarkupPattern = new Regex(@"[\s\.\-_]", RegexOptions.Compiled);

        /// <summary>Clears spacing.</summary>
        public static string ClearSpacing(string str)
        {
            return SpacePattern.Replace(str, "");
        }
        /// <summary>Clears spacing and uppercases.</summary>
        public static string ClearSpacingToUpper(string str)
        {
            return ClearSpacing(str).ToUpperInvariant();
        }
        /// <summary>Clears spacing.</summary>
        public static string ClearSpacingAndMarkup(string str)
        {
            return SpaceAndMarkupPattern.Replace(str, "");
        }
        /// <summary>Clears spacing and uppercases.</summary>
        public static string ClearSpacingAndMarkupToUpper(string str)
        {
            return ClearSpacingAndMarkup(str).ToUpperInvariant();
        }

        /// <summary>Unifies the string.</summary>
        public static string ToUnified(string str)
        {
            return (StringFormatter.ToNonDiacritic(str) ?? string.Empty).ToUpperInvariant().Replace(" ", "");
        }
    }
}
