using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Tooling.Resx;

/// <summary>Represents a header of a resource file.</summary>
[Serializable]
[DebuggerDisplay("Header, Name: {Name}, Value: '{Value}'")]
	public sealed class XResourceFileHeader : IXmlSerializable
	{
		/// <summary>Gets the Resourse mime type header (text/microsoft-resx).</summary>
		public static readonly XResourceFileHeader ResMimeType = new("resmimetype", "text/microsoft-resx");

    /// <summary>Gets the Resourse reader header (System.Resources.ResXResourceReader).</summary>
    public static readonly XResourceFileHeader Reader = new("reader", "System.Resources.ResXResourceReader");

    /// <summary>Gets the Resourse writer header (System.Resources.ResXResourceWriter).</summary>
    public static readonly XResourceFileHeader Writer = new("writer", "System.Resources.ResXResourceWriter");

    /// <summary>Initializes a new instance of the <see cref="XResourceFileHeader"/> class.</summary>
    private XResourceFileHeader()
    {
        Name = string.Empty;
        Value = string.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="XResourceFileHeader"/> class.</summary>
    public XResourceFileHeader(string name, string val)
		{
        Name = name;
			Value = val;
		}

    /// <summary>Gets the xml schema to (de) xml serialize a resource file header.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    XmlSchema? IXmlSerializable.GetSchema() => null;

		/// <summary>Reads the resource file header from an xml writer.</summary>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var element = XElement.Parse(reader.ReadOuterXml());
			Name = element.Attribute("name")?.Value ?? string.Empty;
			Value = element.Element("value")?.Value ?? string.Empty;
		}

		/// <summary>Writes the resource file header to an xml writer.</summary>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", Name);
        writer.WriteElementString("value", Value);
		}

		/// <summary>Gets and set the name.</summary>
		[XmlAttribute("name")]
		public string Name { get; private set; }

		/// <summary>Gets and set the value.</summary>
		public string Value { get; private set; }
	}
