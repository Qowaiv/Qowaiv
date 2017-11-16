using System.Collections.Generic;

namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>Represents a validation warning message.</summary>
    public class ValidationWarningMessage : ValidationMessage
    {
        /// <summary>Creates a new instance of a <see cref="ValidationMessage"/>.</summary>
        /// <param name="message">
        /// The validation message.
        /// </param>
        /// <param name="memberNames">
        /// The involved members.
        /// </param>
        internal ValidationWarningMessage(string message, IEnumerable<string> memberNames)
            : base(message, memberNames) { }

        /// <summary>Gets the Severity (Warning) of the message.</summary>
        public sealed override ValidationSeverity Severity => ValidationSeverity.Warning;
    }
}
