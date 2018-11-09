#pragma warning disable S4035
// Classes implementing "IEquatable<T>" should be sealed
// The Implementation takes types into account, and uses an equality comparer.

using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.DomainModel.Persistence;
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
    public class Entity<TId> : IEntity<TId> where TId : struct
    {
        /// <summary>Creates a new instance of an <see cref="Entity{TId}"/>.</summary>
        public Entity()
        {
            Properties =  PropertyCollection.Create(this);
        }

        /// <summary>Creates a new instance of an <see cref="Entity{TId}"/>.</summary>
        /// <param name="id">
        /// The identifier of the entity.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the identifier has the default (transient) value.
        /// </exception>
        public Entity(TId id) : this()
        {
            Properties[nameof(Id)].Init(Guard.NotDefault(id, nameof(id)));
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
        public bool IsTransient => default(TId).Equals(Id) || Properties[nameof(Id)].IsDirty;

        /// <summary>Returns true if any of the properties is dirty, otherwise false.</summary>
        public bool IsDirty => Properties.IsDirty;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Gets the delta of the properties.</summary>
        public Delta GetDelta() => Properties.GetDelta();

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
            if (Properties[propertyName].Set(value) && PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>Loads a property (value).</summary>
        /// <remarks>
        /// Will not trigger any validation constraints, and clears a potential dirty flag.
        /// </remarks>
        protected void InitProperty(object value, string propertyName)
        {
            Properties[propertyName].Init(value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Entity<TId>);

        /// <inheritdoc />
        public bool Equals(IEntity<TId> other)
        {
            return _comparer.Equals(this, other);
        }

        /// <summary>Returns true if left and right are equal.</summary>
        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            return _comparer.Equals(left, right);
        }

        /// <summary>Returns false if left and right are equal.</summary>
        public static bool operator !=(Entity<TId> left, Entity<TId> right) => !(left == right);

        /// <inheritdoc />
        public override int GetHashCode() => _comparer.GetHashCode(this);

        /// <summary>Represents the entity as a DEBUG <see cref="string"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get => $"{base.ToString()}, ID: {(IsTransient ? "?" : Id.ToString())}";
        }

        /// <summary>The comparer that deals with equals and hash codes.</summary>
        private static readonly EntityEqualityComparer<TId> _comparer = new EntityEqualityComparer<TId>();
    }
}
