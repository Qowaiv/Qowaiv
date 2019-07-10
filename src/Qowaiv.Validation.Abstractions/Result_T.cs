using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.Validation.Abstractions
{
    /// <summary>Represents a result of a validation, executed command, etcetera.</summary>
    public sealed class Result<T> : Result
    {
        /// <summary>Creates a new instance of a <see cref="Result{T}"/>.</summary>
        /// <param name="data">
        /// The data related to the result.
        /// </param>
        /// <param name="messages">
        /// The messages related to the result.
        /// </param>
        internal Result(T data, IEnumerable<IValidationMessage> messages) : base(messages)
        {
            _value =  IsValid ? data : default;
        }

        /// <summary>Gets the value related to result.</summary>
        public T Value => IsValid
            ? _value
            : throw InvalidModelException.For<T>(Errors);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly T _value;

        /// <summary>Implicitly casts a model to the <see cref="Result"/>.</summary>
        public static implicit operator Result<T>(T model) => For(model);

        /// <summary>Explicitly casts the <see cref="Result"/> to the type of the related model.</summary>
        public static explicit operator T(Result<T> result) => result == null ? default : result.Value;
    }
}
