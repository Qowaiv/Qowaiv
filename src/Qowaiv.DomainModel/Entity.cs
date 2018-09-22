#pragma warning disable S4035
// Classes implementing "IEquatable<T>" should be sealed
// The Implementation takes types into account, and uses an equality comparer.

using System;
using System.Diagnostics;

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
        public Entity() { }

        /// <summary>Creates a new instance of an <see cref="Entity{TId}"/>.</summary>
        /// <param name="id">
        /// The identifier of the entity.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the identifier has the default (transient) value.
        /// </exception>
        public Entity(TId id)
        {
            Guard.NotDefault(id, nameof(id));
            _id = id;
        }

        /// <inheritdoc />
        public TId Id
        {
            get => _id;
            protected set => _id = IsTransient
                ? Guard.NotDefault(value, nameof(Id))
                : throw new NotSupportedException(QowaivDomainModelMessages.NotSupported_UpdateEntityId);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TId _id;

        /// <inheritdoc />
        public bool IsTransient => default(TId).Equals(Id);

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
