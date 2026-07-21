#if !NET8_0_OR_GREATER
namespace System.Collections.Frozen;

internal sealed class FrozenDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
    : Dictionary<TKey, TValue>(dictionary) where TKey : notnull;
#endif
