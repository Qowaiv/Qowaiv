using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>Represents a validation info message.</summary>
    [Serializable]
    public class ValidationInfoMessage : ValidationMessage
    {
        /// <summary>Creates a new instance of a <see cref="ValidationInfoMessage"/>.</summary>
        /// <param name="message">
        /// The validation message.
        /// </param>
        /// <param name="memberNames">
        /// The involved members.
        /// </param>
        internal ValidationInfoMessage(string message, IEnumerable<string> memberNames)
            : base(message, memberNames) { }

        /// <summary>Creates a new instance of <see cref="ValidationInfoMessage"/>.</summary>
        protected ValidationInfoMessage(SerializationInfo info, StreamingContext context) :
            base(info, context){ }

        /// <summary>Gets the Severity (Info) of the message.</summary>
        public sealed override ValidationSeverity Severity => ValidationSeverity.Info;
    }
}
