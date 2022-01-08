namespace Qowaiv.Mathematics;

internal static partial class FractionParser
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
    /// integer     => long.Parse()
    /// </remarks>
    [Pure]
    public static Fraction? Parse(string s, NumberFormatInfo formatInfo)
        => External(s, formatInfo) 
        ?? Complex(s.Buffer(), formatInfo);

    [Impure]
    private static Fraction? Complex(this CharBuffer buffer, NumberFormatInfo numberInfo)
    {
        var s = buffer.Sign(numberInfo);
        
        // double signs.
        if (s.HasValue && buffer.Sign(numberInfo) is { }) return null;
        
        var sign = s ?? 1;
        if (buffer.Fraction(numberInfo) is { } fractional)
        {
            if (buffer.IsEmpty())
            {
                return fractional * sign;
            }
            else if (buffer.Separator(numberInfo).Integer(numberInfo, out var integer)
                && Calculate(sign, integer, fractional.Numerator, fractional.Denominator, out var numerator))
            {
                return numerator.DividedBy(fractional.Denominator);
            }
            else return null;
        }
        else return null;
    }

    [Impure]
    private static Fraction? Fraction(this CharBuffer buffer, NumberFormatInfo numberInfo)
        => buffer.Vulgar()
        ?? buffer.Regular(numberInfo);

    [Impure]
    private static Fraction? Vulgar(this CharBuffer buffer)
    {
        if (Vulgars.TryGetValue(buffer.Last(), out var vulgar))
        {
            buffer.RemoveFromEnd(1);
            return vulgar;
        }
        else return null;
    }

    [Impure]
    private static Fraction? Regular(this CharBuffer buffer, NumberFormatInfo numberInfo)
        => buffer.Denominator(numberInfo, out var denominator)
        && buffer.Bar()
        && buffer.Numerator(numberInfo, out var numerator)
        ? numerator.DividedBy(denominator)
        : null;

    private static bool Numerator(this CharBuffer buffer, NumberFormatInfo numberInfo, out long numerator)
        => buffer.Integer(Mathematics.Fraction.Formatting.SuperScript, numberInfo, out numerator);

    private static bool Denominator(this CharBuffer buffer, NumberFormatInfo numberInfo, out long denominator)
        => buffer.Integer(Mathematics.Fraction.Formatting.SubScript, numberInfo, out denominator);

    private static bool Integer(this CharBuffer buffer, string lookup, NumberFormatInfo numberInfo, out long number)
    {
        long factor = 1;
        number = 0;
        while (buffer.NotEmpty())
        {
            var ch = buffer.Last();
            if (lookup.IndexOf(ch) is var index && index != CharBuffer.NotFound)
            {
                buffer.RemoveFromEnd(1);
                number += index * factor;
                factor *= 10;
            }
            else break;
        }
        return factor > 1 || buffer.Integer(numberInfo, out number);
    }

    private static bool Integer(this CharBuffer buffer, NumberFormatInfo numberInfo, out long integer)
    {
        integer = default;
        var index = buffer.Length;
        var exit = Mathematics.Fraction.Formatting.FractionBars 
            + numberInfo.PositiveSign 
            + numberInfo.Separator();

        while (index > 0 && exit.IndexOf(buffer[index - 1]) == CharBuffer.NotFound)
        {
            index--;
        }
        if (index < buffer.Length && long.TryParse(buffer.Substring(index), IntegerStyle, numberInfo, out integer))
        {
            buffer.RemoveFromEnd(buffer.Length - index);
            return true;
        }
        else return false;
    }

    [Impure]
    private static bool Bar(this CharBuffer buffer)
    {
        if (buffer.NotEmpty() && Mathematics.Fraction.Formatting.IsFractionBar(buffer.Last()))
        {
            buffer.RemoveFromEnd(1);
            return true;
        }
        else return false;
    }

    [Impure]
    private static CharBuffer Separator(this CharBuffer buffer, NumberFormatInfo numberInfo)
    {
        if (buffer.Last() == numberInfo.Separator())
        {
            buffer.RemoveFromEnd(1);
        }
        return buffer;
    }
      
    [Impure]
    private static int? Sign(this CharBuffer buffer, NumberFormatInfo formatInfo)
    {
        if (buffer.StartsWith(formatInfo.NegativeSign))
        {
            buffer.RemoveFromStart(formatInfo.NegativeSign.Length);
            return -1;
        }
        else if (buffer.StartsWith(formatInfo.PositiveSign))
        {
            buffer.RemoveFromStart(formatInfo.PositiveSign.Length);
            return +1;
        }
        else return null; 
    }

    private static bool Calculate(int sign, long integer, long numerator, long denominator, out long result)
    {
        try
        {
            result = checked((integer * denominator + numerator) * sign);
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
    private static Fraction? External(string s, IFormatProvider formatInfo)
    {
        if (long.TryParse(s, IntegerStyle, formatInfo, out var lng) && ValidLong(lng))
        {
            return Mathematics.Fraction.Create(lng);
        }
        else if (decimal.TryParse(s, DecimalStyle, formatInfo, out var dec) && ValidDec(dec))
        {
            return Mathematics.Fraction.Create(dec);
        }
        else if (PotentialPercentage(s) && Percentage.TryParse(s, formatInfo, out var percentage) && ValidDec((decimal)percentage))
        {
            return Mathematics.Fraction.Create((decimal)percentage);
        }
        else return null;

        static bool ValidLong(long number) => number != long.MinValue;
        static bool ValidDec(decimal number) => number >= Mathematics.Fraction.MinValue.Numerator && number <= Mathematics.Fraction.MaxValue.Numerator;
    }

    [Pure]
    private static char Separator(this NumberFormatInfo numberInfo) => numberInfo.NumberGroupSeparator == " " ? '+' : ' ';

    /// <summary>Only strings containing percentage markers (%, ‰, ‱) should be parsed by <see cref="Percentage.TryParse(string)"/>.</summary>
    [Pure]
    private static bool PotentialPercentage(string str) => str.Any(ch => "%‰‱".IndexOf(ch) != CharBuffer.NotFound);

    private const NumberStyles IntegerStyle = NumberStyles.Integer | NumberStyles.AllowThousands ^ NumberStyles.AllowTrailingSign;
    private const NumberStyles DecimalStyle = IntegerStyle | NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign;
    private static readonly Dictionary<char, Fraction> Vulgars = new()
    {
        { '½', 1.DividedBy(2) },

        { '⅓', 1.DividedBy(3) },
        { '⅔', 2.DividedBy(3) },

        { '¼', 1.DividedBy(4) },
        { '¾', 3.DividedBy(4) },

        { '⅕', 1.DividedBy(5) },
        { '⅖', 2.DividedBy(5) },
        { '⅗', 3.DividedBy(5) },
        { '⅘', 4.DividedBy(5) },

        { '⅙', 1.DividedBy(6) },
        { '⅚', 5.DividedBy(6) },

        { '⅐', 1.DividedBy(7) },

        { '⅛', 1.DividedBy(8) },
        { '⅜', 3.DividedBy(8) },
        { '⅝', 5.DividedBy(8) },
        { '⅞', 7.DividedBy(8) },

        { '⅑', 1.DividedBy(9) },
        { '⅒', 1.DividedBy(10) },

        { '↉', Mathematics.Fraction.Zero },
    };
}
