namespace Benchmarks;

public partial class IbanBenchmark
{
    public static class Reference
    {
        /// <summary>
        /// Implementation of <see cref="InternationalBankAccountNumber"/>
        /// when still relying on <see cref="Regex"/>es.
        /// </summary>
        public static class RegexBased
        {
            private static readonly Regex Pattern = new(@"^[A-Z]\s{0,2}[A-Z]\s{0,2}[0-9]\s{0,2}[0-9](\s{0,2}[0-9A-Z]){8,36}$", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));

            private static readonly Regex PatternTimmed = new(@"^[A-Z][A-Z][0-9][0-9][0-9A-Z]{8,36}$", RegexOptions.None, TimeSpan.FromSeconds(1));

            public static string? Improved(string? s)
            {
                var buffer = s.Buffer().Unify();
                if (buffer.IsEmpty())
                {
                    return null;
                }
                else if (buffer.Length >= 12 && buffer.Length <= 36)
                {
                    var str = buffer.ToString();

#pragma warning disable S1067 // Expressions should not be too complex
                    // This code is here just for reference purposes.
                    if (Country.TryParse(str[..2], out var country)
                        && !country.IsEmptyOrUnknown()
                        && (LocalizedPatterns.TryGetValue(country, out var localizedPattern)
                            ? localizedPattern.IsMatch(str)
                            : PatternTimmed.IsMatch(str))
                        && Mod97(str))
                    {
                        return str;
                    }
                    else
                    {
                        return null;
                    }
#pragma warning restore S1067 // Expressions should not be too complex

                }
                else if (Qowaiv.Unknown.IsUnknown(buffer))
                {
                    return "ZZ";
                }
                return null;
            }

            public static string? Parse(string? s)
            {
                var str = s.Buffer().Unify();
                if (str.IsEmpty())
                {
                    return null;
                }
#pragma warning disable S1067 // Expressions should not be too complex
                // here for reference only.
                else if (Qowaiv.Unknown.IsUnknown(str))
                {
                    return "ZZ";
                }
                else if (str.Length >= 12 && str.Length <= 36
                    && Pattern.IsMatch(str)
                    && ValidForCountry(str)
                    && Mod97(str))
                {
                    return str;
                }
#pragma warning restore S1067 // Expressions should not be too complex
                return null;
            }

            private static bool ValidForCountry(string iban)
               => Country.TryParse(iban[..2], out var country)
               && !country.IsEmptyOrUnknown()
               && (!LocalizedPatterns.TryGetValue(country, out var localizedPattern)
               || localizedPattern.IsMatch(iban));

            private static bool Mod97(string iban)
            {
                var mod = 0;
                for (var i = 0; i < iban.Length; i++)
                {
                    var digit = iban[(i + 4) % iban.Length]; // Calculate the first 4 characters (country and checksum) last
                    var index = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(digit);
                    mod *= index > 9 ? 100 : 10;
                    mod += index;
                    mod %= 97;
                }
                return mod == 1;
            }

            /// <summary>Constructs a <see cref="Regex"/> based on its BBAN pattern.</summary>
            [Pure]
            private static KeyValuePair<Country, Regex> Bban(Country country, string bban, int? checksum = default)
            {
                var pattern = new StringBuilder(64)
                    .Append('^')
                    .Append(country.IsoAlpha2Code)
                    .Append(checksum.HasValue ? checksum.Value.ToString("00") : "[0-9][0-9]");

                foreach (var block in bban.Split(','))
                {
                    var type = block.Last();

                    if (!int.TryParse(block[..^1], out int length) && type == ']')
                    {
                        pattern.Append(block[1..^1]);
                    }
                    else
                    {
                        pattern.Append(CharType(type)).Append('{').Append(length).Append('}');
                    }
                }
                return new KeyValuePair<Country, Regex>(country, new Regex(pattern.Append('$').ToString(), RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)));

