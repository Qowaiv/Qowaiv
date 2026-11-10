#pragma warning disable

using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace Bench;

public partial class IbanBenchmark
{
    private static class RegexBasedParser
    {
        public static string? Parse(string str)
        {
            var sb = new StringBuilder(str.Length);
            foreach (var c in str)
            {
                if (c is ' ' or '\r' or '\t' or '\n' or '-' or '_' or '.' or (char)160) continue;
                sb.Append(char.ToUpperInvariant(c));
            }
            var iban = sb.ToString();

            if (iban.Length < 12) return null;

            return (Patterns.TryGetValue(iban[..2], out var pattern)
                ? pattern.IsMatch(iban[2..])
                : Generic.IsMatch(iban))
                && Mod97(iban)
                ? iban
                : null;
        }

        private static bool Mod97(string iban)
        {
            var num = 0;

            // Calculate the first 4 characters (country and checksum) last
            for (var i = 4; i < iban.Length; i++)
            {
                num = Next(num, iban[i]);
            }
            for (var i = 0; i < 4; i++)
            {
                num = Next(num, iban[i]);
            }

            return num is 1;

            static int Next(int num, char ch)
                => (ch <= '9'
                ? (num * 10) + ch - '0'
                : (num * 100) + ch - 'A' + 10) % 97;
        }

