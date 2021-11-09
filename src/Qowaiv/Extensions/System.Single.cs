using Qowaiv;

namespace System;

/// <summary>Extensions on <see cref="float"/>.</summary>
public static class QowaivSingleExtensions
{
    /// <summary>Adds the specified percentage to the <see cref="float"/>.</summary>
    /// <param name="d">
    /// The value to add a percentage to.
    /// </param>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public static float Add(this float d, Percentage p) => d + d.Multiply(p);

    /// <summary>Subtracts the specified percentage to the <see cref="float"/>.</summary>
    /// <param name="d">
    /// The value to Subtract a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage to Subtract.
    /// </param>
    [Pure]
    public static float Subtract(this float d, Percentage p) => d - d.Multiply(p);

    /// <summary>Gets the specified percentage of the <see cref="float"/>.</summary>
    /// <param name="d">
    /// The value to get a percentage from.
    /// </param>
    /// <param name="p">
    /// The percentage.
    /// </param>
    [Pure]
    public static float Multiply(this float d, Percentage p) => (float)((decimal)d).Multiply(p);

    /// <summary>Divides the <see cref="float"/> by the specified percentage.</summary>
    /// <param name="d">
    /// The value to divide.
    /// </param>
    /// <param name="p">
    /// The percentage to divide to.
    /// </param>
    [Pure]
    public static float Divide(this float d, Percentage p) => (float)((decimal)d).Divide(p);
}
