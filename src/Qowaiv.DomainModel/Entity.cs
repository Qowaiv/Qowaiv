#pragma warning disable S4035
// Classes implementing "IEquatable<T>" should be sealed
// The Implementation takes types into account, and uses an equality comparer.

using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Validation;
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
    /// <typeparam name="TId">
    /// The type of the identifier.
    /// </typeparam>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class Entity<TEntity, TId> : IEntity<TEntity, TId>, IValidatableObject 
        where TEntity : Entity<TEntity, TId>
        where TId : struct
    {
        private readonly EntityChangeTracker<TEntity, TId> _tracker;
        private readonly PropertyCollection _properties;

        /// <summary>Creates a new instance of an <see cref="Entity{Tentity, TId}"/>.</summary>
        /// <param name="validator">
        /// The validator to validate the entity with.
        /// </param>
        protected Entity(AnnotatedModelValidator validator)
        {
            _properties = PropertyCollection.Create(this);
            _tracker = new EntityChangeTracker<TEntity, TId>((TEntity)this, _properties, validator);
        }

        /// <summary>Creates a new instance of an <see cref="Entity{TEntity, TId}"/>.</summary>
        protected Entity() : this(null) { }

        /// <inheritdoc />
        public TId Id
        {
            get => GetProperty<TId>();
            set => SetImmutableProperty(value);
        }

        /// <summary>Validates if the <see cref="Entity{TEntity, TId}"/> is valid.</summary>
        /// <remarks>
        /// This rules are triggered automatically on every change to guarantee
        /// that the entity is always valid.
        /// </remarks>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => Enumerable.Empty<ValidationResult>();

        /// <summary>Sets multiple properties simultaneously.</summary>
        /// <param name="update">
        /// The action trying to update the state of the properties.
        /// </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the entity or the messages.
        /// </returns>
        public Result<TEntity> SetProperties(Action<TEntity> update)
        {
            Guard.NotNull(update, nameof(update));

            _tracker.BufferChanges = true;
            update((TEntity)this);
            return _tracker.ProcessChanges();
        }

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
            _tracker.AddPropertyChange(propertyName, value);
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
        public static bool operator ==(Entity<TEntity, TId> left, Entity<TEntity, TId> right) => _comparer.Equals((TEntity)left, (TEntity)right);

        /// <summary>Returns false if left and right are equal.</summary>
        public static bool operator !=(Entity<TEntity, TId> left, Entity<TEntity, TId> right) => !(left == right);

        /// <inheritdoc />
        public override int GetHashCode() => _comparer.GetHashCode((TEntity)this);

        /// <summary>Represents the entity as a DEBUG <see cref="string"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"{base.ToString()}, ID: {(default(TId).Equals(Id) ? "?" : Id.ToString())}";

        /// <summary>The comparer that deals with equals and hash codes.</summary>
        private static readonly EntityEqualityComparer<TEntity, TId> _comparer = new EntityEqualityComparer<TEntity, TId>();
    }
}
