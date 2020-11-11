#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Security.Cryptography;
using Qowaiv.Diagnostics;
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
    public partial struct CryptographicSeed : ISerializable, IXmlSerializable, IFormattable, IEquatable<CryptographicSeed>, IComparable, IComparable<CryptographicSeed>
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

        /// <summary>Serializes the cryptographic seed to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => Length == 0 ? null : ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current cryptographic seed for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => IsEmpty() 
            ? DebugDisplay.Empty 
            : Base64.ToString(m_Value);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current cryptographic seed.</summary>
        /// <param name="format">
        /// The format that describes the formatting.
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

        /// <inheritdoc />
        public bool Equals(CryptographicSeed other)
        {
            if (Length == other.Length)
            {
                for (var i = 0; i < Length; i++)
                {
                    if (!m_Value[i].Equals(other.m_Value[i])) { return false; }
                }
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = 0;
            for (var i = 0; i < Length; i++)
            {
                hash ^= m_Value[i] << ((i * 17) % 24);
            }
            return hash;
        }

        /// <inheritdoc />
        public int CompareTo(CryptographicSeed other)
        {
            var minLength = Math.Min(Length, other.Length);

            for (var i = 0; i < minLength; i++)
            {
                var compare = m_Value[i].CompareTo(other.m_Value[i]);
                if (compare != 0)
                {
                    return compare;
                }
            }
            return Length.CompareTo(other.Length);
        }

        /// <summary>Casts a cryptographic seed to a <see cref="string"/>.</summary>
        public static explicit operator string(CryptographicSeed val) => val.ToString(CultureInfo.InvariantCulture);
        /// <summary>Casts a <see cref="string"/> to a cryptographic seed.</summary>
        public static explicit operator CryptographicSeed(string str) => Cast.InvariantString<CryptographicSeed>(TryParse, str);

        /// <summary>Casts a cryptographic seed to a System.byte[].</summary>
        public static explicit operator byte[](CryptographicSeed val) => val.ToByteArray();
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

            return new CryptographicSeed(bytes);
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
