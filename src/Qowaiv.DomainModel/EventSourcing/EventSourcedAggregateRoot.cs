using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Validation;
using Qowaiv.DomainModel.Dynamic;
using System;
using System.Diagnostics;

namespace Qowaiv.DomainModel.EventSourcing
{
    /// <summary>Represents an (domain-driven design) aggregate root that is based on event sourcing.</summary>
    /// <typeparam name="TAggrgate">
    /// The type of the aggregate root itself.
    /// </typeparam>
    public abstract class EventSourcedAggregateRoot<TAggrgate> : AggregateRoot<TAggrgate>
        where TAggrgate : EventSourcedAggregateRoot<TAggrgate>
    {
        /// <summary>Creates a new instance of an <see cref="EventSourcedAggregateRoot{TAggrgate}"/>.</summary>
        protected EventSourcedAggregateRoot() : this(null) { }

        /// <summary>Creates a new instance of an <see cref="EventSourcedAggregateRoot{TAggrgate}"/>.</summary>
        protected EventSourcedAggregateRoot(AnnotatedModelValidator validator) : base(validator)
        {
            InitProperty(nameof(Id), Guid.NewGuid());
            EventStream = new EventStream(Id);
        }

        /// <summary>Gets the event stream representing the state of the aggregate root.</summary>
        public EventStream EventStream { get; private set; }

        /// <summary>Gets the version of the aggregate root.</summary>
        public int Version => EventStream.Version;

        /// <summary>Applies a change.</summary>
        protected Result<TAggrgate> ApplyChange(object @event)
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
                    EventStream.Add(@event);
                }
                return result;
            }
        }

        /// <summary>Loads the state of the aggregate root based on historical events.</summary>
        internal Result<TAggrgate> LoadEvents(EventStream stream)
        {
            Tracker.Intialize();

            InitProperty(nameof(Id), stream.AggregateId);
            EventStream = new EventStream(Id, stream.Version);

            foreach (var e in stream)
            {
                AsDynamic().Apply(e.Event);
            }
            return Tracker.Validate();
        }

        /// <summary>Represents the aggregate root as a dynamic.</summary>
        /// <remarks>
        /// By default, this dynamic is only capable of invoking Apply(@event).
        /// If more is wanted, this method should be overridden.
        /// </remarks>
        protected virtual dynamic AsDynamic()
        {
            if (_dynamic is null)
            {
                _dynamic = new DynamicApplyEventObject(this);
            }
            return _dynamic;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private dynamic _dynamic;
    }
}
