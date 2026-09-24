using static System.Net.WebRequestMethods;

namespace Qowaiv.TestTools.Resx;

/// <summary>Represents a collection of RESX resource files.</summary>
public sealed class XResourceCollection : IReadOnlyDictionary<CultureInfo, XResourceFile>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly Dictionary<CultureInfo, XResourceFile> lookup = [];

    /// <inheritdoc />
    public int Count =>lookup.Count;

    /// <inheritdoc />
    public IEnumerable<CultureInfo> Keys => lookup.Keys;
    
    /// <inheritdoc />
    public IEnumerable<XResourceFile> Values => lookup.Values;

    public XResourceFile Invariant => this[CultureInfo.InvariantCulture];

    public XResourceFile this[CultureInfo key]
    {
        get
        {
            if (!TryGetValue(key, out var resoures))
            {
                resoures = new();
                lookup[key] = resoures;
            }
            return resoures;
        }
        private set => lookup[key] = value;
    }

    /// <summary>Tries to get the value for a specific culture.</summary>
    public string? TryGetValue(string key, CultureInfo culture) => culture switch
    {
        _ when lookup.TryGetValue(culture, out var file) && file[key]?.Value is { Length: > 0 } v => v,
        _ when culture.Parent is { } parent => TryGetValue(key, parent),
        _ => null,
    };

    /// <summary>Saves the RESX resources files.</summary>
    public void Save(DirectoryInfo directory, string name)
    {
        foreach (var file in this.Where(f => f.Value.Data.Any()))
        {
            var filename = $"{name}.{file.Key.Name}.resx".Replace("..", ".");

            file.Value.Save(new FileInfo(Path.Combine(directory.FullName, filename)));
        }
    }

    public static XResourceCollection Load(DirectoryInfo dir, string name)
    {
        var collection = new XResourceCollection();

        foreach (var file in dir.GetFiles($"{name}*.resx"))
        {
            var resource = XResourceFile.Load(file);
            collection[GetCulture(file)] = resource;
        }

        return collection;
    }
    
    private static CultureInfo GetCulture(FileInfo file)
    {
        var name = Path.GetFileNameWithoutExtension(file.Name);
        var index = name.LastIndexOf('.') + 1;
        name = index == 0 ? string.Empty : name[index..];
        var culture = new CultureInfo(name);
        return culture;
    }

    public bool ContainsKey(CultureInfo key) => ((IReadOnlyDictionary<CultureInfo, XResourceFile>)lookup).ContainsKey(key);
    public bool TryGetValue(CultureInfo key, [MaybeNullWhen(false)] out XResourceFile value) => ((IReadOnlyDictionary<CultureInfo, XResourceFile>)lookup).TryGetValue(key, out value);
    public IEnumerator<KeyValuePair<CultureInfo, XResourceFile>> GetEnumerator() => ((IEnumerable<KeyValuePair<CultureInfo, XResourceFile>>)lookup).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)lookup).GetEnumerator();
}
