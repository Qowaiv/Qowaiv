using System.Runtime.CompilerServices;

namespace System;

/// <summary>Extensions on <see cref="int"/>.</summary>
internal static class QowaivInt32Extensions
{
    /// <summary>Gets a (positive) modulo.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Mod(this int n, int modulo)
    {
        var m = n % modulo;
        return m < 0 ? m + modulo : m;
    }
}
