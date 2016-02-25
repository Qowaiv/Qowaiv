using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Qowaiv.CodeGenerator.CodeDom.Razor
{
    /// <summary>Represents a template not found exception.</summary>
    [Serializable]
    public class TemplateNotFoundException : Exception
    {
        /// <summary>Initializes a new instance of a template not found exception.</summary>
        public TemplateNotFoundException() { }

        /// <summary>Initializes a new instance of a template not found exception.</summary>
        /// <param name="type">
        /// The type to search a template for.
        /// </param>
        /// <param name="postfix">
        /// The used postfix.
        /// </param>
        public TemplateNotFoundException(Type type, string postfix = null) :
            base(string.Format(CultureInfo.CurrentCulture, QowaivCodeGeneratorMessages.TemplateNotFoundException_Type, type, postfix)) { }

        /// <summary>Initializes a new instance of a template not found exception.</summary>
        /// <param name="name">
        /// The name to search a template for.
        /// </param>
        public TemplateNotFoundException(string name) :
            base(string.Format(CultureInfo.CurrentCulture, QowaivCodeGeneratorMessages.TemplateNotFoundException_Name, name)) { }

        /// <summary>Initializes a new instance of a template not found exception ith serialized data.</summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual
        /// information about the source or destination.
        /// </param>
        protected TemplateNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
