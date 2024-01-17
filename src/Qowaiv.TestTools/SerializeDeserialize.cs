#pragma warning disable S5773 // Types allowed to be deserialized should be restricted
// Test code, so no risk

using System.IO;

namespace Qowaiv.TestTools;

/// <summary>Helps with testing serialization code.</summary>
public static class SerializeDeserialize
{
#if NET8_0_OR_GREATER
#else
    /// <summary>Serializes and deserializes an instance using <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>.</summary>
    /// <typeparam name="T">
    /// Type of the instance.
    /// </typeparam>
    /// <param name="instance">
    /// The instance to serialize and deserialize.
    /// </param>
    [Pure]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public static T Binary<T>(T instance)
    {
        using var buffer = new MemoryStream();
        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        formatter.Serialize(buffer, instance ?? throw new ArgumentNullException(nameof(instance)));
        buffer.Position = 0;
        return (T)formatter.Deserialize(buffer)!;
    }
#endif

    /// <summary>Serializes and deserializes an instance using a <see cref="DataContractSerializer"/>.</summary>
    /// <typeparam name="T">
    /// Type of the instance.
    /// </typeparam>
    /// <param name="instance">
    /// The instance to (XML) serialize and (XML) deserialize.
    /// </param>
    [Pure]
    public static T DataContract<T>(T instance)
    {
        using var stream = new MemoryStream();

        var serializer = new DataContractSerializer(typeof(T));
        serializer.WriteObject(stream, instance);
        stream.Position = 0;
        return (T)serializer.ReadObject(stream)!;
    }

    /// <summary>Serializes and deserializes an instance using an <see cref="XmlSerializer"/>.</summary>
    /// <typeparam name="T">
    /// Type of the instance.
    /// </typeparam>
    /// <param name="instance">
    /// The instance to (XML) serialize and (XML) deserialize.
    /// </param>
    [Pure]
    public static T Xml<T>(T instance)
    {
        using var stream = new MemoryStream();
        var writer = new XmlTextWriter(stream, Encoding.UTF8);
        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(writer, instance);
        stream.Position = 0;
        return (T)serializer.Deserialize(stream)!;
    }
}
