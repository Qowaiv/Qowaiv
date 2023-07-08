using System.Runtime.CompilerServices;

namespace System;

internal static class QowaivArrayExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Exists<T>(this T[] array, Predicate<T> match)
        => Array.Exists(array, match);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Find<T>(this T[] array, Predicate<T> match)
       => Array.Find(array, match);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool None<T>(this T[] array) => array.Length == 0;
}
