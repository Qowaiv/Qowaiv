namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="decimal" />.</summary>
internal static class QowaivDecimalExtensions
{
    extension(decimal value)
    {
        /// <summary>Returns true if the number is in the specified range.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInRange(decimal low, decimal max) => value >= low && value <= max;
    }
}
