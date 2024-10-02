namespace Qowaiv.Mathematics;

/// <summary>Extensions to create <see cref="Percentage" />s, inspired by Humanizer.NET.</summary>
public static class NumberToFractionExtensions
{
    /// <summary>Divides the <paramref name="numerator" /> by the <paramref name="denominator" />.</summary>
    [Pure]
    public static Fraction DividedBy(this int numerator, long denominator) => ((long)numerator).DividedBy(denominator);

    /// <summary>Divides the <paramref name="numerator" /> by the <paramref name="denominator" />.</summary>
    [Pure]
    public static Fraction DividedBy(this long numerator, long denominator) => new(numerator, denominator);

    /// <summary>Converts the <see cref="decimal" /> to a <see cref="Qowaiv.Mathematics.Fraction" />.</summary>
    [Pure]
    public static Fraction Fraction(this decimal number) => Qowaiv.Mathematics.Fraction.Create(number);

    /// <summary>Converts the <see cref="double" /> to a <see cref="Qowaiv.Mathematics.Fraction" />.</summary>
    [Pure]
    public static Fraction Fraction(this double number) => Qowaiv.Mathematics.Fraction.Create(number);
}
