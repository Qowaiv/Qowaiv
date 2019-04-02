using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Validation;
using Qowaiv.DomainModel.Dynamic;
using Qowaiv.DomainModel.Events;
using Qowaiv.DomainModel.Tracking;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) aggregate root.</summary>
    /// <typeparam name="TAggrgate">
    /// The type of the aggregate root itself.
    /// </typeparam>
    public abstract class AggregateRoot<TAggrgate> : Entity<TAggrgate>
        where TAggrgate : AggregateRoot<TAggrgate>
    {
        /// <summary>Creates a new instance of an <see cref="AggregateRoot{TAggrgate}"/>.</summary>
        protected AggregateRoot() : this(null) { }

        /// <summary>Creates a new instance of an <see cref="AggregateRoot{TAggrgate}"/>.</summary>
        /// <param name="validator">
        /// A custom validator.
        /// </param>
        protected AggregateRoot(AnnotatedModelValidator validator)
            : base(new ChangeTracker<TAggrgate>())
        {
            Tracker.Init((TAggrgate)this, validator ?? new AnnotatedModelValidator());
        }

        /// <summary>Sets multiple properties simultaneously.</summary>
        /// <param name="update">
        /// The action trying to update the state of the properties.
        /// </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the entity or the messages.
        /// </returns>
        public Result<TAggrgate> TrackChanges(Action<TAggrgate> update)
        {
            Guard.NotNull(update, nameof(update));

            Tracker.BufferChanges = true;
            update((TAggrgate)this);
            return Tracker.Process();
        }

        /// <summary>Gets the event stream representing the state of the aggregate root.</summary>
        public EventStream EventStream { get; } = new EventStream();

        /// <summary>Gets the version of the aggregate root.</summary>
        public int Version => EventStream.Version;

        /// <inheritdoc />
        protected new ChangeTracker<TAggrgate> Tracker => (ChangeTracker<TAggrgate>)base.Tracker;

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

        /// <summary>Adds an element to the child collection.</summary>
        protected void Add<TChild>(ChildCollection<TChild> collection, TChild item)
        {
            Tracker.Add(new ItemAdded<TChild>(collection, item));
        }

        /// <summary>Adds elements to the child collection.</summary>
        protected void AddRange<TChild>(ChildCollection<TChild> collection, IEnumerable<TChild> items)
        {
            Guard.NotNull(items, nameof(items));
            foreach(var item in items)
            {
                Add(collection, item);
            }
        }

        /// <summary>Removes an element from the child collection.</summary>
        protected void Remove<TChild>(ChildCollection<TChild> collection, TChild item)
        {
            Tracker.Add(new ItemRemoved<TChild>(collection, item));
        }

        /// <summary>Removes all elements from the child collection.</summary>
        protected void Clear<TChild>(ChildCollection<TChild> collection)
        {
            Tracker.Add(new ClearedCollection<TChild>(collection));
        }

        /// <summary>Represents the aggregate root as a dynamic.</summary>
        /// <remarks>
        /// By default, this dynamic is only capable of invoking Apply(@event).
        /// If more is wanted, this method should be overridden.
        /// </remarks>
        protected virtual dynamic AsDynamic()
        {
            if(_dynamic is null)
            {
                _dynamic = new DynamicApplyEventObject(this);
            }
            return _dynamic;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private dynamic _dynamic;
    }
}
