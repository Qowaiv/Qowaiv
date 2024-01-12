using System.Xml;

namespace Qowaiv.TestTools.Resx;

/// <summary>Represents a header of a resource file.</summary>
[Serializable]
[DebuggerDisplay("Header, Name: {Name}, Value: '{Value}'")]
public sealed class XResourceFileHeader
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

    /// <summary>Gets and sets the name.</summary>
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("value")]
    /// <summary>Gets and sets the value.</summary>
    public string Value { get; set; }
}
