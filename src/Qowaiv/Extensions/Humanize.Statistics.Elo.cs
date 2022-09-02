namespace Qowaiv.Statistics;

/// <summary>Extensions to create <see cref="Statistics.Elo"/>s, inspired by Humanizer.NET.</summary>
public static class NumberToEloExtensions
{
    /// <summary>Interprets the <see cref="int"/> if it was written with a '%' sign.</summary>
    [Pure]
    public static Elo Elo(this int number) => number;
}
