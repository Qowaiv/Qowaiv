using Qowaiv.ComponentModel;
using Qowaiv.DomainModel.Events;
using System.Collections.Generic;

namespace Qowaiv.DomainModel
{
    /// <summary>Factory to create <see cref="AggregateRoot{TAggrgate}"/>s.</summary>
    public static class AggregateRoot
    {
        /// <summary>Loads an aggregate root from historical events.</summary>
        public static Result<TAggrgate> LoadFromHistory<TAggrgate>(IEnumerable<IEvent> events)
             where TAggrgate : AggregateRoot<TAggrgate>, new()
        {
            var eventArray = EventGuard.LoadFromHistory(events, nameof(events));

            var aggregateRoot = new TAggrgate();
            return aggregateRoot.LoadFromHistory(eventArray);
        }
    }
}
