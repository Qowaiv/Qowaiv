namespace Qowaiv.Tooling.Resx;

/// <summary>Represents a collection of RESX resource files.</summary>
public sealed class XResourceCollection : Dictionary<CultureInfo, XResourceFile>
{
    /// <summary>Saves the RESX resources files.</summary>
    public void Save(DirectoryInfo directory, string name)
    {
        foreach (var file in this)
        {
            var filename = $"{name}.{file.Key.Name}.resx".Replace("..", ".");

            file.Value.Save(new FileInfo(Path.Combine(directory.FullName, filename)));
        }
    }

    public static XResourceCollection Load(DirectoryInfo dir)
    {
        var collection = new XResourceCollection();

        foreach (var file in dir.GetFiles("*.resx"))
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
        name = index == 0 ? string.Empty : name.Substring(index);
        var culture = new CultureInfo(name);
        return culture;
    }
}
