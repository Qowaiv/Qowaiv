using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Validation;
using Qowaiv.DomainModel.Tracking;
using System;

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
        /// <inheritdoc />
        protected new ChangeTracker<TAggrgate> Tracker => (ChangeTracker<TAggrgate>)base.Tracker;
    }
}
