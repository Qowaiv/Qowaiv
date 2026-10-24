#if !NET8_0_OR_GREATER
namespace System.Collections.Frozen;

internal sealed class FrozenSet<T>(ISet<T> dictionary, IEqualityComparer<T> comparer)
    : HashSet<T>(dictionary, comparer) where T : notnull
{
    /// <summary>Gets an empty <see cref="FrozenSet{T}"/>.</summary>
    public static FrozenSet<T> Empty { get; } = new FrozenSet<T>(new HashSet<T>(), EqualityComparer<T>.Default);
}
#endif
