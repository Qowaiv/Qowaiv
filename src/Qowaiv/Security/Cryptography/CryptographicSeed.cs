#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Security.Cryptography;
using Qowaiv.Formatting;
using Qowaiv.Json;
using Qowaiv.Text;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.Security.Cryptography
{
    /// <summary>Represents a cryptographic seed.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [SingleValueObject(SingleValueStaticOptions.AllExcludingCulture ^ SingleValueStaticOptions.HasUnknownValue, typeof(byte[]))]
    [OpenApiDataType(description: "Base64 encoded cryptographic seed.", type: "string", format: "cryptographic-seed", nullable: true)]
    [TypeConverter(typeof(CryptographicSeedTypeConverter))]
    public partial struct CryptographicSeed : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<CryptographicSeed>, IComparable, IComparable<CryptographicSeed>
    {
        /// <summary>Represents an empty/not set cryptographic seed.</summary>
        public static readonly CryptographicSeed Empty;

        /// <summary>Gets the length of the cryptographic seed.</summary>
        public int Length => m_Value is null ? 0 : m_Value.Length;

        /// <summary>Returns a byte array that contains the value of this instance.</summary>
        public byte[] ToByteArray()
        {
            var clone = new byte[Length];

            if (Length > 0)
            {
                Array.Copy(m_Value, clone, clone.Length);
            }
            return clone;
        }

        /// <summary>Generates a cryptographic seed from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = null;

        /// <summary>Generates a cryptographic seed from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the cryptographic seed.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)=> m_Value = Parse(jsonString).m_Value;

        /// <summary>Generates a cryptographic seed from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the cryptographic seed.
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger) => throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates a cryptographic seed from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the cryptographic seed.
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a cryptographic seed from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the cryptographic seed.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a cryptographic seed into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => Length == 0 ? null : ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current cryptographic seed for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay=> IsEmpty()? "{empty}" : Base64.ToString(m_Value);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current cryptographic seed.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            return Base64.ToString(m_Value);
        }

        /// <summary>Gets an XML string representation of the cryptographic seed.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts a cryptographic seed to a <see cref="string"/>.</summary>
        public static explicit operator string(CryptographicSeed val) => val.ToString();
        /// <summary>Casts a <see cref="string"/> to a cryptographic seed.</summary>
        public static explicit operator CryptographicSeed(string str) => Parse(str);

        /// <summary>Casts a cryptographic seed to a System.byte[].</summary>
        public static explicit operator byte[] (CryptographicSeed val) => val.ToByteArray();
        /// <summary>Casts a System.byte[] to a cryptographic seed.</summary>
        public static implicit operator CryptographicSeed(byte[] bytes) => Create(bytes);

        /// <summary>Converts the string to a cryptographic seed.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a cryptographic seed to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out CryptographicSeed result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            if (Base64.TryGetBytes(s, out byte[] bytes))
            {
                result = Create(bytes);
                return true;
            }
            return false;
        }

        /// <summary>Creates a cryptographic seed from a byte[]. </summary >
        /// <param name="val" >
        /// A byte array describing a cryptographic seed.
        /// </param >
        public static CryptographicSeed Create(byte[] val)
        {
            if (val == null || val.Length == 0)
            {
                return Empty;
            }
            var bytes = new byte[val.Length];
            Array.Copy(val, bytes, val.Length);

            return new CryptographicSeed { m_Value = bytes };
        }

        /// <summary>Creates a cryptographic seed from a GUID.</summary >
        /// <param name="id" >
        /// A GUID describing a cryptographic seed.
        /// </param >
        public static CryptographicSeed Create(Guid id) => Create(id.ToByteArray());

        /// <summary>Creates a cryptographic seed from a UUID.</summary >
        /// <param name="id" >
        /// A UUID describing a cryptographic seed.
        /// </param >
        public static CryptographicSeed Create(Uuid id) => Create(id.ToByteArray());
    }
}
