namespace Qowaiv;

/// <summary>Helper for easier to read code.</summary>
internal static class Statics
{
    /// <summary>Creates a froen set.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FrozenSet<T> frozen<T>(params IEnumerable<T> items) where T : notnull, IEquatable<T>
        => new HashSet<T>(items).ToFrozenSet();

    /// <summary>Creates a froen set.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FrozenSet<string> frozen_case_insensitive(params IEnumerable<string> items)
        => new HashSet<string>(items).ToFrozenSet(StringComparer.OrdinalIgnoreCase);
}
