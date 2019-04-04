using Qowaiv.ComponentModel;
using Qowaiv.DomainModel.EventSourcing;

namespace Qowaiv.DomainModel
{
    /// <summary>Factory to create <see cref="AggregateRoot{TAggrgate}"/>s.</summary>
    public static class AggregateRoot
    {
        /// <summary>Loads an aggregate root from historical events.</summary>
        public static Result<TAggrgate> FromEventStream<TAggrgate>(EventStream stream)
             where TAggrgate : EventSourcedAggregateRoot<TAggrgate>, new()
        {
            Guard.NotNull(stream, nameof(stream));
            var aggregateRoot = new TAggrgate();
            return aggregateRoot.LoadEvents(stream);
        }
    }
}
