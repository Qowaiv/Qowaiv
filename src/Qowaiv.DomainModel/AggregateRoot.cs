using Qowaiv.DomainModel.Tracking;
using Qowaiv.Validation.Abstractions;
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
        /// <param name="validator">
        /// A custom validator.
        /// </param>
        protected AggregateRoot(IValidator<TAggrgate> validator)
            : this(Guid.NewGuid(), validator) { }

        /// <summary>Creates a new instance of an <see cref="AggregateRoot{TAggrgate}"/>.</summary>
        /// <param name="id">
        /// The identifier of the aggregate root.
        /// </param>
        /// <param name="validator">
        /// A custom validator.
        /// </param>
        protected AggregateRoot(Guid id, IValidator<TAggrgate> validator)
            : base(id, new ChangeTracker<TAggrgate>())
        {
            Tracker.Init((TAggrgate)this, validator);
        }

        /// <summary>Initializes multiple properties simultaneously, without triggering validation.</summary>
        /// <param name="initialize">
        /// The action trying to initialize the state of the properties.
        /// </param>
        /// <remarks>
        /// Should only be called via the constructor.
        /// </remarks>
        protected void Initialize(Action initialize)
        {
            Guard.NotNull(initialize, nameof(initialize));

            Tracker.Intialize();
            initialize();
            Tracker.Mode = default;
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

            Tracker.BufferChanges();
            update((TAggrgate)this);
            return Tracker.Process();
        }
        /// <inheritdoc />
        protected new ChangeTracker<TAggrgate> Tracker => (ChangeTracker<TAggrgate>)base.Tracker;
    }
}
