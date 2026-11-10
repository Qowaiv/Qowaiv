namespace System.Linq;

/// <summary>Extensions on arrays.</summary>
internal static class QowaivDomainModelArrayExtensions
{
    /// <inheritdoc cref="Array.Find{T}(T[], Predicate{T})" />
    [Pure]
    public static T? TryFind<T>(this T[] array, Predicate<T> match)
        => Array.Find(array, match);
}
