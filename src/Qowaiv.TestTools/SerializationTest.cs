﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Qowaiv.TestTools
{
    /// <summary>Helps with serialization testing.</summary>
    public static class SerializationTest
    {
        /// <summary>Serializes and deserializes an instance using <see cref="BinaryFormatter"/>.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="instance">
        /// The instance to serialize and deserialize.
        /// </param>
        public static T BinaryFormatterSerializeDeserialize<T>(T instance)
        {
            using var buffer = new MemoryStream();

            var formatter = new BinaryFormatter();
            formatter.Serialize(buffer, instance);
            buffer.Position = 0;
            return (T)formatter.Deserialize(buffer);
        }

        /// <summary>Get <see cref="SerializationInfo"/> filled by <see cref="ISerializable.GetObjectData(SerializationInfo, StreamingContext)"/>.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="instance">
        /// The instance to retrieve the object data from.
        /// </param>
        public static SerializationInfo GetSerializationInfo<T>(T instance) where T: ISerializable
        {
            ISerializable obj = instance;
            var info = new SerializationInfo(typeof(YesNo), new FormatterConverter());
            obj.GetObjectData(info, default);
            return info;
        }

        /// <summary>Serializes and deserializes an instance.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="instance">
        /// The instance to serialize and deserialize.
        /// </param>
        [Obsolete("use BinaryFormatterSerializeDeserialize<T>(instance) instead.")]
        public static T SerializeDeserialize<T>(T instance)
        {
            using var buffer = new MemoryStream();

            var formatter = new BinaryFormatter();
            formatter.Serialize(buffer, instance);

            // reset position.
            buffer.Position = 0;

            var result = (T)formatter.Deserialize(buffer);
            return result;
        }

        /// <summary>Serializes an instance using an XmlSerializer.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="instance">
        /// The instance to (XML) serialize.
        /// </param>
        public static string XmlSerialize<T>(T instance)
        {
            using var stream = new MemoryStream();

            var writer = new XmlTextWriter(stream, Encoding.UTF8);
            var serializer = new XmlSerializer(typeof(SerializationWrapper<T>));
            serializer.Serialize(writer, new SerializationWrapper<T> { Value = instance });
            stream.Position = 0;

            var reader = new StreamReader(stream);

            var xml = XDocument.Load(reader);
            return xml.Element("Wrapper")?.Element("Value")?.Value;
        }

        /// <summary>Serializes an instance using an XmlSerializer.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="xml">
        /// The xml string to (XML) deserialize.
        /// </param>
        public static T XmlDeserialize<T>(string xml)
        {
            var value = new XElement("Value", xml);
            var doc = new XDocument(new XElement("Wrapper", value));

            using var stream = new MemoryStream();

            doc.Save(stream);
            stream.Position = 0;
            var serializer = new XmlSerializer(typeof(SerializationWrapper<T>));
            try
            {
                var wrapper = (SerializationWrapper<T>)serializer.Deserialize(stream);
                return wrapper.Value;
            }
            catch (Exception x)
            {
                throw new SerializationException($"'{value.Value}' failed on: {x.Message}", x);
            }
        }

        /// <summary>Serializes and deserializes an instance using an XmlSerializer.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="instance">
        /// The instance to (XML) serialize and (XML) deserialize.
        /// </param>
        public static T XmlSerializeDeserialize<T>(T instance)
        {
            using var stream = new MemoryStream();

            var writer = new XmlTextWriter(stream, Encoding.UTF8);
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, instance);
            stream.Position = 0;
            return (T)serializer.Deserialize(stream);
        }

        /// <summary>Serializes and deserializes an instance using an DataContractSerializer.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="instance">
        /// The instance to (XML) serialize and (XML) deserialize.
        /// </param>
        public static T DataContractSerializeDeserialize<T>(T instance)
        {
            using var stream = new MemoryStream();

            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return (T)serializer.ReadObject(stream);
        }

        /// <summary>Invokes the Deserialize Constructor ( T(SerializationInfo, StreamingContext) ).</summary>
        /// <typeparam name="T">
        /// Type of the constructor to invoke.
        /// </typeparam>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The streaming context.
        /// </param>
        /// <remarks>
        /// This can throw target invocation exceptions, if so, the inner exception is thrown.
        /// </remarks>
        public static T DeserializeUsingConstructor<T>(SerializationInfo info, StreamingContext context)
        {
            var ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new [] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
            Assert.IsNotNull(ctor, $"No {typeof(T).Name}(SerializationInfo, StreamingContext) constructor found.");

            try
            {
                return (T)ctor.Invoke(new object[] { info, context });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        /// <summary>Gets new serialization info using System.Runtime.Serialization.FormatterConverter.</summary>
        /// <typeparam name="T">
        /// The type to (de)serialize.
        /// </typeparam>
        public static SerializationInfo GetSerializationInfo<T>()
        {
            var info = new SerializationInfo(typeof(T), new FormatterConverter());
            return info;
        }
    }
}
