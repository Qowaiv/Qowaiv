using System.Runtime.CompilerServices;

namespace System;

internal static class QowaivArrayExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Exists<T>(this T[] array, Predicate<T> match)
        => Array.Exists(array, match);
}
