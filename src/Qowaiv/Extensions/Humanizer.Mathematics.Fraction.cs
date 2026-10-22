namespace Qowaiv.Mathematics;

/// <summary>Extensions to create <see cref="Percentage" />s, inspired by Humanizer.NET.</summary>
public static class NumberToFractionExtensions
{
    extension(int numerator)
    {
        /// <summary>Divides the <paramref name="numerator" /> by the <paramref name="denominator" />.</summary>
        [Pure]
        public Fraction DividedBy(long denominator) => ((long)numerator).DividedBy(denominator);
    }

    extension(long numerator)
    {
        /// <summary>Divides the <paramref name="numerator" /> by the <paramref name="denominator" />.</summary>
        [Pure]
        public Fraction DividedBy(long denominator) => new(numerator, denominator);
    }

    extension(decimal number)
    {
        /// <summary>Converts the <see cref="decimal" /> to a <see cref="Mathematics.Fraction" />.</summary>
        [Pure]
        public Fraction Fraction() => Mathematics.Fraction.Create(number);
    }

    extension(double number)
    {
        /// <summary>Converts the <see cref="double" /> to a <see cref="Mathematics.Fraction" />.</summary>
        [Pure]
        public Fraction Fraction() => Mathematics.Fraction.Create(number);
    }
}
