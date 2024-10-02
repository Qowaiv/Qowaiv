using System.Runtime.CompilerServices;

namespace System;

/// <summary>Extensions on <see cref="long" />.</summary>
internal static class QowaivInt64Extensions
{
    /// <summary>Gets the absolute value of the <see cref="long" />.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Abs(this long number) => Math.Abs(number);

    /// <summary>Gets the sign of the <see cref="long" />.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(this long number) => Math.Sign(number);

    /// <summary>Returns true if the number is in the specified range.</summary>
    [Pure]
    public static bool IsInRange(this long num, long low, long max) => num >= low && num <= max;
}
