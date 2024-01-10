namespace Qowaiv.Text;

/// <summary>Provides written numeral words for numbers.</summary>
/// <remarks>
/// See: https://en.wikipedia.org/wiki/English_numerals.
/// </remarks>
public static class Numerals
{
    private const string NL = nameof(NL);
    private const string EN = nameof(EN);

    /// <summary>Represents the numbers as numeral.</summary>
    /// <param name="number">
    /// The number to represent as a numeral.
    /// </param>
    /// <param name="culture">
    /// The culture to of the numeral.
    /// </param>
    [Pure]
    public static string ToNumeral(this long number, CultureInfo? culture)
    {
        culture ??= CultureInfo.CurrentCulture;
        if (culture.Name.StartsWith("nl")) return Dutch(number);
        else return culture.Name switch
        {
            "en-IE" or
            "en-AU" or
            "en-NZ" or
            "en-GB" => English(number, british: true),
            _ => English(number),
        };
    }

    /// <summary>Converts the Integer to written text in Dutch.</summary>
    [Pure]
    private static string Dutch(long number)
    {
        if (number == 0) return Zero[NL];
        else
        {
            var sb = new StringBuilder().AppendSign(number, NL);
            var groups = Group.Select(number);
            var multiplier = groups.Count;

            foreach (var group in groups.Reverse())
            {
                multiplier--;
                // "honderd" not "een honderd"
                if (group.Hundreds == 1) sb.Append(Hundred[NL]);
                else if (group.Hundreds > 1) sb.Append(Unit[NL][group.Hundreds]).Append(Hundred[NL]);

                if (multiplier == 1 && group.Value == 1) { /* not "eenduizend" but "duizend" */ }
                else if (group.TensAndUnits < 20)
                {
                    sb.Append(Unit[NL][group.TensAndUnits]);
                }
                else
                {
                    sb.Append(Unit[NL][group.Units])
                        // tussen-en/-ën
                        .AppendIf(group.Units > 0 && group.Tens > 0, () => sb[sb.Length - 1] == 'e' ? "ën" : "en")
                        .Append(Ten[NL][group.Tens]);
                }
                // x duizend, miljoen etc...
                sb.AppendIf(group.Value > 0, () => Multiplier[NL][multiplier]);
            }
            return sb.ToString().TrimEnd();
        }
    }

    /// <summary>Converts the Integer to written text in American English.</summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/English_numerals.
    /// </remarks>
    [Pure]
    private static string English(long number, bool british = false)
    {
        if (number == 0) return Zero[EN];
        else
        {
            var sb = new StringBuilder().AppendSign(number, EN);
            var groups = Group.Select(number);
            var multiplier = groups.Count;

            foreach (var group in groups.Reverse())
            {
                multiplier--;
                sb.AppendIf(group.Hundreds > 0, () => Unit[EN][group.Hundreds] + Hundred[EN]);

                // For British, the first group contains a and, once a hundred is present.
                if (british && groups.Count > 1 && multiplier == groups.Count - 1 && group.Hundreds > 0)
                {
                    sb.Append(" and ");
                }

                if (group.TensAndUnits < 20)
                {
                    sb.Append(Unit[EN][group.TensAndUnits]);
                }
                else
                {
                    sb.Append(Ten[EN][group.Tens])
                        .AppendIf(group.Units > 0, () => $"-{Unit[EN][group.Units]}");
                }
                // x thousand, million etc...
                sb.AppendIf(group.Value > 0, () => $" {Multiplier[EN][multiplier]}");
            }

            sb.Replace("  ", " ");
            return sb.ToString().Trim();
        }
    }

    [FluentSyntax]
    private static StringBuilder AppendIf(this StringBuilder sb, bool condition, Func<string> append)
        => condition ? sb.Append(append()) : sb;

    [FluentSyntax]
    private static StringBuilder AppendSign(this StringBuilder sb, long number, string language)
    {
        if (number < 0)
        {
            sb.Append(Sign[language]);
        }
        return sb;
    }

    private readonly struct Group
    {
        public Group(long number)
        {
            Value = (int)Math.Abs(number % 1000);
            Hundreds = Value / 100;
            Tens = (Value / 10) % 10;
            Units = Value % 10;
            TensAndUnits = Value % 100;
        }

        public readonly int Value;
        public readonly int Hundreds;
        public readonly int Tens;
        public readonly int Units;
        public readonly int TensAndUnits;

        [Pure]
        public static IReadOnlyList<Group> Select(long number)
        {
            var blocks = new List<Group>();
            while (number != 0)
            {
                blocks.Add(new(number));
                number /= 1000;
            }
            return blocks;
        }
    }

    private static readonly Dictionary<string, string> Zero = new()
    {
        [NL] = "nul",
        [EN] = "zero",
    };

    private static readonly Dictionary<string, string> Sign = new()
    {
        [NL] = "min ",
        [EN] = "minus ",
    };

    private static readonly Dictionary<string, string> Hundred = new()
    {
        [NL] = "honderd ",
        [EN] = " hundred ",
    };

    private static readonly Dictionary<string, string[]> Multiplier = new()
    {
        [NL] = [string.Empty, "duizend ", " miljoen ", " miljard ", " biljoen ", " biljard ", " triljoen ", " triljard "],
        [EN] = [string.Empty, "thousand ", " million ", " billion ", " trillion ", " quadrillion ", " quintillion ", " sextillion "],
    };

    private static readonly Dictionary<string, string[]> Ten = new()
    {
        [NL] = [string.Empty, string.Empty, "twintig", "dertig", "veertig", "vijftig", "zestig", "zeventig", "tachtig", "negentig"],
        [EN] = [string.Empty, string.Empty, "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"],
    };

    private static readonly Dictionary<string, string[]> Unit = new()
    {
        [NL] = [string.Empty, "een", "twee", "drie", "vier", "vijf", "zes", "zeven", "acht", "negen", "tien", "elf", "twaalf", "dertien", "veertien", "vijftien", "zestien", "zeventien", "achttien", "negentien"],
        [EN] = [string.Empty, "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"],
    };
}
