using System.Collections.Generic;

namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>Represents a validation error message.</summary>
    public class ValidationErrorMessage : ValidationMessage
    {
        /// <summary>Creates a new instance of a <see cref="ValidationMessage"/>.</summary>
        /// <param name="message">
        /// The validation message.
        /// </param>
        /// <param name="memberNames">
        /// The involved members.
        /// </param>
        internal ValidationErrorMessage(string message, IEnumerable<string> memberNames)
            : base(message, memberNames) { }

        /// <summary>Gets the Severity (Error) of the message.</summary>
        public sealed override ValidationSeverity Severity => ValidationSeverity.Error;
    }
}
