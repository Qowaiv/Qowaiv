using System.Collections.Generic;

namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>Represents a validation info message.</summary>
    public class ValidationInfoMessage : ValidationMessage
    {
        /// <summary>Creates a new instance of a <see cref="ValidationMessage"/>.</summary>
        /// <param name="message">
        /// The validation message.
        /// </param>
        /// <param name="memberNames">
        /// The involved members.
        /// </param>
        internal ValidationInfoMessage(string message, IEnumerable<string> memberNames)
            : base(message, memberNames) { }

        /// <summary>Gets the Severity (Info) of the message.</summary>
        public sealed override ValidationSeverity Severity => ValidationSeverity.Info;
    }
}
