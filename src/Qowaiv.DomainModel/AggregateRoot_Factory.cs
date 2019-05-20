using Qowaiv.ComponentModel;
using Qowaiv.DomainModel.EventSourcing;

namespace Qowaiv.DomainModel
{
    /// <summary>Factory to create <see cref="AggregateRoot{TAggregate}"/>s.</summary>
    public static class AggregateRoot
    {
        /// <summary>Loads an aggregate root from historical events.</summary>
        public static Result<TAggregate> FromEvents<TAggregate>(EventStream stream)
             where TAggregate : EventSourcedAggregateRoot<TAggregate>, new()
        {
            Guard.NotNull(stream, nameof(stream));

            if(!stream.ContainsFullHistory)
            {
                throw new EventStreamNoFullHistoryException(nameof(stream));
            }
            var aggregateRoot = new TAggregate();
            return aggregateRoot.LoadEvents(stream);
        }
    }
}
