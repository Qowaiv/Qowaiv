using Qowaiv;

namespace System;

/// <summary>Extensions on <see cref="ushort"/>.</summary>
[CLSCompliant(false)]
public static class QowaivUInt16Extensions
{
    /// <summary>Adds the specified percentage to the <see cref="ushort"/>.</summary>
    /// <param name="num">
    /// The value to add a percentage to.
    /// </param>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public static ushort Add(this ushort num, Percentage p) => (ushort)(num + num.Multiply(p));

    /// <summary>Subtracts the specified percentage to the <see cref="ushort"/>.</summary>
    /// <param name="num">
    /// The value to Subtract a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage to Subtract.
    /// </param>
    [Pure]
    public static ushort Subtract(this ushort num, Percentage p) => (ushort)(num - num.Multiply(p));

    /// <summary>Gets the specified percentage of the <see cref="ushort"/>.</summary>
    /// <param name="num">
    /// The value to get a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage.
    /// </param>
    [Pure]
    public static ushort Multiply(this ushort num, Percentage p) => (ushort)((decimal)num).Multiply(p);

    /// <summary>Divides the <see cref="ushort"/> by the specified percentage.</summary>
    /// <param name="num">
    /// The value to divide.
    /// </param>
    /// <param name="p">
    /// The percentage to divide to.
    /// </param>
    [Pure]
    public static ushort Divide(this ushort num, Percentage p) => (ushort)((decimal)num).Divide(p);
}
