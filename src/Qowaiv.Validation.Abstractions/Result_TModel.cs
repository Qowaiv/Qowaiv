using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.Validation.Abstractions
{
    /// <summary>Represents a result of a validation, executed command, etcetera.</summary>
    public sealed class Result<TModel> : Result
    {
        /// <summary>Creates a new instance of a <see cref="Result{T}"/>.</summary>
        /// <param name="data">
        /// The data related to the result.
        /// </param>
        /// <param name="messages">
        /// The messages related to the result.
        /// </param>
        internal Result(TModel data, IEnumerable<IValidationMessage> messages) : base(messages)
        {
            _value = IsValid ? data : default;
        }

        /// <summary>Gets the value related to result.</summary>
        public TModel Value => IsValid
            ? _value
            : throw InvalidModelException.For<TModel>(Errors);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly TModel _value;

        /// <summary>Implicitly casts a model to the <see cref="Result"/>.</summary>
        public static implicit operator Result<TModel>(TModel model) => For(model);

        /// <summary>Throws an <see cref="InvalidModelException"/> if the result is not valid.</summary>
        public void ThrowIfInvalid()
        {
            if (!IsValid)
            {
                throw InvalidModelException.For<TModel>(Errors);
            }
        }

        /// <summary>Explicitly casts the <see cref="Result"/> to the type of the related model.</summary>
        public static explicit operator TModel(Result<TModel> result) => result == null ? default : result.Value;
    }
}
