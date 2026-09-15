namespace Qowaiv;

/// <summary>Extensions on <see cref="DecimalRounding" /> rounding.</summary>
public static class DecimalRoundingExtensions
{
    extension(DecimalRounding mode)
    {
        /// <summary>Returns true if the rounding is direct; the nearest of the two options is not relevant.</summary>
        [Pure]
        public bool IsDirectRounding()
            => mode is >= DecimalRounding.Truncate and <= DecimalRounding.Floor;

        /// <summary>Returns true if rounding is to the nearest. These modes have half-way tie-breaking rule.</summary>
        [Pure]
        public bool IsNearestRounding()
            => mode is >= DecimalRounding.ToEven and <= DecimalRounding.RandomTieBreaking;
    }
}
