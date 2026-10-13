#if !NET8_0_OR_GREATER
namespace System.Collections.Frozen;

internal sealed class FrozenDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
    : Dictionary<TKey, TValue>(dictionary) where TKey : notnull
{
    /// <summary>Gets an empty <see cref="FrozenDictionary{TKey, TValue}"/>.</summary>
    public static FrozenDictionary<TKey, TValue> Empty => new(new Dictionary<TKey, TValue>());
}
#endif
