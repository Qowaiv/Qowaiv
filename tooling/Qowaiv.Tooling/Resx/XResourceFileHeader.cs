using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Tooling.Resx
{
    /// <summary>Represents a header of a resource file.</summary>
    [Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public sealed class XResourceFileHeader : IXmlSerializable
	{
		/// <summary>Gets the Resourse mime type header (text/microsoft-resx).</summary>
		public static readonly XResourceFileHeader ResMimeType = new("resmimetype", "text/microsoft-resx");

		/// <summary>Gets the Resourse version header (2.0).</summary>
		public static readonly XResourceFileHeader Version = new("version", "2.0");

        /// <summary>Gets the Resourse reader header (System.Resources.ResXResourceReader).</summary>
        public static readonly XResourceFileHeader Reader = new("reader", "System.Resources.ResXResourceReader");

        /// <summary>Gets the Resourse writer header (System.Resources.ResXResourceWriter).</summary>
        public static readonly XResourceFileHeader Writer = new("writer", "System.Resources.ResXResourceWriter");


        private XResourceFileHeader() { }

        /// <summary>Initializes a new instance of a resource file header.</summary>
        public XResourceFileHeader(string name, string val)
		{
            Name = name;
			Value = val;
		}

		#region IXmlSerializable

		/// <summary>Gets the xml schema to (de) xml serialize a resource file header.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		public XmlSchema GetSchema() { return null; }

		/// <summary>Reads the resource file header from an xml writer.</summary>
		/// <param name="reader">An xml reader.</param>
		public void ReadXml(XmlReader reader)
		{
			var element = XElement.Parse(reader.ReadOuterXml());
			Name = element.Attribute("name")?.Value ?? string.Empty;
			Value = element.Value;
		}

		/// <summary>Writes the resource file header to an xml writer.</summary>
		/// <param name="writer">An xml writer.</param>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", Name);
			writer.WriteString(Value);
		}

		#endregion

		/// <summary>Gets and set the name.</summary>
		[XmlAttribute("name")]
		public string Name { get; private set; }

		/// <summary>Gets and set the value.</summary>
		public string Value { get; private set; }

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
