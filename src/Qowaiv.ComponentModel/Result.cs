using Qowaiv.ComponentModel.Messages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel
{
    /// <summary>Represents a result of a validation, executed command, etcetera.</summary>
    public class Result
    {
        /// <summary>Creates a new instance of a <see cref="Result"/>.</summary>
        public Result()
        {
            Messages = new List<ValidationResult>();
        }
        /// <summary>Creates a new instance of a <see cref="Result"/>.</summary>
        /// <param name="messages">
        /// The messages related to the result.
        /// </param>
        public Result(IEnumerable<ValidationResult> messages) : this()
        {
            Guard.NotNull(messages, nameof(messages));
            AddRange(messages);
        }

        /// <summary>Gets the messages related to the result.</summary>
        public IList<ValidationResult> Messages { get; }

        /// <summary>Adds a <see cref="ValidationResult"/> to the <see cref="Result"/>.</summary>
        public bool Add(ValidationResult message)
        {
            if (message.GetSeverity() == ValidationSeverity.None)
            {
                return false;
            }
            Messages.Add(message);
            return true;
        }

        /// <summary>Adds a range of <see cref="ValidationResult"/>s to the <see cref="Result"/>.</summary>
        public void AddRange(IEnumerable<ValidationResult> messages)
        {
            ((List<ValidationResult>)Messages).AddRange(messages.GetWithSeverity());
        }

        /// <summary>Return true if there are no error messages, otherwise false.</summary>
        public bool IsValid => !Errors.Any();

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Error"/>.</summary>
        public IEnumerable<ValidationResult> Errors => Messages.GetErrors();

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Warning"/>.</summary>
        public IEnumerable<ValidationResult> Warnings => Messages.GetWarnings();

        /// <summary>Gets all messages with <see cref="ValidationSeverity.Info"/>.</summary>
        public IEnumerable<ValidationResult> Infos => Messages.GetInfos();


        /// <summary>Creates a <see cref="Result{T}"/> for the data.</summary>
        public static Result<T> For<T>(T data) => new Result<T>(data);

        /// <summary>Creates a result with messages.</summary>
        public static Result WithMessages(params ValidationResult[] messages) => new Result(messages);

        /// <summary>Creates a result with messages.</summary>
        public static Result<T> WithMessages<T>(params ValidationResult[] messages) => new Result<T>(default(T), messages);
    }
}
