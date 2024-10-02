using System.Runtime.CompilerServices;

namespace System;

/// <summary>Extensions on <see cref="int" />.</summary>
internal static class QowaivInt32Extensions
{
    /// <summary>Returns true if the number is in the specified range.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInRange(this int num, int low, int max) => num >= low && num <= max;

    /// <summary>Gets a (positive) modulo.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Mod(this int n, int modulo)
    {
        var m = n % modulo;
        return m < 0 ? m + modulo : m;
    }
}
