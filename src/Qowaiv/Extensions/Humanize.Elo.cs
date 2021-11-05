using System.Diagnostics.Contracts;

namespace Qowaiv.Statistics
{
    /// <summary>Extensions to create <see cref="Statistics.Elo"/>s, inspired by Humanizer.NET.</summary>
    public static class HumanizeElo
    {
        /// <summary>Interprets the <see cref="int"/> if it was written with a '%' sign.</summary>
        [Pure]
        public static Elo Elo(this int number) => number;

        /// <summary>Interprets the <see cref="double"/> if it was written with a '%' sign.</summary>
        [Pure]
        public static Elo Percent(this double number) => number;

        /// <summary>Interprets the <see cref="decimal"/> if it was written with a '%' sign.</summary>
        [Pure]
        public static Elo Percent(this decimal number) => number;
    }
}
