using System;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>Extensions for and/or on <see cref="IEvent"/>.</summary>
    public static class EventExtensions
    {
        public static IEnumerable<VersionedEvent> Versioned(this IEnumerable<IEvent> events, Guid aggregateId = default(Guid))
        {
            Guard.NotNull(events, nameof(events));
            var id = aggregateId == Guid.Empty ? Guid.NewGuid() : aggregateId;
            var version = 1;
            return events.Select(e => new VersionedEvent(new EventInfo(version++, id, Clock.UtcNow()), e));
        }
    }
}
