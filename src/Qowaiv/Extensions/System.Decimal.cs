using Qowaiv;
using System.Runtime.CompilerServices;

namespace System;

/// <summary>Extensions on <see cref="decimal"/>.</summary>
public static class QowaivDecimalExtensions
{
    /// <summary>Gets the absolute value of the <paramref name="number"/>.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static decimal Abs(this decimal number) => Math.Abs(number);

    /// <summary>Gets the sign of the <paramref name="number"/>.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int Sign(this decimal number) => Math.Sign(number);

    /// <summary>Rounds a value to the closed number that is a multiple of the specified factor.</summary>
    /// <param name="value">
    /// The value to round to.
    /// </param>
    /// <param name="multipleOf">
    /// The factor of which the number should be multiple of.
    /// </param>
    /// <returns>
    /// A rounded number that is multiple to the specified factor.
    /// </returns>
    [Pure]
    public static decimal RoundToMultiple(this decimal value, decimal multipleOf) => value.RoundToMultiple(multipleOf, DecimalRounding.ToEven);

    /// <summary>Rounds a value to the closed number that is a multiple of the specified factor.</summary>
    /// <param name="value">
    /// The value to round to.
    /// </param>
    /// <param name="multipleOf">
    /// The factor of which the number should be multiple of.
    /// </param>
    /// <param name="mode">
    /// The rounding method used to determine the closed by number.
    /// </param>
    /// <returns>
    /// A rounded number that is multiple to the specified factor.
    /// </returns>
    [Pure]
    public static decimal RoundToMultiple(this decimal value, decimal multipleOf, DecimalRounding mode)
    {
        Guard.Positive(multipleOf);
        return (value / multipleOf).Round(0, mode) * multipleOf;
    }

    /// <summary>Rounds a decimal value to the nearest integer.</summary>
    /// <param name="value">
    /// A decimal number to round.
    /// </param>
    /// <returns>
    /// The integer that is nearest to the <paramref name="value"/> parameter. If the <paramref name="value"/> is halfway between two integers,
    /// it is rounded away from zero.
    /// </returns>
    [Pure]
    public static decimal Round(this decimal value) => value.Round(0);

    /// <summary>Rounds a decimal value to a specified number of decimal places.</summary>
    /// <param name="value">
    /// A decimal number to round.
    /// </param>
    /// <param name="decimals">
    /// A value from -28 to 28 that specifies the number of decimal places to round to.
    /// </param>
    /// <returns>
    /// The decimal number equivalent to <paramref name="value"/> rounded to <paramref name="decimals"/> number of decimal places.
    /// </returns>
    /// <remarks>
    /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
    /// </remarks>
    [Pure]
    public static decimal Round(this decimal value, int decimals) => value.Round(decimals, DecimalRounding.ToEven);

    /// <summary>Rounds a decimal value to a specified number of decimal places.</summary>
    /// <param name="value">
    /// A decimal number to round.
    /// </param>
    /// <param name="decimals">
    /// A value from -28 to 28 that specifies the number of decimal places to round to.
    /// </param>
    /// <param name="mode">
    /// The mode of rounding applied.
    /// </param>
    /// <returns>
    /// The decimal number equivalent to <paramref name="value"/> rounded to <paramref name="decimals"/> number of decimal places.
    /// </returns>
    /// <remarks>
    /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
    /// </remarks>
    [Pure]
    public static decimal Round(this decimal value, int decimals, DecimalRounding mode) => DecimalRound.Round(value, decimals, mode);

    /// <summary>Returns true if the number is in the specified range.</summary>
    [Pure]
    internal static bool IsInRange(this decimal num, decimal low, decimal max) => num >= low && num <= max;
}
