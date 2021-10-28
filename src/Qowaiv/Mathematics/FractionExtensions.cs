using System.Diagnostics.Contracts;

namespace Qowaiv.Mathematics
{
    /// <summary>Extensions to increase the usability of <see cref="Fraction"/>.</summary>
    public static class FractionExtensions
    {
        /// <summary>Divides the <paramref name="numerator"/> by the <paramref name="denominator"/>.</summary>
        [Pure]
        public static Fraction DividedBy(this int numerator, long denominator)=> ((long)numerator).DividedBy(denominator);

        /// <summary>Divides the <paramref name="numerator"/> by the <paramref name="denominator"/>.</summary>
        [Pure]
        public static Fraction DividedBy(this long numerator, long denominator)=> new Fraction(numerator, denominator);
    }
}
