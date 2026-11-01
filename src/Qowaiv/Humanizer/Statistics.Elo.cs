using Qowaiv.Statistics;

namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int number)
    {
        /// <summary>Interprets the <see cref="int" /> as an <see cref="Elo" /> rating.</summary>
        [Pure]
        public Elo Elo() => number;
    }
}
