using System.Runtime.CompilerServices;

namespace System;

internal static class QowaivArrayExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Find<T>(this T[] array, Predicate<T> match)
        => Array.Find(array, match);
}
