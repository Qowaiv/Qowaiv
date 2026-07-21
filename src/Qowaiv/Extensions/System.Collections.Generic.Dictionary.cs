#if !NET8_0_OR_GREATER
namespace System.Collections.Generic;

internal static class QowaivDictionaryExtensions
{
    extension<TKey, TValue>(IDictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        [Pure]
        public FrozenDictionary<TKey, TValue> ToFrozenDictionary()
            => new(dictionary);
    }
}
#endif
