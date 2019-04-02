#pragma warning disable S4035
// Classes implementing "IEquatable<T>" should be sealed
// The Implementation takes types into account, and uses an equality comparer.

using Qowaiv.DomainModel.Tracking;
using Qowaiv.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) entity.</summary>
    /// <typeparam name="TEntity">
    /// The type of the entity itself.
    /// </typeparam>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class Entity<TEntity> : IEntity<TEntity>, IValidatableObject
        where TEntity : Entity<TEntity>
    {
        private readonly PropertyCollection _properties;

        /// <summary>Creates a new instance of an <see cref="Entity{Tentity}"/>.</summary>
        /// <param name="tracker">
        /// </param>
        protected Entity(ChangeTracker tracker)
        {
            _properties = PropertyCollection.Create(GetType());
            Tracker = Guard.NotNull(tracker, nameof(tracker));
        }

        /// <inheritdoc />
        public Guid Id
        {
            get => GetProperty<Guid>();
            set => SetImmutableProperty(value);
        }

        /// <summary>Gets the change tracker.</summary>
        protected virtual ChangeTracker Tracker { get; }

        /// <summary>Validates if the <see cref="Entity{TEntity}"/> is valid.</summary>
        /// <remarks>
        /// This rules are triggered automatically on every change to guarantee
        /// that the entity is always valid.
        /// </remarks>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => Enumerable.Empty<ValidationResult>();

        /// <summary>Gets a property (value).</summary>
        protected T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            return (T)_properties[propertyName];
        }

        /// <summary>Sets a property (value).</summary>
        /// <exception cref="ValidationException">
        /// If the new value violates the property constraints.
        /// </exception>
        protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            Tracker.Add(new PropertyChanged(_properties, propertyName, value));
        }

        /// <summary>Sets a property (value).</summary>
        /// <exception cref="ValidationException">
        /// If the new value violates the property constraints, including the
        /// its immutability.
        /// </exception>
        protected void SetImmutableProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            var current = GetProperty<T>(propertyName);
            if (!QowaivType.IsNullOrDefaultValue(current))
            {
                throw new ValidationException(QowaivDomainModelMessages.ImmutableAttribute_ErrorMessage);
            }
            SetProperty(value, propertyName);
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
        private string DebuggerDisplay => $"{base.ToString()}, ID: {(default(Guid).Equals(Id) ? "?" : Id.ToString())}";

        /// <summary>The comparer that deals with equals and hash codes.</summary>
        private static readonly EntityEqualityComparer<TEntity> _comparer = new EntityEqualityComparer<TEntity>();
    }
}