        private static readonly FrozenDictionary<string, Regex> Patterns = Init().ToFrozenDictionary();
        private static readonly Regex Generic = new("^[A-Z]{2}[0-9]{2}[A-Z0-9]{8,30}$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);

        private static Dictionary<string, Regex> Init()
        {
            var opts = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.NonBacktracking;
            return new()
            {
                ["AD"] = new(@"^[0-9]{10}[A-Z0-9]{12}$", opts),
                ["AE"] = new(@"^[0-9]{21}$", opts),
                ["AL"] = new(@"^[0-9]{10}[A-Z0-9]{16}$", opts),
                ["AO"] = new(@"^[0-9]{23}$", opts),
                ["AT"] = new(@"^[0-9]{18}$", opts),
                ["AZ"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{20}$", opts),
                ["BA"] = new(@"^39[0-9]{16}$", opts),
                ["BE"] = new(@"^[0-9]{14}$", opts),
                ["BF"] = new(@"^[0-9]{2}[A-Z0-9]{2}[0-9]{22}$", opts),
                ["BG"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{6}[A-Z0-9]{8}$", opts),
                ["BH"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{14}$", opts),
                ["BI"] = new(@"^[0-9]{25}$", opts),
                ["BJ"] = new(@"^[0-9]{2}[A-Z0-9]{2}[0-9]{22}$", opts),
                ["BR"] = new(@"^[0-9]{25}[A-Z][A-Z0-9]$", opts),
                ["BY"] = new(@"^[0-9]{2}[A-Z0-9]{4}[0-9]{4}[A-Z0-9]{16}$", opts),
                ["CF"] = new(@"^[0-9]{25}$", opts),
                ["CG"] = new(@"^[0-9]{25}$", opts),
                ["CH"] = new(@"^[0-9]{7}[A-Z0-9]{12}$", opts),
                ["CI"] = new(@"^[0-9]{2}[A-Z]{2}[0-9]{22}$", opts),
                ["CM"] = new(@"^[0-9]{25}$", opts),
                ["CR"] = new(@"^[0-9]{2}0[0-9]{17}$", opts),
                ["CV"] = new(@"^[0-9]{23}$", opts),
                ["CY"] = new(@"^[0-9]{10}[A-Z0-9]{16}$", opts),
                ["CZ"] = new(@"^[0-9]{22}$", opts),
                ["DE"] = new(@"^[0-9]{20}$", opts),
                ["DJ"] = new(@"^[0-9]{25}$", opts),
                ["DK"] = new(@"^[0-9]{16}$", opts),
                ["DO"] = new(@"^[0-9]{2}[A-Z0-9]{4}[0-9]{20}$", opts),
                ["DZ"] = new(@"^[0-9]{24}$", opts),
                ["EE"] = new(@"^[0-9]{18}$", opts),
                ["EG"] = new(@"^[0-9]{27}$", opts),
                ["ES"] = new(@"^[0-9]{22}$", opts),
                ["FI"] = new(@"^[0-9]{16}$", opts),
                ["FK"] = new(@"^[0-9]{2}[A-Z]{2}[0-9]{12}$", opts),
                ["FO"] = new(@"^[0-9]{16}$", opts),
                ["FR"] = new(@"^[0-9]{12}[A-Z0-9]{11}[0-9]{2}$", opts),
                ["GA"] = new(@"^[0-9]{25}$", opts),
                ["GB"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{14}$", opts),
                ["GE"] = new(@"^[0-9]{2}[A-Z]{2}[0-9]{16}$", opts),
                ["GI"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{15}$", opts),
                ["GL"] = new(@"^[0-9]{16}$", opts),
                ["GQ"] = new(@"^[0-9]{25}$", opts),
                ["GR"] = new(@"^[0-9]{9}[A-Z0-9]{16}$", opts),
                ["GT"] = new(@"^[0-9]{2}[A-Z0-9]{24}$", opts),
                ["GW"] = new(@"^[0-9]{2}[A-Z0-9]{2}[0-9]{19}$", opts),
                ["HN"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{20}$", opts),
                ["HR"] = new(@"^[0-9]{19}$", opts),
                ["HU"] = new(@"^[0-9]{26}$", opts),
                ["IE"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{14}$", opts),
                ["IL"] = new(@"^[0-9]{21}$", opts),
                ["IQ"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{15}$", opts),
                ["IR"] = new(@"^[0-9]{24}$", opts),
                ["IS"] = new(@"^[0-9]{24}$", opts),
                ["IT"] = new(@"^[0-9]{2}[A-Z][0-9]{10}[A-Z0-9]{12}$", opts),
                ["JO"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{4}[A-Z0-9]{18}$", opts),
                ["KM"] = new(@"^[0-9]{25}$", opts),
                ["KW"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{22}$", opts),
                ["KZ"] = new(@"^[0-9]{5}[A-Z0-9]{13}$", opts),
                ["LB"] = new(@"^[0-9]{6}[A-Z0-9]{20}$", opts),
                ["LC"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{24}$", opts),
                ["LI"] = new(@"^[0-9]{7}[A-Z0-9]{12}$", opts),
                ["LT"] = new(@"^[0-9]{18}$", opts),
                ["LU"] = new(@"^[0-9]{5}[A-Z0-9]{13}$", opts),
                ["LV"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{13}$", opts),
                ["LY"] = new(@"^[0-9]{23}$", opts),
                ["MA"] = new(@"^[0-9]{26}$", opts),
                ["MC"] = new(@"^[0-9]{12}[A-Z0-9]{11}[0-9]{2}$", opts),
                ["MD"] = new(@"^[0-9]{2}[A-Z0-9]{20}$", opts),
                ["ME"] = new(@"^25[0-9]{18}$", opts),
                ["MG"] = new(@"^[0-9]{25}$", opts),
                ["MK"] = new(@"^07[0-9]{3}[A-Z0-9]{10}[0-9]{2}$", opts),
                ["ML"] = new(@"^[0-9]{2}[A-Z0-9]{2}[0-9]{22}$", opts),
                ["MN"] = new(@"^[0-9]{6}[0-9]{12}$", opts),
                ["MR"] = new(@"^13[0-9]{23}$", opts),
                ["MT"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{5}[A-Z0-9]{18}$", opts),
                ["MU"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{16}000[A-Z]{3}$", opts),
                ["MZ"] = new(@"^[0-9]{23}$", opts),
                ["NE"] = new(@"^[0-9]{2}[A-Z]{2}[0-9]{22}$", opts),
                ["NI"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{20}$", opts),
                ["NL"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{10}$", opts),
                ["NO"] = new(@"^[0-9]{13}$", opts),
                ["OM"] = new(@"^[0-9]{5}[A-Z0-9]{16}$", opts),
                ["PK"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{16}$", opts),
                ["PL"] = new(@"^[0-9]{26}$", opts),
                ["PS"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{21}$", opts),
                ["PT"] = new(@"^50[0-9]{21}$", opts),
                ["QA"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{21}$", opts),
                ["RO"] = new(@"^[0-9]{2}[A-Z]{4}[A-Z0-9]{16}$", opts),
                ["RS"] = new(@"^35[0-9]{18}$", opts),
                ["RU"] = new(@"^[0-9]{16}[A-Z0-9]{15}$", opts),
                ["SA"] = new(@"^[0-9]{4}[A-Z0-9]{18}$", opts),
                ["SC"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{20}[A-Z]{3}$", opts),
                ["SD"] = new(@"^[0-9]{16}$", opts),
                ["SE"] = new(@"^[0-9]{22}$", opts),
                ["SI"] = new(@"^56[0-9]{15}$", opts),
                ["SK"] = new(@"^[0-9]{22}$", opts),
                ["SM"] = new(@"^[0-9]{2}[A-Z][0-9]{10}[A-Z0-9]{12}$", opts),
                ["SN"] = new(@"^[0-9]{2}[A-Z]{2}[0-9]{22}$", opts),
                ["SO"] = new(@"^[0-9]{21}$", opts),
                ["ST"] = new(@"^[0-9]{23}$", opts),
                ["SV"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{20}$", opts),
                ["TD"] = new(@"^[0-9]{25}$", opts),
                ["TG"] = new(@"^[0-9]{2}[A-Z]{2}[0-9]{22}$", opts),
                ["TL"] = new(@"^38[0-9]{19}$", opts),
                ["TN"] = new(@"^59[0-9]{20}$", opts),
                ["TR"] = new(@"^[0-9]{7}0[A-Z0-9]{16}$", opts),
                ["UA"] = new(@"^[0-9]{8}[A-Z0-9]{19}$", opts),
                ["VA"] = new(@"^[0-9]{20}$", opts),
                ["VG"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{16}$", opts),
                ["XK"] = new(@"^[0-9]{18}$", opts),
                ["YE"] = new(@"^[0-9]{2}[A-Z]{4}[0-9]{4}[A-Z0-9]{18}$", opts),
            };
        }
    }
}
