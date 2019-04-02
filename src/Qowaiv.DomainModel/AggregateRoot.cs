using Qowaiv.ComponentModel;
using Qowaiv.DomainModel.Events;
using System.Collections.Generic;

namespace Qowaiv.DomainModel
{
    /// <summary>Factory to create <see cref="AggregateRoot{TAggrgate}"/>s.</summary>
    public static class AggregateRoot
    {
        /// <summary>Loads an aggregate root from historical events.</summary>
        public static Result<TAggrgate> LoadFromHistory<TAggrgate>(IEnumerable<VersionedEvent> events)
             where TAggrgate : AggregateRoot<TAggrgate>, new()
        {
            var aggregateRoot = new TAggrgate();
            return aggregateRoot.LoadFromHistory(events);
        }
    }
}
