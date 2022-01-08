using Qowaiv;

namespace System;

/// <summary>Extensions on <see cref="int"/>.</summary>
public static class QowaivInt32Extensions
{
    /// <summary>Adds the specified percentage to the <see cref="int"/>.</summary>
    /// <param name="num">
    /// The value to add a percentage to.
    /// </param>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public static int Add(this int num, Percentage p) => num + num.Multiply(p);

    /// <summary>Subtracts the specified percentage to the <see cref="int"/>.</summary>
    /// <param name="num">
    /// The value to Subtract a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage to Subtract.
    /// </param>
    [Pure]
    public static int Subtract(this int num, Percentage p) => num - num.Multiply(p);

    /// <summary>Gets the specified percentage of the <see cref="int"/>.</summary>
    /// <param name="num">
    /// The value to get a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage.
    /// </param>
    [Pure]
    public static int Multiply(this int num, Percentage p) => (int)((decimal)num).Multiply(p);

    /// <summary>Divides the <see cref="int"/> by the specified percentage.</summary>
    /// <param name="num">
    /// The value to divide.
    /// </param>
    /// <param name="p">
    /// The percentage to divide to.
    /// </param>
    [Pure]
    public static int Divide(this int num, Percentage p) => (int)((decimal)num).Divide(p);

    /// <summary>Gets a (positive) modulo.</summary>
    [Pure]
    internal static int Mod(this int n, int modulo)
    {
        var m = n % modulo;
        return m < 0 ? m + modulo : m;
    }

    /// <summary>Returns true if the number is in the specified range.</summary>
    [Pure]
    internal static bool IsInRange(this int num, int low, int max) => num >= low && num <= max;
}
