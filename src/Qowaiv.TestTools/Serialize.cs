using System.IO;
using System.Xml.Linq;

namespace Qowaiv.TestTools;

/// <summary>Helps with serialization testing.</summary>
public static class Serialize
{
    /// <summary>Get <see cref="SerializationInfo"/> filled by <see cref="ISerializable.GetObjectData(SerializationInfo, StreamingContext)"/>.</summary>
    /// <typeparam name="T">
    /// Type of the instance.
    /// </typeparam>
    /// <param name="instance">
    /// The instance to retrieve the object data from.
    /// </param>
#if NET8_0_OR_GREATER
    [Obsolete("Formatter-based serialization is obsolete and should not be used.", error: true)]
#endif
    [Pure]
    public static SerializationInfo GetInfo<T>(T instance) where T : ISerializable
    {
        ISerializable obj = instance;
        var info = new SerializationInfo(typeof(T), new FormatterConverter());
        obj.GetObjectData(info, default);
        return info;
    }

    /// <summary>Serializes an instance using an <see cref="XmlSerializer"/>.</summary>
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
