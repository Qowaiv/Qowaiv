#pragma warning disable S5773 // Types allowed to be deserialized should be restricted
// Test code, os no risk

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Qowaiv.TestTools
{
    /// <summary>Helps with testing serialization code.</summary>
    public static class SerializeDeserialize
    {
        /// <summary>Serializes and deserializes an instance using <see cref="BinaryFormatter"/>.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="instance">
        /// The instance to serialize and deserialize.
        /// </param>
        [Pure]
        public static T Binary<T>(T instance)
        {
            using var buffer = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(buffer, instance);
            buffer.Position = 0;
            return (T)formatter.Deserialize(buffer);
        }

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
            return (T)serializer.ReadObject(stream);
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
            return (T)serializer.Deserialize(stream);
        }
    }
}
