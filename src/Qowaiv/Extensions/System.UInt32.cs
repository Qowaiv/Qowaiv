using Qowaiv;

namespace System;

/// <summary>Extensions on <see cref="uint"/>.</summary>
[CLSCompliant(false)]
public static class QowaivUInt32Extensions
{
    /// <summary>Adds the specified percentage to the <see cref="uint"/>.</summary>
    /// <param name="num">
    /// The value to add a percentage to.
    /// </param>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public static uint Add(this uint num, Percentage p) => num + num.Multiply(p);

    /// <summary>Subtracts the specified percentage to the <see cref="uint"/>.</summary>
    /// <param name="num">
    /// The value to Subtract a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage to Subtract.
    /// </param>
    [Pure]
    public static uint Subtract(this uint num, Percentage p) => num - num.Multiply(p);

    /// <summary>Gets the specified percentage of the <see cref="uint"/>.</summary>
    /// <param name="num">
    /// The value to get a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage.
    /// </param>
    [Pure]
    public static uint Multiply(this uint num, Percentage p) => (uint)((decimal)num).Multiply(p);

    /// <summary>Divides the <see cref="uint"/> by the specified percentage.</summary>
    /// <param name="num">
    /// The value to divide.
    /// </param>
    /// <param name="p">
    /// The percentage to divide to.
    /// </param>
    [Pure]
    public static uint Divide(this uint num, Percentage p) => (uint)((decimal)num).Divide(p);
}
