using Qowaiv.Statistics;

namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int number)
    {
        /// <summary>Interprets the <see cref="int" /> if it was written with a '%' sign.</summary>
        [Pure]
        public Elo Elo() => number;
    }
}
