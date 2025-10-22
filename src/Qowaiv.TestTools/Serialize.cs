using System.IO;
using System.Xml.Linq;

namespace Qowaiv.TestTools;

/// <summary>Helps with serialization testing.</summary>
public static class Serialize
{
    /// <summary>Serializes an instance using an <see cref="XmlSerializer" />.</summary>
    /// <typeparam name="T">
    /// Type of the instance.
    /// </typeparam>
    /// <param name="instance">
    /// The instance to (XML) serialize.
    /// </param>
    [Pure]
    public static string? Xml<T>(T instance)
    {
        using var stream = new MemoryStream();

        var writer = new XmlTextWriter(stream, Encoding.UTF8);
        var serializer = new XmlSerializer(typeof(SerializationWrapper<T>));
        serializer.Serialize(writer, new SerializationWrapper<T> { Value = instance });
        stream.Position = 0;

        var reader = new StreamReader(stream);
        var xml = XDocument.Load(reader);
        return xml.Element("Wrapper")!.Element("Value")!.Value;
    }
}
