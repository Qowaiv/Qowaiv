using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>Represents a validation message.</summary>
    /// <remarks>
    /// To support messages with different severities.
    /// </remarks>
    public abstract class ValidationMessage : ValidationResult
    {
        /// <summary>Creates a new instance of a <see cref="ValidationMessage"/>.</summary>
        /// <param name="message">
        /// The validation message.
        /// </param>
        protected ValidationMessage(string message)
            : base(message) { }

        /// <summary>Creates a new instance of a <see cref="ValidationMessage"/>.</summary>
        /// <param name="message">
        /// The validation message.
        /// </param>
        /// <param name="memberNames">
        /// The involved members.
        /// </param>
        protected ValidationMessage(string message, IEnumerable<string> memberNames)
            : base(message, memberNames) { }

        /// <summary>Gets the Severity of the message.</summary>
        public abstract ValidationSeverity Severity { get; }

        /// <summary>Creates a <see cref="ValidationMessage"/> based on its outcome.</summary>
        /// <param name="isValid">
        /// Outcome of the test expression.
        /// </param>
        /// <param name="message">
        /// The message of the error message.
        /// </param>
        /// <param name="memberNames">
        /// The involved members.
        /// </param>
        /// <returns>
        /// An <see cref="None"/> if the test was successful,
        /// otherwise a <see cref="ValidationErrorMessage"/>.
        /// </returns>
        public static ValidationResult Validate(bool? isValid, string message, params string[] memberNames)
        {
            return isValid != false ? None : Error(message, memberNames);
        }

        /// <summary>Creates a None message.</summary>
        public static ValidationResult None => Success;

        /// <summary>Creates an error message.</summary>
        public static ValidationErrorMessage Error(string message, params string[] memberNames) => new ValidationErrorMessage(message, memberNames);

        /// <summary>Creates a warning message.</summary>
        public static ValidationWarningMessage Warning(string message, params string[] memberNames) => new ValidationWarningMessage(message, memberNames);

        /// <summary>Creates an info message.</summary>
        public static ValidationInfoMessage Info(string message, params string[] memberNames) => new ValidationInfoMessage(message, memberNames);

        /// <summary>Throws the <see cref="ValidationException"/> messages contain any error messages.</summary>
        /// <exception cref="ValidationException">
        /// If a single error occurs.
        /// </exception>
        /// <exception cref="AggregateException">
        /// If multiple errors occur.
        /// </exception>
        public static void ThrowIfAnyErrors(IEnumerable<ValidationResult> messages)
        {
            Guard.NotNull(messages, nameof(messages));

            var errors = messages.GetErrors().ToArray();
            if(errors.Length == 1)
            {
                var error = errors[0];
                throw new ValidationException(error, null, null);
            }
            else if(errors.Any())
            {
                throw new AggregateException(errors.Select(error => new ValidationException(error, null, null)).ToArray());
            }
        }
    }
}
