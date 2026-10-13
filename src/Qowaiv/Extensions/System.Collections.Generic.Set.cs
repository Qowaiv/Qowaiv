#if !NET8_0_OR_GREATER
namespace System.Collections.Generic;

internal static class QowaivSetExtensions
{
    extension<TValue>(ISet<TValue> @set) where TValue : notnull
    {
        [Pure]
        public FrozenSet<TValue> ToFrozenSet(IEqualityComparer<TValue> comparer) => new(@set, comparer);
    }
}
#endif
