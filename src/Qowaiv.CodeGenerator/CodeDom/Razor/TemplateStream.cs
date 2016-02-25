using System;
using System.IO;

namespace Qowaiv.CodeGenerator.CodeDom.Razor
{
    /// <summary>Represents a template stream.</summary>
    public class TemplateStream
    {
        /// <summary>Constructor.</summary>
        /// <remarks>
        /// No direct access.
        /// </remarks>
        protected TemplateStream() { }

        /// <summary>Initializes a new instance of a template stream.</summary>
        /// <param name="stream">
        /// The stream of the template.
        /// </param>
        /// <param name="classname">
        /// The class name of the template.
        /// </param>
        public TemplateStream(Stream stream, string classname)
        {
            this.Stream = stream;
            this.ClassName = classname;
        }

        /// <summary>Gets the stream.</summary>
        public Stream Stream { get; protected set; }

        /// <summary>Gets the class name.</summary>
        public string ClassName { get; protected set; }
    }
}
