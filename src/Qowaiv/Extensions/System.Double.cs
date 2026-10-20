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

        /// <summary>Casts to a <see cref="decimal" /> keeping all floating point info.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal ToDecimal() => (decimal)value;

        /// <summary>
        /// Casts with applying <see cref="DecimalMath.Round(decimal, int, DecimalRounding)" />.
        /// </summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal ToDecimal(int decimals, DecimalRounding mode)
            => DecimalMath.Round((decimal)value, decimals, mode);
    }
}
