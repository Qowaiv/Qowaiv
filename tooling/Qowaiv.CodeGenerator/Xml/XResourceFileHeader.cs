using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.CodeGenerator.Xml
{
    /// <summary>Represents a header of a resource file.</summary>
    [Serializable, DebuggerDisplay("{DebuggerDisplay}")]
    public class XResourceFileHeader : IXmlSerializable
    {
        /// <summary>Gets the Resourse mime type header (text/microsoft-resx).</summary>
        public static readonly XResourceFileHeader ResMimeType = new XResourceFileHeader("resmimetype", "text/microsoft-resx");

        /// <summary>Gets the Resourse version header (2.0).</summary>
        public static readonly XResourceFileHeader Version = new XResourceFileHeader("version", "2.0");

        /// <summary>Gets the Resourse reader header (System.Resources.ResXResourceReader).</summary>
        public static readonly XResourceFileHeader Reader = new XResourceFileHeader("reader", "System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");

        /// <summary>Gets the Resourse writer header (System.Resources.ResXResourceWriter).</summary>
        public static readonly XResourceFileHeader Writer = new XResourceFileHeader("writer", "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");

        /// <summary>Initializes a new instance of a resource file header.</summary>
        protected XResourceFileHeader() { }

        /// <summary>Initializes a new instance of a resource file header.</summary>
        public XResourceFileHeader(string name, string val)
        {
            this.Name = Guard.NotNullOrEmpty(name, "name");
            this.Value = val;
        }

        #region IXmlSerializable

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a resource file header.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        public XmlSchema GetSchema() { return null; }

        /// <summary>Reads the resource file header from an <see href="XmlReader"/>.</summary>
        /// <param name="reader">An XML reader.</param>
        public void ReadXml(XmlReader reader)
        {
            var element = XElement.Parse(reader.ReadOuterXml());
            this.Name = element.Attribute("name").Value;
            this.Value = element.Value;
        }

        /// <summary>Writes the resource file header to an <see href="XmlWriter"/>.</summary>
        /// <param name="writer">An XML writer.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", this.Name);
            writer.WriteString(this.Value);
        }

        #endregion

        /// <summary>Gets and set the name.</summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>Gets and set the value.</summary>
        public string Value { get; set; }

        /// <summary>Represents the resource file data as debug string.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "Header, Name: {0}, Value: '{1}'",
                    Name,
                    Value);
            }
        }
    }
}
