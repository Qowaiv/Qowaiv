#if NET10_0_OR_GREATER

namespace System.Numerics;

/// <summary>Extensions on <see cref="INumberBase{TSelf}" />.</summary>
public static class QowaivINumberBase
{
    /// <typeparam name="TSelf">
    /// Type of the number.
    /// </typeparam>
    extension<TSelf>(TSelf? number) where TSelf : struct, INumberBase<TSelf>
    {
        /// <summary>Returns zero is the <paramref name="number" /> is null.</summary>
        [Pure]
        public TSelf ZeroIfNull()
            => number is TSelf value
            ? value
            : TSelf.Zero;
    }
}
#endif
