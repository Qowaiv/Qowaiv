using Qowaiv.ComponentModel;
using Qowaiv.DomainModel.Dynamic;
using Qowaiv.DomainModel.Events;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) aggregate root.</summary>
    /// <typeparam name="TAggrgate">
    /// The type of the aggregate root itself.
    /// </typeparam>
    public abstract class AggregateRoot<TAggrgate> : Entity<TAggrgate>
        where TAggrgate : AggregateRoot<TAggrgate>
    {
        /// <summary>Gets the event stream representing the state of the aggregate root.</summary>
        public EventStream EventStream { get; } = new EventStream();

        /// <summary>Gets the version of the aggregate root.</summary>
        public int Version => EventStream.Version;

        /// <summary>Applies a change.</summary>
        protected Result<TAggrgate> ApplyChange(IEvent @event)
        {
            Guard.NotNull(@event, nameof(@event));

            lock (EventStream.Lock())
            {
                var result = TrackChanges((self) =>
                {
                    self.AsDynamic().Apply(@event);
                });
                if (result.IsValid)
                {
                    EventStream.AddUncommited(@event);
                }
                return result;
            }
        }

        /// <summary>Loads the state of the aggregate root based on historical events.</summary>
        internal Result<TAggrgate> LoadFromHistory(IEvent[] events)
        {
            var result = TrackChanges((self) =>
            {
                // Set ID:
                //self.Id = Guard.NotDefault(events[0].Id, "events.Id")

                foreach (var e in events)
                {
                    self.AsDynamic().Apply(e);
                }
            });
            if (result.IsValid)
            {
                EventStream.AddCommitted(events);
            }
            return result;
        }

        /// <summary>Represents the aggregate root as a dynamic.</summary>
        /// <remarks>
        /// By default, this dynamic is only capable of invoking Apply(@event).
        /// If more is wanted, this method should be overriden.
        /// </remarks>
        protected virtual dynamic AsDynamic()
        {
            if(_dynamic is null)
            {
                _dynamic = new DynamicApplyEventObject(this);
            }
            return _dynamic;
        }
        private dynamic _dynamic;
    }
}