                static string CharType(char type) => type switch { 'n' => "[0-9]", 'a' => "[A-Z]", 'c' => "[0-9A-Z]", _ => throw new FormatException() };
            }
            private static readonly Dictionary<Country, Regex> LocalizedPatterns = new[]
            {
                Bban(Country.AD, "8n,12c"),
                Bban(Country.AE, "3n,16n"),
                Bban(Country.AL, "8n,16c"),
                Bban(Country.AO, "21n"),
                Bban(Country.AT, "16n"),
                Bban(Country.AZ, "4a,20c"),
                Bban(Country.BA, "16n", checksum: 39),
                Bban(Country.BE, "12n"),
                Bban(Country.BF, "2c,22n"),
                Bban(Country.BG, "4a,6n,8c"),
                Bban(Country.BH, "4a,14c"),
                Bban(Country.BI, "5n,5n,11n,2n"),
                Bban(Country.BJ, "2c,22n"),
                Bban(Country.BR, "23n,1a,1c"),
                Bban(Country.BY, "4c,4n,16c"),
                Bban(Country.CF, "23n"),
                Bban(Country.CG, "23n"),
                Bban(Country.CH, "5n,12c"),
                Bban(Country.CI, "1a,23n"),
                Bban(Country.CM, "23n"),
                Bban(Country.CR, "[0],17n"),
                Bban(Country.CV, "21n"),
                Bban(Country.CY, "8n,16c"),
                Bban(Country.CZ, "20n"),
                Bban(Country.DE, "18n"),
                Bban(Country.DJ, "23n"),
                Bban(Country.DK, "14n"),
                Bban(Country.DO, "4c,20n"),
                Bban(Country.DZ, "22n"),
                Bban(Country.EE, "16n"),
                Bban(Country.EG, "25n"),
                Bban(Country.ES, "20n"),
                Bban(Country.FI, "14n"),
                Bban(Country.FO, "14n"),
                Bban(Country.FR, "10n,11c,2n"),
                Bban(Country.GA, "23n"),
                Bban(Country.GB, "4a,14n"),
                Bban(Country.GE, "2a,16n"),
                Bban(Country.GI, "4a,15c"),
                Bban(Country.GL, "14n"),
                Bban(Country.GQ, "23n"),
                Bban(Country.GR, "7n,16c"),
                Bban(Country.GT, "4c,20c"),
                Bban(Country.GW, "2c,19n"),
                Bban(Country.HN, "4a,20n"),
                Bban(Country.HR, "17n"),
                Bban(Country.HU, "24n"),
                Bban(Country.IE, "4a,14n"),
                Bban(Country.IL, "19n"),
                Bban(Country.IQ, "4a,15n"),
                Bban(Country.IR, "[0],2n,[0],18n"),
                Bban(Country.IS, "22n"),
                Bban(Country.IT, "1a,10n,12c"),
                Bban(Country.JO, "4a,4n,18c"),
                Bban(Country.KM, "23n"),
                Bban(Country.KW, "4a,22c"),
                Bban(Country.KZ, "3n,13c"),
                Bban(Country.LB, "4n,20c"),
                Bban(Country.LC, "4a,24c"),
                Bban(Country.LI, "5n,12c"),
                Bban(Country.LT, "16n"),
                Bban(Country.LU, "3n,13c"),
                Bban(Country.LV, "4a,13c"),
                Bban(Country.LY, "21n"),
                Bban(Country.MA, "24n"),
                Bban(Country.MC, "10n,11c,2n"),
                Bban(Country.MD, "2c,18c"),
                Bban(Country.ME, "18n", checksum: 25),
                Bban(Country.MG, "23n"),
                Bban(Country.MK, "3n,10c,2n", checksum: 07),
                Bban(Country.ML, "2c,22n"),
                Bban(Country.MR, "23n", checksum: 13),
                Bban(Country.MT, "4a,5n,18c"),
                Bban(Country.MU, "4a,16n,[000],3a"),
                Bban(Country.MZ, "21n"),
                Bban(Country.NE, "2a,22n"),
                Bban(Country.NI, "4a,24n"),
                Bban(Country.NL, "4a,10n"),
                Bban(Country.NO, "11n"),
                Bban(Country.PK, "4a,16c"),
                Bban(Country.PL, "24n"),
                Bban(Country.PS, "4a,21c"),
                Bban(Country.PT, "21n", checksum: 50),
                Bban(Country.QA, "4a,21c"),
                Bban(Country.RO, "4a,16c"),
                Bban(Country.RS, "18n", checksum: 35),
                Bban(Country.RU, "14n,15c"),
                Bban(Country.SA, "2n,18c"),
                Bban(Country.SC, "4a,20n,3a"),
                Bban(Country.SD, "14n"),
                Bban(Country.SE, "20n"),
                Bban(Country.SI, "15n", checksum: 56),
                Bban(Country.SK, "20n"),
                Bban(Country.SM, "1a,10n,12c"),
                Bban(Country.SN, "2a,22n"),
                Bban(Country.ST, "21n"),
                Bban(Country.SV, "4a,20n"),
                Bban(Country.TD, "23n"),
                Bban(Country.TG, "2a,22n"),
                Bban(Country.TL, "19n", checksum: 38),
                Bban(Country.TN, "20n", checksum: 59),
                Bban(Country.TR, "5n,[0],16c"),
                Bban(Country.UA, "6n,19c"),
                Bban(Country.VA, "3n,15n"),
                Bban(Country.VG, "4a,16n"),
                Bban(Country.XK, "4n,10n,2n"),
            }
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }

}
