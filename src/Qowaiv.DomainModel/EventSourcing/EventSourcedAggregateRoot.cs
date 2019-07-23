using Qowaiv.DomainModel.Dynamic;
using Qowaiv.Validation.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        /// <param name="validator">
        /// A custom validator.
        /// </param>
        protected EventSourcedAggregateRoot(IValidator<TAggrgate> validator) : this(Guid.NewGuid(), validator) { }

        /// <summary>Creates a new instance of an <see cref="EventSourcedAggregateRoot{TAggrgate}"/>.</summary>
        /// <param name="id">
        /// The identifier of the aggregate root.
        /// </param>
        /// <param name="validator">
        /// A custom validator.
        /// </param>
        protected EventSourcedAggregateRoot(Guid id, IValidator<TAggrgate> validator) : base(id, validator)
        {
            EventStream = new EventStream(id);
        }

        /// <inheritdoc />
        public sealed override Guid Id => EventStream.AggregateId;

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

        /// <summary>Applies the changes.</summary>
        protected Result<TAggrgate> ApplyChanges(params object[] events) => ApplyChanges(events?.AsEnumerable());

        /// <summary>Applies the changes.</summary>
        protected Result<TAggrgate> ApplyChanges(IEnumerable<object> events)
        {
            var all = Guard.NotNull(events, nameof(events)).ToArray();

            lock (EventStream.Lock())
            {
                var result = TrackChanges((self) =>
                {
                    foreach (var @event in all)
                    {
                        self.AsDynamic().Apply(@event);
                    }
                });
                if (result.IsValid)
                {
                    foreach (var @event in all)
                    {
                        EventStream.Add(@event);
                    }
                }
                return result;
            }
        }

        /// <summary>Loads the state of the aggregate root based on historical events.</summary>
        internal Result<TAggrgate> LoadEvents(EventStream stream)
        {
            Tracker.Intialize();
            EventStream = new EventStream(stream.AggregateId, stream.Version);

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
