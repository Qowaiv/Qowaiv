using System.Globalization;
using System.Xml.Serialization;

namespace Qowaiv.Tooling.Resx;

/// <summary>Represents data of a resource file.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable, XmlType("data")]
public sealed class XResourceFileData : IComparable, IComparable<XResourceFileData>
{
    /// <summary>Initializes a new instance of resource file data.</summary>
    public XResourceFileData(string name, string val) : this(name, val, null) { }

    /// <summary>Initializes a new instance of resource file data.</summary>
    public XResourceFileData(string name, string val, string? comment)
    {
        Name = name;
        Value = val;
        // not add empty comments.
        Comment = string.IsNullOrWhiteSpace(comment) ? null : comment;
    }

    /// <summary>Gets and set the name.</summary>
    [XmlAttribute("name")]
    public string Name { get; set; }

    /// <summary>Gets and set the value.</summary>
    [XmlElement("value")]
    public string Value { get; set; }

    /// <summary>Gets and set a comment.</summary>
    [XmlElement("comment")]
    public string? Comment { get; set; }

    #region IComparable

    public int CompareTo(object obj)
    {
        return CompareTo(obj as XResourceFileData);
    }

    public int CompareTo(XResourceFileData other)
    {
        return string.CompareOrdinal(Name, other == null ? null : other.Name);
    }

    #endregion

    /// <summary>Represents the resource file data as debug string.</summary>
    private string DebuggerDisplay
    {
        get
        {
            return string.Format(CultureInfo.InvariantCulture,
                "Data, Name: {0}, Value: '{1}'{2}",
                Name,
                Value,
                string.IsNullOrEmpty(Comment) ? "" : ", Comment: '" + Comment + "'");
        }
    }
}
