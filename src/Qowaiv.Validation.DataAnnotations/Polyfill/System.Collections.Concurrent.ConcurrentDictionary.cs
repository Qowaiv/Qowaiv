#if NETSTANDARD2_0
namespace System.Collections.Concurrent;

internal static class QowaivConcurrentDictionaryExtensions
{
    extension<TKey, TValue, TArg>(ConcurrentDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        [Impure]
        public TValue GetOrAdd(
            TKey key,
            Func<TKey, TArg, TValue> valueFactory,
            TArg factoryArgument)
            => dictionary.GetOrAdd(key, k => valueFactory(k, factoryArgument));
    }
}
#endif
