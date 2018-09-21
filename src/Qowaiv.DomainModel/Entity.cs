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
    public class Entity<TId> : IEntity<TId> where TId : struct
    {
        /// <inheritdoc />
        public TId Id
        {
            get => _id;
            protected set => _id = IsTransient()
                ? value 
                : throw new NotSupportedException(QowaivDomainModelMessages.NotSupported_UpdateEntityId);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TId _id;

        /// <inheritdoc />
        public bool IsTransient() => default(TId).Equals(Id);

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

        private static readonly EntityEqualityComparer<TId> _comparer = new EntityEqualityComparer<TId>();
    }
}
