using System.Runtime.CompilerServices;

namespace System;

internal static class QowaivArrayExtensions
{
    extension<T>(T[] array)
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Exists(Predicate<T> match)
            => Array.Exists(array, match);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool None() => array.Length == 0;
    }
}
