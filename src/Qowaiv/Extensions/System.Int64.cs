using Qowaiv;

namespace System;

/// <summary>Extensions on <see cref="long"/>.</summary>
public static class QowaivInt64Extensions
{
    /// <summary>Gets the absolute value of the <see cref="long"/>.</summary>
    [Pure]
    public static long Abs(this long number) => Math.Abs(number);

    /// <summary>Adds the specified percentage to the <see cref="long"/>.</summary>
    /// <param name="num">
    /// The value to add a percentage to.
    /// </param>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public static long Add(this long num, Percentage p) => num + num.Multiply(p);

    /// <summary>Subtracts the specified percentage to the <see cref="long"/>.</summary>
    /// <param name="num">
    /// The value to Subtract a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage to Subtract.
    /// </param>
    [Pure]
    public static long Subtract(this long num, Percentage p) => num - num.Multiply(p);

    /// <summary>Gets the specified percentage of the <see cref="long"/>.</summary>
    /// <param name="num">
    /// The value to get a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage.
    /// </param>
    [Pure]
    public static long Multiply(this long num, Percentage p) => (long)((decimal)num).Multiply(p);

    /// <summary>Divides the <see cref="long"/> by the specified percentage.</summary>
    /// <param name="num">
    /// The value to divide.
    /// </param>
    /// <param name="p">
    /// The percentage to divide to.
    /// </param>
    [Pure]
    public static long Divide(this long num, Percentage p) => (long)((decimal)num).Divide(p);

    /// <summary>Gets the sign of the <see cref="long"/>.</summary>
    [Pure]
    public static int Sign(this long number) => Math.Sign(number);

    /// <summary>Returns true if the number is in the specified range.</summary>
    [Pure]
    internal static bool IsInRange(this long num, long low, long max) => num >= low && num <= max;
}
