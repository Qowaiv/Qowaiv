namespace Qowaiv.TestTools.Resx;

/// <summary>Represents data of a resource file.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[XmlType("data")]
public sealed class XResourceFileData : IComparable<XResourceFileData>
{
    /// <summary>Initializes a new instance of the <see cref="XResourceFileData" /> class.</summary>
    private XResourceFileData()
    {
        Name = string.Empty;
        Value = string.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="XResourceFileData" /> class.</summary>
    public XResourceFileData(string name, string val) : this(name, val, null) { }

    /// <summary>Initializes a new instance of the <see cref="XResourceFileData" /> class.</summary>
    public XResourceFileData(string name, string val, string? comment)
    {
        Name = name;
        Value = val;
        // Do not add empty comments.
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

    /// <summary>Represents the resource file data as debug string.</summary>
    private string DebuggerDisplay => Comment is { }
        ? $"Data, Name: {Name}, Value: '{Value}' Comment: '{Comment}'"
        : $"Data, Name: {Name}, Value: '{Value}'";

    public int CompareTo(XResourceFileData? other)
        => other is null 
        ? +1
        : Name.CompareTo(other.Name);
}
