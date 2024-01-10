using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace Qowaiv.Tooling.Resx;

/// <summary>Represents a resource file.</summary>
[Serializable, XmlType("root")]
public sealed class XResourceFile
{
    private XResourceFile() { }

    /// <summary>Initializes a new instance of a resource file.</summary>
    public XResourceFile(params XResourceFileData[] data)
    {
        Headers =
        [
            XResourceFileHeader.ResMimeType,
            XResourceFileHeader.Version,
            XResourceFileHeader.Reader,
            XResourceFileHeader.Writer,
        ];
        Data = data.ToList();
    }

    public void Add(string name, string val)
    {
        Data.Add(new XResourceFileData(name, val));
    }
    public void Add(string name, string val, string comment)
    {
        Data.Add(new XResourceFileData(name, val, comment));
    }

    /// <summary>Initializes a new instance of a resource file.</summary>
    public XResourceFile(IEnumerable<XResourceFileData> data) : this(data.ToArray()) { }

    /// <summary>Gets the headers.</summary>
    [XmlElement(Type = typeof(XResourceFileHeader), ElementName = "resheader")]
    public List<XResourceFileHeader> Headers { get; set; }

    /// <summary>Gets the data.</summary>
    [XmlElement(Type = typeof(XResourceFileData), ElementName = "data")]
    public List<XResourceFileData> Data { get; set; } = [];

    /// <summary>Gets the first (or default) item with the specified key.</summary>
    /// <param name="key">
    /// The key to search for.
    /// </param>
    public XResourceFileData this[string key]
    {
        get
        {
            return Data.FirstOrDefault(data => data.Name == key);
        }
    }

    /// <summary>Sorts the data.</summary>
    public void Sort()
    {
        Data.Sort();
    }

    /// <summary>Saves the resource file to a file.</summary>
    /// <param name="file">
    /// The file to safe to.
    /// </param>
    public void Save(FileInfo file)
    {
        using var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write);
        Save(stream);
    }

    /// <summary>Saves the resource file to a file.</summary>
    /// <param name="file">
    /// The file to safe to.
    /// </param>
    /// <param name="culture">
    /// The specif culture of the <see cref="XResourceFile"/>.
    /// </param>
    public void Save(FileInfo file, CultureInfo culture)
    {
        var languague = (culture ?? CultureInfo.InvariantCulture).Name;
        var resource = file;

        if (!string.IsNullOrEmpty(languague))
        {
            var dir = file.Directory.FullName;
            var name = $"{Path.GetFileNameWithoutExtension(file.Name)}.{languague}{file.Extension}";
            resource = new FileInfo(Path.Combine(dir, name));
        }
        Save(resource);
    }

    /// <summary>Saves the resource file to a stream.</summary>
    /// <param name="stream">
    /// The stream to safe to.
    /// </param>
    public void Save(Stream stream)
    {
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, this);
    }

    /// <summary>Loads a resource file from stream.</summary>
    /// <param name="stream">
    /// The stream to load from.
    /// </param>
    public static XResourceFile Load(Stream stream)
    {
        return (XResourceFile)serializer.Deserialize(stream)!;
    }

    /// <summary>Loads a resource file from stream.</summary>
    /// <param name="file">
    /// The file to load from.
    /// </param>
    public static XResourceFile Load(FileInfo file)
    {
        using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);

        return Load(stream);
    }

    /// <summary>Loads a resource file from stream.</summary>
    /// <param name="file">
    /// The file to load from.
    /// </param>
    public static XResourceFile Load(string file)
    {
        return Load(new FileInfo(file));
    }

    /// <summary>The serializer to load and save the resource file.</summary>
    private static readonly XmlSerializer serializer = new(typeof(XResourceFile), string.Empty);
}
