#pragma warning disable S4035
// Classes implementing "IEquatable<T>" should be sealed
// The Implementation takes types into account, and uses an equality comparer.

using Qowaiv.ComponentModel.Validation;
using Qowaiv.DomainModel.ChangeManagement;
using Qowaiv.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) entity.</summary>
    /// <typeparam name="TId">
    /// The type of the identifier.
    /// </typeparam>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class Entity<TId> : IEntity<TId>, IValidatableObject where TId : struct
    {
        private readonly EntityChangeTracker<TId> _tracker;
        private readonly PropertyCollection _properties;

        /// <summary>Creates a new instance of an <see cref="Entity{TId}"/>.</summary>
        /// <param name="validator">
        /// The validator to validate the entity with.
        /// </param>
        protected Entity(AnnotatedModelValidator validator)
        {
            _properties = PropertyCollection.Create(this);
            _tracker = new EntityChangeTracker<TId>(this, _properties, validator);
        }

        /// <summary>Creates a new instance of an <see cref="Entity{TId}"/>.</summary>
        protected Entity() : this(null) { }

        /// <inheritdoc />
        public TId Id
        {
            get => GetProperty<TId>();
            set => SetImmutableProperty(value);
        }

        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Validates if the <see cref="Entity{TId}"/> is valid.</summary>
        /// <remarks>
        /// This rules are triggered automatically on every change to guarantee
        /// that the entity is always valid.
        /// </remarks>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => Enumerable.Empty<ValidationResult>();

        /// <summary>Notifies that the property changed.</summary>
        internal void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Sets multiple properties simultaneously.</summary>
        /// <param name="setProperties">
        /// The action initializing multiple properties.
        /// </param>
        /// <exception cref="ValidationException">
        /// If an single error occurs.
        /// </exception>
        /// <exception cref="AggregateException">
        /// If multiple errors occur.
        /// </exception>
        /// <remarks>
        /// Triggers <see cref="PropertyChangedEventHandler"/> events if all
        /// properties could be set without an error.
        /// </remarks>
        public void SetProperties(Action setProperties)
        {
            Guard.NotNull(setProperties, nameof(setProperties));
            _tracker.BufferChanges = true;
            setProperties();
            _tracker.ProcessChanges();
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
        public override bool Equals(object obj) => Equals(obj as Entity<TId>);

        /// <inheritdoc />
        public bool Equals(IEntity<TId> other) => _comparer.Equals(this, other);

        /// <summary>Returns true if left and right are equal.</summary>
        public static bool operator ==(Entity<TId> left, Entity<TId> right) => _comparer.Equals(left, right);

        /// <summary>Returns false if left and right are equal.</summary>
        public static bool operator !=(Entity<TId> left, Entity<TId> right) => !(left == right);

        /// <inheritdoc />
        public override int GetHashCode() => _comparer.GetHashCode(this);

        private bool IsTransient => default(TId).Equals(Id);

        /// <summary>Represents the entity as a DEBUG <see cref="string"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"{base.ToString()}, ID: {(IsTransient ? "?" : Id.ToString())}";

        /// <summary>The comparer that deals with equals and hash codes.</summary>
        private static readonly EntityEqualityComparer<TId> _comparer = new EntityEqualityComparer<TId>();
    }
}
