using Qowaiv.ComponentModel.Messages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel
{
    /// <summary>Represents a result of a validation, executed command, etcetera.</summary>
    public class Result
    {
        /// <summary>Creates a new instance of a <see cref="Result"/>.</summary>
        /// <param name="messages">
        /// The messages related to the result.
        /// </param>
        protected Result(IEnumerable<ValidationResult> messages)
        {
            Guard.NotNull(messages, nameof(messages));
            Messages = new ReadOnlyCollection<ValidationResult>(messages.GetWithSeverity().ToList());
        }

        /// <summary>Gets the messages related to the result.</summary>
        public IReadOnlyList<ValidationResult> Messages { get; }

        /// <summary>Return true if there are no error messages, otherwise false.</summary>
        public bool IsValid => !Errors.Any();

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Error"/>.</summary>
        public IEnumerable<ValidationResult> Errors => Messages.GetErrors();

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Warning"/>.</summary>
        public IEnumerable<ValidationResult> Warnings => Messages.GetWarnings();

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Info"/>.</summary>
        public IEnumerable<ValidationResult> Infos => Messages.GetInfos();

        /// <summary>Creates an OK <see cref="Result"/>.</summary>
        public static Result OK => new Result(Enumerable.Empty<ValidationResult>());

        /// <summary>Creates a <see cref="Result{T}"/> for the data.</summary>
        public static Result<T> For<T>(T data, IEnumerable<ValidationResult> messages) => new Result<T>(data, messages);

        /// <summary>Creates a <see cref="Result{T}"/> for the data.</summary>
        public static Result<T> For<T>(T data, params ValidationResult[] messages) => new Result<T>(data, messages);

        /// <summary>Creates a result with messages.</summary>
        public static Result WithMessages(params ValidationResult[] messages) => new Result(messages);

        /// <summary>Creates a result with messages.</summary>
        public static Result<T> WithMessages<T>(params ValidationResult[] messages) => new Result<T>(default(T), messages);
    }
}
