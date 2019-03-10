using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>Represents a validation warning message.</summary>
    [Serializable]
    public class ValidationWarningMessage : ValidationMessage
    {
        /// <summary>Creates a new instance of a <see cref="ValidationWarningMessage"/>.</summary>
        /// <param name="message">
        /// The validation message.
        /// </param>
        /// <param name="memberNames">
        /// The involved members.
        /// </param>
        internal ValidationWarningMessage(string message, IEnumerable<string> memberNames)
            : base(message, memberNames) { }

        /// <summary>Creates a new instance of <see cref="ValidationWarningMessage"/>.</summary>
        protected ValidationWarningMessage(SerializationInfo info, StreamingContext context) :
            base(info, context) { }

        /// <summary>Gets the Severity (Warning) of the message.</summary>
        public sealed override ValidationSeverity Severity => ValidationSeverity.Warning;
    }
}
