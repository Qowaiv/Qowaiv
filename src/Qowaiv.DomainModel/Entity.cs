#pragma warning disable S4035
// Classes implementing "IEquatable<T>" should be sealed
// The Implementation takes types into account, and uses an equality comparer.

using Qowaiv.DomainModel.Tracking;
using Qowaiv.Validation.Abstractions;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) entity.</summary>
    /// <typeparam name="TEntity">
    /// The type of the entity itself.
    /// </typeparam>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class Entity<TEntity> : IEntity<TEntity>
        where TEntity : Entity<TEntity>
    {
        private readonly PropertyCollection _properties;

        /// <summary>Creates a new instance of an <see cref="Entity{Tentity}"/>.</summary>
        /// <param name="tracker">
        /// The change tracker of the entity.
        /// </param>
        protected Entity(ChangeTracker tracker) : this(Guid.NewGuid(), tracker) { }

        /// <summary>Creates a new instance of an <see cref="Entity{Tentity}"/>.</summary>
        /// <param name="id">
        /// The identifier of the entity.
        /// </param>
        /// <param name="tracker">
        /// The change tracker of the entity.
        /// </param>
        protected Entity(Guid id, ChangeTracker tracker)
        {
            Id = Guard.NotEmpty(id, nameof(id));
            _properties = PropertyCollection.Create(GetType());
            Tracker = Guard.NotNull(tracker, nameof(tracker));
        }

        /// <inheritdoc />
        public virtual Guid Id { get; }

        /// <summary>Gets the change tracker.</summary>
        protected virtual ChangeTracker Tracker { get; }

        /// <summary>Gets a property (value).</summary>
        protected T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            return (T)_properties[propertyName];
        }

        /// <summary>Sets a property (value).</summary>
        /// <exception cref="InvalidModelException">
        /// If the new value violates the property constraints.
        /// </exception>
        protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            Tracker.Add(new PropertyChanged(_properties, propertyName, value));
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as TEntity);

        /// <inheritdoc />
        public bool Equals(TEntity other) => _comparer.Equals((TEntity)this, other);

        /// <summary>Returns true if left and right are equal.</summary>
        public static bool operator ==(Entity<TEntity> left, Entity<TEntity> right) => _comparer.Equals((TEntity)left, (TEntity)right);

        /// <summary>Returns false if left and right are equal.</summary>
        public static bool operator !=(Entity<TEntity> left, Entity<TEntity> right) => !(left == right);

        /// <inheritdoc />
        public override int GetHashCode() => _comparer.GetHashCode((TEntity)this);

        /// <summary>Represents the entity as a DEBUG <see cref="string"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"{GetType().Name}, ID: {Id:B}";

        /// <summary>The comparer that deals with equals and hash codes.</summary>
        private static readonly EntityEqualityComparer<TEntity> _comparer = new EntityEqualityComparer<TEntity>();
    }
}
