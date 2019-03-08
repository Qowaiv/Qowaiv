using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv.ComponentModel
{
    /// <summary>Represents a result of a validation, executed command, etcetera.</summary>
    public class Result<T> : Result
    {
        /// <summary>Creates a new instance of a <see cref="Result{T}"/>.</summary>
        public Result(IEnumerable<ValidationResult> messages) : this(default(T), messages) { }

        /// <summary>Creates a new instance of a <see cref="Result{T}"/>.</summary>
        /// <param name="data">
        /// The data related to the result.
        /// </param>
        public Result(T data) : this(data, Enumerable.Empty<ValidationResult>()) { }

        /// <summary>Creates a new instance of a <see cref="Result{T}"/>.</summary>
        /// <param name="data">
        /// The data related to the result.
        /// </param>
        /// <param name="messages">
        /// The messages related to the result.
        /// </param>
        public Result(T data, IEnumerable<ValidationResult> messages) : base(messages)
        {
            _data = data;
        }

        /// <summary>Gets the data related to result.</summary>
        public T Data => IsValid 
            ? _data 
            : throw new InvalidOperationException(QowaivComponentModelMessages.InvalidOperationException_InvalidModel);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly T _data;

        /// <summary>Implicitly casts a model to the <see cref="Result"/>.</summary>
        public static implicit operator Result<T>(T model) => new Result<T>(model);

        /// <summary>Explicitly casts the <see cref="Result"/> to the type of the related model.</summary>
        public static explicit operator T(Result<T> result) => result == null ? default(T) : result.Data;
    }
}
