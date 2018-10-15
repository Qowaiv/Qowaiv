#pragma warning disable S4035
// Classes implementing "IEquatable<T>" should be sealed
// The Implementation takes types into account, and uses an equality comparer.

using System;
using System.ComponentModel;
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
            _properties = EntityPropertyCollection.Create(GetType());
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
            Guard.NotDefault(id, nameof(id));
            SetId(id);
        }

        /// <inheritdoc />
        public TId Id => m_Id;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TId m_Id;

        /// <inheritdoc />
        public bool IsTransient => default(TId).Equals(Id);

        /// <summary>Initializes the identifier of the entity.</summary>
        protected void SetId(TId id)
        {
            if (!IsTransient)
            {
                throw new NotSupportedException(QowaivDomainModelMessages.NotSupported_UpdateEntityId);
            }
            m_Id = Guard.NotDefault(id, nameof(id));
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        protected T GetProperty<T>([CallerMemberName] string propertyName = null) => (T)_properties[propertyName].Value;

        protected void SetProperty(object value, [CallerMemberName] string propertyName = null)
        {
            if (_properties[propertyName].SetValue(value, this) && PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
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

        private readonly EntityPropertyCollection _properties;

        /// <summary>The comparer that deals with equals and hash codes.</summary>
        private static readonly EntityEqualityComparer<TId> _comparer = new EntityEqualityComparer<TId>();
    }
}
