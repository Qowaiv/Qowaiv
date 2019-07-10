using FluentValidation;
using FluentValidation.Results;
using Qowaiv.Validation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.Validation.Fluent
{
    [Serializable]
    public class ValidationMessage : IValidationMessage
    {
        /// <inheritdoc />
        public ValidationSeverity Severity { get; set; }

        /// <inheritdoc />
        public string MemberName { get; set; }

        /// <inheritdoc />
        public string Message { get; set; }

        /// <summary>The property value that caused the failure.</summary>
        public object AttemptedValue { get; set; }

        /// <summary>Custom state associated with the failure.</summary>
        public object CustomState { get; set; }

        /// <summary>Gets or sets the error code.</summary>
        public string ErrorCode { get; set; }

        /// <summary>Gets or sets the formatted message arguments. These are values for custom formatted
        /// message in validator resource files Same formatted message can be reused in UI
        /// and with same number of format placeholders Like "Value {0} that you entered
        /// should be {1}"
        /// </summary>
        public object[] FormattedMessageArguments { get; set; }

        /// <summary>Gets or sets the formatted message placeholder values.</summary>
        public Dictionary<string, object> FormattedMessagePlaceholderValues { get; set; }

        /// <summary>The resource name used for building the message.</summary>
        public string ResourceName { get; set; }

        /// <inheritdoc />
        public override string ToString() => Message;

        /// <summary>Gets a collection of <see cref="ValidationMessage"/>s 
        /// based on a collection of <see cref="ValidationFailure"/>s.
        /// </summary>
        public static IEnumerable<ValidationMessage> For(IEnumerable<ValidationFailure> messages)
        {
            Guard.NotNull(messages, nameof(messages));

            return messages.Select(message => For(message));
        }

        /// <summary>Gets a <see cref="ValidationMessage"/> based on a <see cref="ValidationFailure"/>.</summary>
        public static ValidationMessage For(ValidationFailure message)
        {
            Guard.NotNull(message, nameof(message));

            return new ValidationMessage
            {
                Severity = message.Severity.ToValidationSeverity(),
                MemberName = message.PropertyName,
                Message = message.ErrorMessage,

                AttemptedValue = message.AttemptedValue,
                CustomState = message.CustomState,
                ErrorCode = message.ErrorCode,
                FormattedMessageArguments = message.FormattedMessageArguments,
                FormattedMessagePlaceholderValues =message.FormattedMessagePlaceholderValues,
                ResourceName = message.ResourceName,
            };

        }
    }
}
