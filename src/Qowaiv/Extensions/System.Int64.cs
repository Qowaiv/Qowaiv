namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="long" />.</summary>
internal static class QowaivInt64
{
    extension(long number)
    {
        /// <summary>Gets the absolute value of the <see cref="long" />.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Abs() => Math.Abs(number);

        /// <summary>Gets the sign of the <see cref="long" />.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sign() => Math.Sign(number);

        /// <summary>Returns true if the number is in the specified range.</summary>
        [Pure]
        public bool IsInRange(long low, long max) => number >= low && number <= max;
    }
}
