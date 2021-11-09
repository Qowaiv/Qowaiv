namespace Qowaiv;

/// <summary>Extensions on <see cref="DecimalRounding"/> rounding.</summary>
public static class DecimalRoundingExtensions
{
    /// <summary>Returns true if the rounding is direct; the nearest of the two options is not relevant.</summary>
    [Pure]
    public static bool IsDirectRounding(this DecimalRounding mode)
        => mode >= DecimalRounding.Truncate && mode <= DecimalRounding.Floor;

    /// <summary>Returns true if rounding is to the nearest. These modes have half-way tie-breaking rule.</summary>
    [Pure]
    public static bool IsNearestRounding(this DecimalRounding mode)
        => mode >= DecimalRounding.ToEven && mode <= DecimalRounding.RandomTieBreaking;

}
