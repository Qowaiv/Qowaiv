namespace System;

/// <summary>Extensions on <see cref="double" />.</summary>
internal static class QowaivDoubleExtensions
{
    /// <summary>Returns true if the number is in the specified range.</summary>
    [Pure]
    public static bool IsInRange(this double num, double low, double max) => num >= low && num <= max;
}
