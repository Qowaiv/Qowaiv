using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.Statistics
{
    /// <summary>Extension on Elo.</summary>
    public static class EloExtensions
    {
        /// <summary>Gets the average Elo.</summary>
        public static Elo Avarage(this IEnumerable<Elo> elos)
        {
            var doubles = elos.Select(elo => (double)elo);
            return doubles.Average();
        }
    }
}
