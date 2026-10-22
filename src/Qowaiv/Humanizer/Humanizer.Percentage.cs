using Qowaiv.Mathematics;

namespace Qowaiv.Humanizer;

/// <summary>Extensions to create <see cref="Percentage" />s, inspired by Humanizer.NET.</summary>
public static class NumberToPercentageExtensions
{
    extension(int number)
    {
        /// <summary>Interprets the <see cref="int" /> as if it was written with a '%' sign.
        /// <example>E.g:
        /// 12.Percent() => 12%
        /// </example>
        /// </summary>
        [Pure]
        public Percentage Percent() => ((decimal)number).Percent();
    }

    extension(double number)
    {
        /// <summary>Interprets the <see cref="double" /> as if it was written with a '%' sign.
        /// <example>E.g:
        /// 12.0.Percent() => 12%
        /// 0.1.Percent() => 0.1%
        /// </example>
        /// </summary>
        [Pure]
        [Obsolete("Use decimals instead.")]
        public Percentage Percent() => Cast.ToDecimal<Percentage>(number).Percent();
    }

    extension(decimal number)
    {
        /// <summary>Interprets the <see cref="decimal" /> as if it was written with a '%' sign.
        /// <example>E.g:
        /// 12m.Percent() => 12%
        /// 0.1m.Percent() => 0.1%
        /// </example>
        /// </summary>
        [Pure]
        public Percentage Percent() => Percentage.Create(DecimalMath.ChangeScale(number, -2));
    }
}
