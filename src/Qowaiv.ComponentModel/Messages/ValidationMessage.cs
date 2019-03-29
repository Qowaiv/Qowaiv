using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>Represents a validation message.</summary>
    /// <remarks>
    /// To support serialization and messages with different severities.
    /// </remarks>
    [Serializable]
    public abstract class ValidationMessage : ValidationResult, ISerializable
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

        /// <summary>Creates a new instance of <see cref="ValidationMessage"/>.</summary>
        protected ValidationMessage(SerializationInfo info, StreamingContext context) :
            base(GetMessage(info), GetMemberNames(info)) { }

        /// <summary>Helper methods to deserialize the <see cref="ValidationMessage"/>.</summary>
        private static string GetMessage(SerializationInfo info) => info.GetString(nameof(ErrorMessage));

        /// <summary>Helper methods to deserialize the <see cref="ValidationMessage"/>.</summary>
        private static string[] GetMemberNames(SerializationInfo info) => info.GetValue(nameof(MemberNames), typeof(string[])) as string[];

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));

            info.AddValue(nameof(ErrorMessage), ErrorMessage);
            info.AddValue(nameof(MemberNames), MemberNames.ToArray());
        }

        /// <summary>Gets the Severity of the message.</summary>
        public abstract ValidationSeverity Severity { get; }

        /// <summary>Creates a None message.</summary>
        public static ValidationResult None => Success;

        /// <summary>Creates an error message.</summary>
        public static ValidationErrorMessage Error(string message, params string[] memberNames) => new ValidationErrorMessage(message, memberNames);

        /// <summary>Creates a warning message.</summary>
        public static ValidationWarningMessage Warning(string message, params string[] memberNames) => new ValidationWarningMessage(message, memberNames);

        /// <summary>Creates an info message.</summary>
        public static ValidationInfoMessage Info(string message, params string[] memberNames) => new ValidationInfoMessage(message, memberNames);

        /// <summary>Creates a validation message.</summary>
        public static ValidationResult For(ValidationSeverity serverity, string message, string[] memberNames)
        {
            switch (serverity)
            {
                case ValidationSeverity.None: return None;
                case ValidationSeverity.Info: return Info(message, memberNames);
                case ValidationSeverity.Warning: return Warning(message, memberNames);
                case ValidationSeverity.Error:
                default: return Error(message, memberNames);
            }
        }

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
            if (errors.Length == 1)
            {
                var error = errors[0];
                throw new ValidationException(error, null, null);
            }
            else if (errors.Any())
            {
                throw new AggregateException(errors.Select(error => new ValidationException(error, null, null)).ToArray());
            }
        }
    }
}
