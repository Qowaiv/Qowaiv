using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.FirstPopCoffeeCompany.RoastPlanning.Events
{
    public class SelectRoastDays
    {
        public SelectRoastDays(params Date[] roastdDays)
        {
            Selected = roastdDays.ToArray();
        }
        public IReadOnlyCollection<Date> Selected { get; }
    }
}
