#if NET8_0_OR_GREATER

namespace System.Numerics;

/// <summary>Extensions on <see cref="INumberBase{TSelf}" />.</summary>
public static class QowaivNumberBaseExtensions
{
    /// <summary>Returns zero is the <paramref name="number" /> is null.</summary>
    /// <typeparam name="TSelf">
    /// Type of the number.
    /// </typeparam>
    [Pure]
    public static TSelf ZeroIfNull<TSelf>(this TSelf? number) where TSelf : struct, INumberBase<TSelf>
        => number is TSelf value
        ? value
        : TSelf.Zero;
}
#endif
