using System;
using System.Xml.Serialization;

namespace Qowaiv.TestTools
{
    /// <summary>Wrapper used by <see cref="SerializationTest.XmlSerialize{T}(T)"/>
    /// and <see cref="SerializationTest.XmlSerializeDeserialize{T}(T)"/>.
    /// </summary>
    [Serializable, XmlRoot("Wrapper")]
    public class SerializationWrapper<T>
    {
        public T Value { get; set; }
    }
}
