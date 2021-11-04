using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Qowaiv.Statistics
{
    /// <summary>Extension on Elo.</summary>
    public static class EloExtensions
    {
        /// <summary>Gets the average Elo.</summary>
        [Pure]
        public static Elo Avarage(this IEnumerable<Elo> elos) => elos.Select(elo => (double)elo).Average();
    }
}
