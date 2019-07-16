#pragma warning disable S4035 
// Classes implementing "IEquatable<T>" should be sealed
// It is left to actual implementations

using System;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) value object.</summary>
    /// <typeparam name="TModel">
    /// The type of the identifier.
    /// </typeparam>
    /// <remarks>
    /// This base class should not be used for Single Value Objects (SVO's).
    /// </remarks>
    public abstract class ValueObject<TModel> : IEquatable<TModel> where TModel : ValueObject<TModel>
    {
        /// <inheritdoc />
        public abstract bool Equals(TModel other);

        /// <inheritdoc />
        public sealed override bool Equals(object obj) => obj is TModel other && Equals(other);

        /// <inheritdoc />
        public sealed override int GetHashCode() => Hash();

        /// <summary>Gets a hash code for the value object.</summary>
        /// <remarks>
        /// The reason to have this abstract Hash() method, and seal the GetHashCode()
        /// method, it enforce a custom implementation of a hash function.
        /// </remarks>
        protected abstract int Hash();

        /// <summary>Returns true if the two value objects are equal, other false.</summary>
        public static bool operator ==(ValueObject<TModel> left, ValueObject<TModel> right)
        {
            if (left is null || right is null)
            {
                return ReferenceEquals(left, right);
            }
            return left.Equals(right);
        }

        /// <summary>Returns true if the two value objects are not equal, other false.</summary>
        public static bool operator !=(ValueObject<TModel> left, ValueObject<TModel> right) => !(left == right);

        /// <summary>Returns true if other value object is the same instance as this one.</summary>
        protected bool AreSame(TModel other) => ReferenceEquals(this, other);

        /// <summary>Returns true if the other value object is not null.</summary>
        protected static bool NotNull(TModel other) => !(other is null);
    }
}
