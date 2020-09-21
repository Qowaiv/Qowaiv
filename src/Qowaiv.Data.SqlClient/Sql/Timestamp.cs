using Qowaiv.Conversion.Sql;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.Sql
{
    /// <summary>Represents a timestamp.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(ulong))]
    [OpenApiDataType(description: "SQL Server timestamp notation, for example 0x00000000000007D9.", type: "string", format: "timestamp")]
    [TypeConverter(typeof(TimestampTypeConverter))]
    public partial struct Timestamp : ISerializable, IXmlSerializable, IFormattable, IEquatable<Timestamp>, IComparable, IComparable<Timestamp>
    {
        /// <summary>Gets the minimum value of a timestamp.</summary>
        public static readonly Timestamp MinValue;

        /// <summary>Gets the maximum value of a timestamp.</summary>
        public static readonly Timestamp MaxValue = new Timestamp(ulong.MaxValue);

        /// <summary>Represents the timestamp .</summary>
        public byte[] ToByteArray() => BitConverter.GetBytes(m_Value);

        /// <summary>Deserializes the timestamp from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized timestamp.
        /// </returns>
        public static Timestamp FromJson(double json) => Create((long)json);

        /// <summary>Deserializes the timestamp from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized timestamp.
        /// </returns>
        public static Timestamp FromJson(long json) => Create(json);

        /// <summary>Serializes the timestamp to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current timestamp for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current timestamp.</summary>
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
            if (string.IsNullOrEmpty(format))
            {
                return string.Format(formatProvider, "0x{0:X16}", m_Value);
            }
            return m_Value.ToString(format, formatProvider);
        }

        /// <summary>Gets an XML string representation of the timestamp.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts a timestamp to a <see cref="string"/>.</summary>
        public static explicit operator string(Timestamp val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a timestamp.</summary>
        public static explicit operator Timestamp(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts a timestamp to a System.Int32.</summary>
        public static explicit operator byte[](Timestamp val) => val.ToByteArray();
        /// <summary>Casts an System.Int32 to a timestamp.</summary>
        public static explicit operator Timestamp(byte[] val) => Create(val);

        /// <summary>Casts a timestamp to a System.Int64.</summary>
        public static explicit operator long(Timestamp val) => BitConverter.ToInt64(val.ToByteArray(), 0);
        /// <summary>Casts a System.Int64 to a timestamp.</summary>
        public static explicit operator Timestamp(long val) => Create(val);

        /// <summary>Casts a timestamp to a System.UInt64.</summary>
        [CLSCompliant(false)]
        public static explicit operator ulong(Timestamp val) => val.m_Value;
        /// <summary>Casts a System.UInt64 to a timestamp.</summary>
        [CLSCompliant(false)]
        public static implicit operator Timestamp(ulong val) => Create(val);

        /// <summary>Converts the string to a timestamp.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a timestamp to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out Timestamp result)
        {
            result = default;
            ulong val;

            if (string.IsNullOrEmpty(s)) { return false; }
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (ulong.TryParse(s.Substring(2), NumberStyles.HexNumber, formatProvider, out val))
                {
                    result = Create(val);
                    return true;
                }
            }
            else if (ulong.TryParse(s, NumberStyles.Number, formatProvider, out val))
            {
                result = new Timestamp(val);
                return true;
            }
            return false;
        }

        /// <summary>Creates a timestamp from a Int64.</summary>
        /// <param name="val" >
        /// A decimal describing a timestamp.
        /// </param >
        [CLSCompliant(false)]
        public static Timestamp Create(ulong val) => new Timestamp(val);

        /// <summary>Creates a timestamp from a Int64.</summary>
        /// <param name="val" >
        /// A decimal describing a timestamp.
        /// </param >
        public static Timestamp Create(long val) => Create(BitConverter.GetBytes(val));

        /// <summary>Creates a timestamp from a Int64.</summary>
        /// <param name="bytes" >
        /// A byte array describing a timestamp.
        /// </param >
        public static Timestamp Create(byte[] bytes)
        {
            Guard.HasAny(bytes, nameof(bytes));
            if (bytes.Length != 8) { throw new ArgumentException(QowaivMessages.ArgumentException_TimestampArrayShouldHaveSize8, "bytes"); }

            return Create(BitConverter.ToUInt64(bytes, 0));
        }
    }
}
