using Qowaiv.Mathematics;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="double" />.</summary>
public static class QowaivDoubleExtensions
{
    extension(double value)
    {
        /// <summary>Returns true if the number is in the specified range.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsInRange(double low, double max) => value >= low && value <= max;

        /// <inheritdoc  cref="DecimalMath.Round(decimal, int, DecimalRounding)" />
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal Round(int decimals, DecimalRounding mode)
            => DecimalMath.Round((decimal)value, decimals, mode);
    }
}
