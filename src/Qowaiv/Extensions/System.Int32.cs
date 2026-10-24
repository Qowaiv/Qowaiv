namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="int" />.</summary>
internal static class QowaivInt32
{
    extension(int num)
    {
        /// <summary>Returns true if the number is in the specified range.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInRange(int low, int max) => num >= low && num <= max;

        /// <summary>Gets a (positive) modulo.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Mod(int modulo)
        {
            var m = num % modulo;
            return m < 0 ? m + modulo : m;
        }
    }
}
