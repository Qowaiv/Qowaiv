#if NETSTANDARD2_0
namespace System.Collections.Frozen;

internal sealed class FrozenSet<T>(IEnumerable<T> collection) : HashSet<T>(collection);

internal static class QowaivFrozenExtensions
{
    extension<T>(IEnumerable<T> set) where T : notnull
    {
        [Pure]
        public FrozenSet<T> ToFrozenSet() => new(set);
    }
}
#endif
