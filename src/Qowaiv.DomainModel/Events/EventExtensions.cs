namespace Qowaiv.DomainModel.Events
{
    /// <summary>Extensions for and/or on <see cref="IEvent"/>.</summary>
    public static class EventExtensions
    {
        /// <summary>Links the aggregate properties <see cref="Entity{TEntity}.Id"/>
        /// and <see cref="AggregateRoot{TAggrgate}.Version"/> to the event.
        /// </summary>
        public static TEvent Link<TEvent, TAggregate>(this TEvent @event, TAggregate aggregate)
            where TEvent: class, IEvent
            where TAggregate: AggregateRoot<TAggregate>
        {
            if(@event is null)
            {
                return null;
            }
            @event.Id = aggregate.Id;
            @event.Version = aggregate.Version + 1;
            return @event;
        }
    }
}
