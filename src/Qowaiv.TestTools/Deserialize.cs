using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Qowaiv.TestTools
{
    /// <summary>Helps with serialization testing.</summary>
    public static class Deserialize
    {
        /// <summary>Serializes an instance using an XmlSerializer.</summary>
        /// <typeparam name="T">
        /// Type of the instance.
        /// </typeparam>
        /// <param name="xml">
        /// The xml string to (XML) deserialize.
        /// </param>
        [Pure]
        public static T Xml<T>(string xml)
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
    }
}
