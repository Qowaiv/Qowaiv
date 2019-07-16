using FluentValidation;
using FluentValidation.Results;
using Qowaiv.Validation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.Validation.Fluent
{
    [Serializable]
    public class ValidationMessage : ValidationFailure, IValidationMessage
    {
        /// <summary>Creates a new instance of a <see cref="ValidationMessage"/>.</summary>
        protected ValidationMessage(string propertyName, string errorMessage)
            : base(propertyName, errorMessage) { }

        /// <inheritdoc />
        ValidationSeverity IValidationMessage.Severity => Severity.ToValidationSeverity();

        /// <inheritdoc />
        public string Message
        {
            get => ErrorMessage;
            set => ErrorMessage = value;
        }

        /// <summary>Gets a collection of <see cref="ValidationMessage"/>s 
        /// based on a collection of <see cref="ValidationFailure"/>s.
        /// </summary>
        public static IEnumerable<ValidationMessage> For(IEnumerable<ValidationFailure> messages)
        {
            Guard.NotNull(messages, nameof(messages));

            return messages.Select(message => For(message));
        }

        /// <summary>Creates an error message.</summary>
        public static ValidationMessage Error(string message, string propertyName) => new ValidationMessage(propertyName, message) { Severity = Severity.Error };

        /// <summary>Creates a warning message.</summary>
        public static ValidationMessage Warn(string message, string propertyName) => new ValidationMessage(propertyName, message) { Severity = Severity.Warning };

        /// <summary>Creates an info message.</summary>
        public static ValidationMessage Info(string message, string propertyName) => new ValidationMessage(propertyName, message) { Severity = Severity.Info };

        /// <summary>Gets a <see cref="ValidationMessage"/> based on a <see cref="ValidationFailure"/>.</summary>
        public static ValidationMessage For(ValidationFailure failure)
        {
            Guard.NotNull(failure, nameof(failure));

            return failure is ValidationMessage message
                ? message
                : new ValidationMessage(failure.PropertyName, failure.ErrorMessage)
                {
                    AttemptedValue = failure.AttemptedValue,
                    CustomState = failure.CustomState,
                    ErrorCode = failure.ErrorCode,
                    FormattedMessageArguments = failure.FormattedMessageArguments,
                    FormattedMessagePlaceholderValues = failure.FormattedMessagePlaceholderValues,
                    ResourceName = failure.ResourceName,
                };
        }
    }
}
