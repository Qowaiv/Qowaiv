#pragma warning disable S4035
// Classes implementing "IEquatable<T>" should be sealed
// The Implementation takes types into account, and uses an equality comparer.

using Qowaiv.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) entity.</summary>
    /// <typeparam name="TId">
    /// The type of the identifier.
    /// </typeparam>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class Entity<TId> : IEntity<TId> where TId : struct
    {
        private readonly EntityChangeTracker<TId> _tracker;

        /// <summary>Creates a new instance of an <see cref="Entity{TId}"/>.</summary>
        protected Entity()
        {
            Properties = PropertyCollection.Create(this);
            _tracker = new EntityChangeTracker<TId>(this);
        }

        /// <summary>Creates a new instance of an <see cref="Entity{TId}"/>.</summary>
        /// <param name="id">
        /// The identifier of the entity.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the identifier has the default (transient) value.
        /// </exception>
        protected Entity(TId id) : this()
        {
            Id = id;
        }

        /// <summary>Gets the (editable) properties of the entity.</summary>
        public PropertyCollection Properties { get; }

        /// <inheritdoc />
        [Mandatory, Immutable]
        public TId Id
        {
            get => GetProperty<TId>();
            set => SetProperty(value);
        }

        /// <inheritdoc />
        public bool IsTransient => default(TId).Equals(Id);

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Notifies that the <see cref="Property"/> changed.</summary>
        internal bool NotifyPropertyChanged(Property property)
        {
            if(PropertyChanged is null)
            {
                return false;
            }
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property.PropertyType.Name));
            return true;
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
        protected T GetProperty<T>([CallerMemberName] string propertyName = null) => (T)Properties[propertyName].Value;

        /// <summary>Sets a property (value).</summary>
        /// <exception cref="ValidationException">
        /// If the new value violates the property constraints.
        /// </exception>
        /// <remarks>
        /// This will trigger the <see cref="PropertyChanged"/> on a change.
        /// </remarks>
        protected void SetProperty(object value, [CallerMemberName] string propertyName = null)
        {
            var property = Properties[propertyName];
            _tracker.AddPropertyChange(property, value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Entity<TId>);

        /// <inheritdoc />
        public bool Equals(IEntity<TId> other)
        {
            return _comparer.Equals(this, other);
        }

        /// <summary>Returns true if left and right are equal.</summary>
        public static bool operator ==(Entity<TId> left, Entity<TId> right) => _comparer.Equals(left, right);

        /// <summary>Returns false if left and right are equal.</summary>
        public static bool operator !=(Entity<TId> left, Entity<TId> right) => !(left == right);

        /// <inheritdoc />
        public override int GetHashCode() => _comparer.GetHashCode(this);

        /// <summary>Represents the entity as a DEBUG <see cref="string"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"{base.ToString()}, ID: {(IsTransient ? "?" : Id.ToString())}";

        /// <summary>The comparer that deals with equals and hash codes.</summary>
        private static readonly EntityEqualityComparer<TId> _comparer = new EntityEqualityComparer<TId>();
    }
}
