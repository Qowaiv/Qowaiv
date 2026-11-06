#if NETSTANDARD2_0
namespace System.Collections.Generic;

internal static class SetExtensions
{
    extension<TValue>(IEnumerable<TValue> @set) where TValue : notnull
    {
        [Pure]
        public FrozenSet<TValue> ToFrozenSet(IEqualityComparer<TValue>? comparer = null) => new(@set, comparer ?? EqualityComparer<TValue>.Default);
    }
}
#endif
