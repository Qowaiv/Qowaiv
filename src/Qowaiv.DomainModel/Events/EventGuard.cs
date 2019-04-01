using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>Helper class on guarding events.</summary>
    internal static class EventGuard
    {
        public static IEvent[] LoadFromHistory(IEnumerable<IEvent> events, string paramName)
        {
            var eventArray = Guard.HasAny(events?.ToArray(), paramName);
            var version = 0;

            foreach(var @event in eventArray)
            {
                Guard.NotNull(@event, paramName + '[' + version.ToString() + ']');

                if(@event.Version != ++version)
                {
                    throw new EventsOutOfOrderException(paramName);
                }
            }

            return eventArray;
        }
    }
}
