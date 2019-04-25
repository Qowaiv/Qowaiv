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
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a Yes-no.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(byte))]
    [TypeConverter(typeof(YesNoTypeConverter))]
    public struct YesNo : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<YesNo>, IComparable, IComparable<YesNo>
    {
        /// <summary>Represents the pattern of a (potential) valid Yes-no.</summary>
        public static readonly Regex Pattern = new Regex(@"^ComplexRegexPattern.*$", RegexOptions.Compiled);

        /// <summary>Represents an empty/not set Yes-no.</summary>
        public static readonly YesNo Empty;

        /// <summary>Represents an unknown (but set) Yes-no.</summary>
        public static readonly YesNo No = new YesNo { m_Value = 1 };

        /// <summary>Represents an unknown (but set) Yes-no.</summary>
        public static readonly YesNo Yes = new YesNo { m_Value = 2 };

        /// <summary>Represents an unknown (but set) Yes-no.</summary>
        public static readonly YesNo Unknown = new YesNo { m_Value = 4 };

        #region Properties

        /// <summary>The inner value of the Yes-no.</summary>
        private byte m_Value;

        #endregion

        #region Methods

        /// <summary>Returns true if the Yes-no is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default(byte);

        /// <summary>Returns true if the Yes-no is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;

        /// <summary>Returns true if the Yes-no is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of Yes-no based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private YesNo(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetByte("Value");
        }

        /// <summary>Adds the underlying property of Yes-no to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a Yes-no.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the Yes-no from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of Yes-no.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the Yes-no to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of Yes-no.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion
        
        #region (JSON) (De)serialization

        /// <summary>Generates a Yes-no from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = default(byte);

        /// <summary>Generates a Yes-no from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the Yes-no.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString) => m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;

        /// <summary>Generates a Yes-no from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the Yes-no.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger) => throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);
        // m_Value = Create(jsonInteger).m_Value;

        /// <summary>Generates a Yes-no from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the Yes-no.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);
        // m_Value = Create(jsonNumber).m_Value;
        
        /// <summary>Generates a Yes-no from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the Yes-no.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);
        // m_Value = Create(jsonDate).m_Value;

        /// <summary>Converts a Yes-no into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => m_Value == default(byte) ? null : ToString(CultureInfo.InvariantCulture);

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current Yes-no for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty())
                {
                    return "YesNo: (empty)";
                }
                if (IsUnknown())
                {
                    return "YesNo: (unknown)";
                }
                return string.Format(CultureInfo.InvariantCulture, "YesNo: {0}", m_Value);
            }
        }

         /// <summary>Returns a <see cref="string"/> that represents the current Yes-no.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Yes-no.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Yes-no.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Yes-no.</summary>
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
            throw new NotImplementedException();
        }

        #endregion
        
        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is YesNo && Equals((YesNo)obj);

        /// <summary>Returns true if this instance and the other <see cref="YesNo" /> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="YesNo" /> to compare with.</param>
        public bool Equals(YesNo other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this Yes-no.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(YesNo left, YesNo right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(YesNo left, YesNo right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a Yes-no.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a Yes-no.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is YesNo)
            {
                return CompareTo((YesNo)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a Yes-no"), "obj");
        }

        /// <summary>Compares this instance with a specified Yes-no and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified Yes-no.
        /// </summary>
        /// <param name="other">
        /// The Yes-no to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(YesNo other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(YesNo l, YesNo r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(YesNo l, YesNo r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(YesNo l, YesNo r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(YesNo l, YesNo r) => l.CompareTo(r) >= 0;

        #endregion
       
        #region (Explicit) casting

        /// <summary>Casts a Yes-no to a <see cref="string"/>.</summary>
        public static explicit operator string(YesNo val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a Yes-no.</summary>
        public static explicit operator YesNo(string str) => Parse(str, CultureInfo.CurrentCulture);

         /// <summary>Casts a Yes-no to a System.Int32.</summary>
        public static explicit operator int(YesNo val) => val.m_Value;
        /// <summary>Casts an System.Int32 to a Yes-no.</summary>
        public static explicit operator YesNo(int val) => Create(val);

        #endregion

        #region Factory methods

        /// <summary>Converts the string to a Yes-no.</summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <returns>
        /// A Yes-no.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static YesNo Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to a Yes-no.</summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A Yes-no.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static YesNo Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out YesNo val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionYesNo);
        }

        /// <summary>Converts the string to a Yes-no.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <returns>
        /// The Yes-no if the string was converted successfully, otherwise Empty.
        /// </returns>
        public static YesNo TryParse(string s)
        {
            if (TryParse(s, out YesNo val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to a Yes-no.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out YesNo result) => TryParse(s, CultureInfo.CurrentCulture, out result);

        /// <summary>Converts the string to a Yes-no.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out YesNo result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
            if (Qowaiv.Unknown.IsUnknown(s, culture))
            {
                result = Unknown;
                return true;
            }
            if (IsValid(s, culture))
            {
                result = new YesNo { m_Value = byte.Parse(s, formatProvider) };
                return true;
            }
            return false;
        }

        /// <summary >Creates a Yes-no from a byte. </summary >
        /// <param name="val" >
        /// A decimal describing a Yes-no.
        /// </param >
        /// <exception cref="System.FormatException" >
        /// val is not a valid Yes-no.
        /// </exception >
        public static YesNo Create(int? val)
        {
            if (TryCreate(val, out YesNo result))
            {
                return result;
            }
            throw new ArgumentOutOfRangeException("val", QowaivMessages.FormatExceptionYesNo);
        }

        /// <summary >Creates a Yes-no from a byte.
        /// A return value indicates whether the conversion succeeded.
        /// </summary >
        /// <param name="val" >
        /// A decimal describing a Yes-no.
        /// </param >
        /// <returns >
        /// A Yes-no if the creation was successfully, otherwise Empty.
        /// </returns >
        public static YesNo TryCreate(int? val)
        {
            if(TryCreate(val, out YesNo result))
            {
                return result;
            }
            return Empty;
        }

        /// <summary >Creates a Yes-no from a byte.
        /// A return value indicates whether the creation succeeded.
        /// </summary >
        /// <param name="val" >
        /// A byte describing a Yes-no.
        /// </param >
        /// <param name="result" >
        /// The result of the creation.
        /// </param >
        /// <returns >
        /// True if a Yes-no was created successfully, otherwise false.
        /// </returns >
        public static bool TryCreate(int? val, out YesNo result)
        {
            result = Empty;

            if (!val.HasValue)
            {
                return true;
            }
            if (IsValid((byte)val.Value))
            {
                result = new YesNo { m_Value = (byte)val.Value };
                return true;
            }
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid Yes-no, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid Yes-no, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return Pattern.IsMatch(val ?? string.Empty);
        }

        /// <summary>Returns true if the val represents a valid Yes-no, otherwise false.</summary>
        public static bool IsValid(byte? val)
        {
            if(!val.HasValue)
            {
                return false;
            }
            throw new NotImplementedException();
        }
        #endregion
     }
}
