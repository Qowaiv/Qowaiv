using System.Xml;

namespace Qowaiv.TestTools.Resx;

/// <summary>Represents a resource file.</summary>
[Serializable]
[XmlType("root")]
public sealed class XResourceFile
{
    /// <summary>Initializes a new instance of the <see cref="XResourceFile"/> class.</summary>
    private XResourceFile() { }

    /// <summary>Initializes a new instance of the <see cref="XResourceFile"/> class.</summary>
    public XResourceFile(params XResourceFileData[] data)
    {
        Headers =
        [
            XResourceFileHeader.ResMimeType,
            XResourceFileHeader.Reader,
            XResourceFileHeader.Writer,
        ];
        Data = data.ToList();
    }

    /// <summary>Gets the headers.</summary>
    [XmlElement(Type = typeof(XResourceFileHeader), ElementName = "resheader")]
    public List<XResourceFileHeader> Headers { get; set; } = [];

    /// <summary>Gets the data.</summary>
    [XmlElement(Type = typeof(XResourceFileData), ElementName = "data")]
    public List<XResourceFileData> Data { get; set; } = [];

    /// <summary>Gets the first (or default) item with the specified key.</summary>
    /// <param name="key">
    /// The key to search for.
    /// </param>
    public XResourceFileData? this[string key] => Data.Find(data => data.Name == key);

    /// <summary>Adds a data entries.</summary>
    public void AddRange(IEnumerable<XResourceFileData> entries) => Data.AddRange(entries);

    /// <summary>Adds a data entry.</summary>
    public void Add(XResourceFileData entry) => Data.Add(entry);

    /// <summary>Adds a data entry.</summary>
    public void Add(string name, string val) => Add(new XResourceFileData(name, val));

    /// <inheritdoc cref="Add(string, string)" />
    public void Add(string name, string val, string comment) => Add(new XResourceFileData(name, val, comment));

    /// <summary>Saves the resource file to a file.</summary>
    /// <param name="file">
    /// The file to save to.
    /// </param>
    public void Save(FileInfo file)
    {
        using var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write);
        Save(stream);
    }

    /// <summary>Saves the resource file to a file.</summary>
    /// <param name="file">
    /// The file to save to.
    /// </param>
    /// <param name="culture">
    /// The specific culture of the <see cref="XResourceFile"/>.
    /// </param>
    public void Save(FileInfo file, CultureInfo culture)
    {
        var language = (culture ?? CultureInfo.InvariantCulture).Name;
        var resource = file;

        if (!string.IsNullOrEmpty(language))
        {
            var dir = file.Directory!.FullName;
            var name = $"{Path.GetFileNameWithoutExtension(file.Name)}.{language}{file.Extension}";
            resource = new FileInfo(Path.Combine(dir, name));
        }
        Save(resource);
    }

    /// <summary>Saves the resource file to a stream.</summary>
    /// <param name="stream">
    /// The stream to save to.
    /// </param>
    public void Save(Stream stream)
    {
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, this);
    }

    /// <summary>Loads a resource file from a stream.</summary>
    /// <param name="stream">
    /// The stream to load from.
    /// </param>
    [Pure]
    public static XResourceFile Load(Stream stream) => (XResourceFile)serializer.Deserialize(stream)!;

    /// <summary>Loads a resource file from file location.</summary>
    /// <param name="file">
    /// The file to load from.
    /// </param>
    [Pure]
    public static XResourceFile Load(FileInfo file)
    {
        using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);

        return Load(stream);
    }

    /// <summary>Loads a resource file from file location.</summary>
    /// <param name="file">
    /// The file to load from.
    /// </param>
    [Pure]
    public static XResourceFile Load(string file) => Load(new FileInfo(file));

    /// <summary>The serializer to load and save the resource file.</summary>
    private static readonly XmlSerializer serializer = new(typeof(XResourceFile), string.Empty);
}
