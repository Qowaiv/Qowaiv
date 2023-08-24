namespace Qowaiv.Mathematics;

internal static class FractionParser
{
    /// <summary>Parses a faction.</summary>
    /// <remarks>
    /// Fraction Grammar
    ///
    /// parse       => [external] | [complex]
    /// external    => long.Parse() | decimal.Parse() | Percentage.Parse()
    /// complex     => [sign] [integer]? [fraction]
    /// fraction    => [vulgar] | [regular]
    /// regular     => [numerator] [bar] [denominator]
    /// numerator   => [integer] | [super]+
    /// denominator => [integer] | [sub]+
    /// vulgar      => ½|⅓|⅔|..
    /// sign        => (+|-)?
    /// bar         => /|:|÷|..
    /// super       => ⁰|¹|²|³|⁴|⁵|⁶|⁷|⁸|⁹
    /// sub         => ₀|₁|₂|₃|₄|₅|₆|₇|₈|₉
    /// integer     => long.Parse().
    /// </remarks>
    [Pure]
    public static Fraction? Parse(string s, IFormatProvider? provider)
        => External(s, provider) ?? Complex(s.CharSpan(), provider);

    [Pure]
    private static Fraction? Complex(CharSpan span, IFormatProvider? provider)
#pragma warning disable S1067 // Expressions should not be too complex
        => Fractional(span, provider, out var fraction) is { } fractional
        && Integer(fractional.TrimRight(), provider, out var integer) is var number
        && Sign(number ?? fractional, provider, out var sign)
        && Calculate(sign, integer, fraction.Numerator, fraction.Denominator, out var numerator)
        ? numerator.DividedBy(fraction.Denominator)
        : null;
#pragma warning restore S1067 // Expressions should not be too complex

    private static CharSpan? Fractional(CharSpan span, IFormatProvider? provider, out Fraction fraction)
        => Vulgar(span, out fraction) ?? Regular(span, provider, out fraction);

    private static CharSpan? Vulgar(CharSpan span, out Fraction fraction)
    {
        var next = span.Last(out var ch);
        return Vulgars.TryGetValue(ch, out fraction)
            ? next : null;
    }

    private static CharSpan? Regular(CharSpan span, IFormatProvider? provider, out Fraction fraction)
    {
        fraction = default;
        if (Denominator(span, provider, out var denominator) is { } first
            && Bar(first) is { } bar
            && Numerator(bar, provider, out var numerator) is { } next)
        {
            fraction = numerator.DividedBy(denominator);
            return next;
        }
        else return null;
    }

    private static CharSpan? Denominator(CharSpan span, IFormatProvider? provider, out long denominator)
    {
        var next = Integer(span, provider, out denominator)
            ?? Integer(span, Fraction.Formatting.SubScript, out denominator);
        return denominator > 0 ? next : null;
    }

    private static CharSpan? Numerator(CharSpan span, IFormatProvider? provider, out long numerator)
        => Integer(span, provider, out numerator)
        ?? Integer(span, Fraction.Formatting.SuperScript, out numerator);

    [Pure]
    private static CharSpan? Bar(CharSpan span)
    {
        var next = span.Last(out char ch);
        return Fraction.Formatting.IsFractionBar(ch) ? next : null;
    }

    private static CharSpan? Integer(CharSpan span, string digits, out long integer)
    {
        var next = span.TrimRight(ch => digits.Contains(ch), out var trimmed);
        integer = 0;
        foreach (var ch in trimmed)
        {
            integer *= 10;
            integer += digits.IndexOf(ch);
        }
        return trimmed.NotEmpty() ? next : null;
    }

    private static CharSpan? Integer(CharSpan span, IFormatProvider? provider, out long integer)
    {
        var exit = Fraction.Formatting.FractionBars
            + provider.PositiveSign()
            + provider.NegativeSign()
            + provider.Separator();

        var next = span.TrimRight(ch => !exit.Contains(ch), out var trimmed);
        return long.TryParse(trimmed.ToString(), IntegerStyle, provider, out integer)
            ? next : null;
    }

    public static bool Sign(CharSpan span, IFormatProvider? provider, out int sign)
    {
        sign = span.Equals(provider.NegativeSign()) ? -1 : +1;
        return sign == -1 || span.IsEmpty() || span.Equals(provider.PositiveSign());
    }

    private static bool Calculate(int sign, long integer, long numerator, long denominator, out long result)
    {
        try
        {
            result = checked(((integer * denominator) + numerator) * sign);
            return true;
        }
        catch (OverflowException)
        {
            result = default;
            return false;
        }
    }

    /// <summary>Parse the fraction, using <see cref="long.Parse(string)"/>, <see cref="decimal.Parse(string)"/>, and <see cref="Percentage.Parse(string)"/>.</summary>
    [Pure]
    private static Fraction? External(string s, IFormatProvider? formatInfo)
    {
        if (long.TryParse(s, IntegerStyle, formatInfo, out var lng) && ValidLong(lng))
        {
            return Fraction.Create(lng);
        }
        else if (decimal.TryParse(s, DecimalStyle, formatInfo, out var dec) && ValidDec(dec))
        {
            return Fraction.Create(dec);
        }
        else if (PotentialPercentage(s) && Percentage.TryParse(s, formatInfo, out var percentage) && ValidDec((decimal)percentage))
        {
            return Fraction.Create((decimal)percentage);
        }
        else return null;

        static bool ValidLong(long number) => number != long.MinValue;
        static bool ValidDec(decimal number) => number >= Fraction.MinValue.Numerator && number <= Fraction.MaxValue.Numerator;
    }

    [Pure]
    private static char Separator(this IFormatProvider? provider)
    {
        var seperator = provider?.GetFormat<NumberFormatInfo>()?.NumberGroupSeparator ?? ".";
        return seperator == " " ? '+' : ' ';
    }

    /// <summary>Only strings containing percentage markers (%, ‰, ‱) should be parsed by <see cref="Percentage.TryParse(string)"/>.</summary>
    [Pure]
    private static bool PotentialPercentage(string str) => str.Any(ch => "%‰‱".Contains(ch));

    private const NumberStyles IntegerStyle = NumberStyles.Integer | NumberStyles.AllowThousands ^ NumberStyles.AllowTrailingSign;
    private const NumberStyles DecimalStyle = IntegerStyle | NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign;

    private static readonly Dictionary<char, Fraction> Vulgars = new()
    {
        ['½'] = 1.DividedBy(2),

        ['⅓'] = 1.DividedBy(3),
        ['⅔'] = 2.DividedBy(3),

        ['¼'] = 1.DividedBy(4),
        ['¾'] = 3.DividedBy(4),

        ['⅕'] = 1.DividedBy(5),
        ['⅖'] = 2.DividedBy(5),
        ['⅗'] = 3.DividedBy(5),
        ['⅘'] = 4.DividedBy(5),

        ['⅙'] = 1.DividedBy(6),
        ['⅚'] = 5.DividedBy(6),

        ['⅐'] = 1.DividedBy(7),

        ['⅛'] = 1.DividedBy(8),
        ['⅜'] = 3.DividedBy(8),
        ['⅝'] = 5.DividedBy(8),
        ['⅞'] = 7.DividedBy(8),

        ['⅑'] = 1.DividedBy(9),
        ['⅒'] = 1.DividedBy(10),

        ['↉'] = Fraction.Zero,
    };
}
