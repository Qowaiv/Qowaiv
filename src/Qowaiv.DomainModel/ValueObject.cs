#pragma warning disable S4035 
// Classes implementing "IEquatable<T>" should be sealed
// It is left to actual implementations

using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) value object.</summary>
    /// <typeparam name="T">
    /// The type of the identifier.
    /// </typeparam>
    /// <remarks>
    /// This base class should not be used for Single Value Objects (SVO's).
    /// </remarks>
    public abstract class ValueObject<T> : IEquatable<T>, IValidatableObject where T : ValueObject<T>
    {
        private readonly AnnotatedModelValidator _validator;

        /// <summary>Creates a new instance of a <see cref="ValueObject{T}"/>.</summary>
        protected ValueObject() : this(null) { }

        /// <summary>Creates a new instance of a <see cref="ValueObject{T}"/>.</summary>
        /// <param name="validator">
        /// The validator to validate the value object with.
        /// </param>
        protected ValueObject(AnnotatedModelValidator validator)
        {
            _validator = validator ?? new AnnotatedModelValidator();
        }

        /// <summary>Validates if the <see cref="ValueObject{T}"/> is valid.</summary>
        /// <exception cref="ValidationException">
        /// If a single error occurs.
        /// </exception>
        /// <exception cref="AggregateException">
        /// If multiple errors occur.
        /// </exception>
        /// <remarks>
        /// This should be called directly after being initiated.
        /// </remarks>
        protected void Validate()
        {
            Result result = _validator.Validate(this);
            ValidationMessage.ThrowIfAnyErrors(result.Messages);
        }

        /// <summary>Validates if the <see cref="ValueObject{T}"/> is valid.</summary>
        /// <remarks>
        /// This should be called directly, but with <see cref="Validate()"/>
        /// directly after being initiated.
        /// </remarks>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => Enumerable.Empty<ValidationResult>();

        /// <inheritdoc />
        public abstract bool Equals(T other);

        /// <inheritdoc />
        public sealed override bool Equals(object obj) => Equals(obj as T);

        /// <inheritdoc />
        public sealed override int GetHashCode() => Hash();

        /// <summary>Gets a hash code for the value object.</summary>
        /// <remarks>
        /// The reason to have this abstract Hash() method, and seal the GetHashCode()
        /// method, it enforce a custom implementation of a hash function.
        /// </remarks>
        protected abstract int Hash();

        /// <summary>Returns true if the two value objects are equal, other false.</summary>
        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            if (left is null || right is null)
            {
                return ReferenceEquals(left, right);
            }
            return left.Equals(right);
        }

        /// <summary>Returns true if the two value objects are not equal, other false.</summary>
        public static bool operator !=(ValueObject<T> left, ValueObject<T> right) => !(left == right);

        /// <summary>Returns true if other value object is the same instance as this one.</summary>
        protected bool AreSame(T other) => ReferenceEquals(this, other);

        /// <summary>Returns true if the other value object is not null.</summary>
        protected static bool NotNull(T other) => !(other is null);
    }
}
