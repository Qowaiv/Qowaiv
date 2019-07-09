#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a UUID (Universally unique identifier) aka GUID (Globally unique identifier).</summary>
    /// <remarks>
    /// The main difference between this UUID and the default System.GUID is 
    /// the default string representation. For this, that is a 22 char long
    /// Base64 representation.
    /// 
    /// The reason not to call this a GUID but an UUID it just to prevent users of 
    /// Qowaiv to be forced to specify the namespace of there GUID of choice 
    /// everywhere.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.AllExcludingCulture ^ SingleValueStaticOptions.HasUnknownValue, typeof(Guid))]
    [TypeConverter(typeof(UuidTypeConverter))]
    public struct Uuid : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Uuid>, IComparable, IComparable<Uuid>
    {
        /// <summary>Represents the pattern of a (potential) valid GUID.</summary>
        public static readonly Regex Pattern = new Regex(@"^[a-zA-Z0-9_-]{22}(=){0,2}$", RegexOptions.Compiled);

        /// <summary>Represents an empty/not set GUID.</summary>
        public static readonly Uuid Empty;

        /// <summary>Initializes a new instance of a GUID.</summary>
        private Uuid(Guid id) => m_Value = id;

        #region Properties

        /// <summary>The inner value of the UUID.</summary>
        private Guid m_Value;

        /// <summary>Get the version of the UUID.</summary>
        public UuidVersion Version => m_Value.GetVersion();

        #endregion

        #region Methods

        /// <summary>Returns true if the UUID is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;

        /// <summary>Returns a 16-element byte array that contains the value of this instance.</summary>
        public byte[] ToByteArray() => m_Value.ToByteArray();

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of UUID based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Uuid(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (Guid)info.GetValue("Value", typeof(Guid));
        }

        /// <summary>Adds the underlying property of UUID to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a UUID.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the UUID from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of UUID.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the UUID to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of UUID.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a UUID from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = default;

        /// <summary>Generates a UUID from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the UUID.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString) => m_Value = Parse(jsonString).m_Value;

        /// <summary>Generates a UUID from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the UUID.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger) => throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates a UUID from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the UUID.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a UUID from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the UUID.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);


        /// <summary>Converts a UUID into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current UUID for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay => ToString();

        /// <summary>Returns a <see cref="string"/> that represents the current UUID.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current UUID.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current UUID.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current UUID.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// S
        /// 22 base64 chars:
        /// 0123465798aAbBcCdDeE_-
        /// N
        /// 32 digits:
        /// 00000000000000000000000000000000
        /// D
        /// 32 digits separated by hyphens:
        /// 00000000-0000-0000-0000-000000000000
        /// B
        /// 32 digits separated by hyphens, enclosed in braces:
        /// {00000000-0000-0000-0000-000000000000}
        /// P
        /// 32 digits separated by hyphens, enclosed in parentheses:
        /// (00000000-0000-0000-0000-000000000000)
        /// X
        /// Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces:
        /// {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
        /// 
        /// the lowercase formats are lowercase (except the the 's').
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            if (IsEmpty()) { return string.Empty; }

            switch (format)
            {
                case null:
                case "":
                case "s":
                case "S":
                    // avoid invalid URL characters
                    return Convert.ToBase64String(ToByteArray()).Replace('+', '-').Replace('/', '_').Substring(0, 22);
                case "N":
                case "D":
                case "B":
                case "P": return m_Value.ToString(format, formatProvider).ToUpperInvariant();
                case "X": return m_Value.ToString(format, formatProvider).ToUpperInvariant().Replace('X', 'x');

                case "n":
                case "d":
                case "b":
                case "p":
                case "x":
                    return m_Value.ToString(format, formatProvider);

                default: throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
            }
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is Uuid && Equals((Uuid)obj);

        /// <summary>Returns true if this instance and the other <see cref="Uuid"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="Uuid"/> to compare with.</param>
        public bool Equals(Uuid other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this GUID.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Uuid left, Uuid right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Uuid left, Uuid right) => !(left == right);


        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a GUID.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a UUID.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Uuid || obj is Guid)
            {
                return CompareTo((Uuid)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a GUID"), "obj");
        }

        /// <summary>Compares this instance with a specified UUID and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified UUID.
        /// </summary>
        /// <param name="other">
        /// The GUID to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Uuid other) => m_Value.CompareTo(other.m_Value);

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a UUID to a <see cref="string"/>.</summary>
        public static explicit operator string(Uuid val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a UUID.</summary>
        public static explicit operator Uuid(string str) => Parse(str);

        /// <summary>Casts a Qowaiv.UUID to a System.GUID.</summary>
        public static implicit operator Guid(Uuid val) => val.m_Value;
        /// <summary>Casts a System.GUID to a Qowaiv.UUID.</summary>
        public static implicit operator Uuid(Guid val) => new Uuid(val);

        #endregion

        #region Factory methods

        /// <summary>Initializes a new instance of a UUID.</summary>
        public static Uuid NewUuid() => new Uuid(Guid.NewGuid());


        /// <summary>Converts the string to a UUID.</summary>
        /// <param name="s">
        /// A string containing a GUID to convert.
        /// </param>
        /// <returns>
        /// A UUID.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Uuid Parse(string s)
        {
            if (TryParse(s, out Uuid val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionQGuid);
        }

        /// <summary>Converts the string to a UUID.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a UUID to convert.
        /// </param>
        /// <returns>
        /// The GUID if the string was converted successfully, otherwise QGuid.Empty.
        /// </returns>
        public static Uuid TryParse(string s)
        {
            if (TryParse(s, out Uuid val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to a UUID.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a UUID to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Uuid result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            if (Pattern.IsMatch(s))
            {
                var bytes = Convert.FromBase64String(s.Replace('-', '+').Replace('_', '/').Substring(0, 22) + "==");
                result = new Uuid(new Guid(bytes));
                return true;
            }

            if (Guid.TryParse(s, out Guid id))
            {
                result = new Uuid(id);
                return true;
            }
            return false;
        }

        /// <summary>Generates an <see cref="Uuid"/> applying a <see cref="MD5"/> hash on the data.</summary>
        public static Uuid GenerateWithMD5(byte[] data)
        {
            Guard.NotNull(data, nameof(data));
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                UuidExtensions.SetVersion(hash, UuidVersion.MD5);
                return new Guid(hash);
            }
        }

        /// <summary>Generates an <see cref="Uuid"/> applying a <see cref="SHA1"/> hash on the data.</summary>
        public static Uuid GenerateWithSHA1(byte[] data)
        {
            Guard.NotNull(data, nameof(data));
            using (var sha1 = SHA1.Create())
            {
                var bytes = sha1.ComputeHash(data);
                var hash = new byte[16];
                Array.Copy(bytes, hash, 16);
                UuidExtensions.SetVersion(hash, UuidVersion.SHA1);
                return new Guid(hash);
            }
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the value represents a valid UUID, otherwise false.</summary>
        public static bool IsValid(string val)
        {
            return Pattern.IsMatch(val ?? string.Empty)
                || Guid.TryParse(val, out Guid id);
        }

        #endregion
    }
}
