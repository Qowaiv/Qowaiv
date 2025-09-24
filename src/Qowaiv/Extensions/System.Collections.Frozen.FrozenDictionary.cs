#if NET8_0_OR_GREATER
#else

namespace System.Collections.Frozen;

internal sealed class FrozenDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
    : Dictionary<TKey, TValue>(dictionary) where TKey : notnull;

internal static class QowaivFrozenDictionaryExtensions
{
    [Pure]
    public static FrozenDictionary<TKey, TValue> ToFrozenDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dic) where TKey : notnull
        => new(dic);
}

#endif
