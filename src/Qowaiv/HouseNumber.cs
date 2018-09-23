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
    /// <summary>Represents a house number.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(Int32))]
    [TypeConverter(typeof(HouseNumberTypeConverter))]
    public struct HouseNumber : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<HouseNumber>, IComparable, IComparable<HouseNumber>
    {
        /// <summary>Represents the pattern of a (potential) valid house number.</summary>
        public static readonly Regex Pattern = new Regex(@"^[1-9][0-9]{0,8}$", RegexOptions.Compiled);

        /// <summary>Represents an empty/not set house number.</summary>
        public static readonly HouseNumber Empty;

        /// <summary>Represents an unknown (but set) house number.</summary>
        public static readonly HouseNumber Unknown = new HouseNumber { m_Value = Int32.MaxValue };

        /// <summary>Represents the smallest possible House number 1.</summary>
        public static readonly HouseNumber MinValue = new HouseNumber { m_Value = 1 };

        /// <summary>Represents the largest possible House number 999999999.</summary>
        public static readonly HouseNumber MaxValue = new HouseNumber { m_Value = 999999999 };

        #region Properties

        /// <summary>The inner value of the house number.</summary>
        private Int32 m_Value;

        /// <summary>Returns true if the House nummber is odd, otherwise false.</summary>
        /// <remarks>
        /// The empty and unknown value are not odd.
        /// </remarks>
        public bool IsOdd => !IsEmptyOrUnknown() && m_Value % 2 == 1;

        /// <summary>Returns true if the House number is even, otherwise false.</summary>
        /// <remarks>
        /// The empty and unknown value are not even.
        /// </remarks>
        public bool IsEven => !IsEmptyOrUnknown() && m_Value % 2 == 0;

        /// <summary>Gets the number of digits.</summary>
        public int Length
        {
            get
            {
                if (IsEmptyOrUnknown()) { return 0; }
                var length = 0;
                var val = m_Value;
                while (val != 0)
                {
                    val /= 10;
                    length++;
                }
                return length;
            }
        }

        #endregion

        #region Methods

        /// <summary>Returns true if the house number is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default(Int32);

        /// <summary>Returns true if the house number is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;

        /// <summary>Returns true if the house number is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of house number based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private HouseNumber(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetInt32("Value");
        }

        /// <summary>Adds the underlying property of house number to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a house number.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the house number from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of house number.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the house number to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of house number.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a house number from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson()
        {
            m_Value = default(Int32);
        }

        /// <summary>Generates a house number from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the house number.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a house number from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the house number.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger)
        {
            m_Value = Create((Int32)jsonInteger).m_Value;
        }

        /// <summary>Generates a house number from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the house number.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber)
        {
            m_Value = Create((Int32)jsonNumber).m_Value;
        }

        /// <summary>Generates a house number from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the house number.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a house number into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return m_Value == default(Int32) ? null : ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current house number for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty()) { return "HouseNumber: (empty)"; }
                if (IsUnknown()) { return "HouseNumber: (unknown)"; }
                return string.Format(CultureInfo.InvariantCulture, "HouseNumber: {0}", m_Value);
            }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current house number.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current house number.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current house number.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current house number.</summary>
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
            if (IsUnknown()) { return "?"; }
            if (IsEmpty()) { return string.Empty; }
            return m_Value.ToString(format, formatProvider);
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is HouseNumber && Equals((HouseNumber)obj);

        /// <summary>Returns true if this instance and the other <see cref="HouseNumber"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="HouseNumber"/> to compare with.</param>
        public bool Equals(HouseNumber other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this house number.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value;

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(HouseNumber left, HouseNumber right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(HouseNumber left, HouseNumber right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a house number.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a house number.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is HouseNumber)
            {
                return CompareTo((HouseNumber)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a house number"), "obj");
        }

        /// <summary>Compares this instance with a specified house number and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified house number.
        /// </summary>
        /// <param name="other">
        /// The house number to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(HouseNumber other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(HouseNumber l, HouseNumber r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(HouseNumber l, HouseNumber r) => l.m_Value > r.m_Value;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(HouseNumber l, HouseNumber r) => l.m_Value <= r.m_Value;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(HouseNumber l, HouseNumber r) => l.m_Value >= r.m_Value;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a house number to a <see cref="string"/>.</summary>
        public static explicit operator string(HouseNumber val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a house number.</summary>
        public static explicit operator HouseNumber(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts a house number to a System.Int32.</summary>
        public static explicit operator Int32(HouseNumber val) => val.m_Value;
        /// <summary>Casts an System.Int32 to a house number.</summary>
        public static implicit operator HouseNumber(Int32 val) => Create(val);

        /// <summary>Casts a house number to a System.Int64.</summary>
        public static explicit operator Int64(HouseNumber val) => val.m_Value;
        /// <summary>Casts a System.Int64 to a house number.</summary>
        public static implicit operator HouseNumber(Int64 val) => Create((Int32)val);
        #endregion

        #region Factory methods

        /// <summary>Converts the string to a house number.</summary>
        /// <param name="s">
        /// A string containing a house number to convert.
        /// </param>
        /// <returns>
        /// A house number.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static HouseNumber Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to a house number.</summary>
        /// <param name="s">
        /// A string containing a house number to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A house number.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static HouseNumber Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out HouseNumber val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionHouseNumber);
        }

        /// <summary>Converts the string to a house number.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a house number to convert.
        /// </param>
        /// <returns>
        /// The house number if the string was converted successfully, otherwise Empty.
        /// </returns>
        public static HouseNumber TryParse(string s)
        {
            if (TryParse(s, out HouseNumber val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to a house number.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a house number to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out HouseNumber result) => TryParse(s, CultureInfo.CurrentCulture, out result);

        /// <summary>Converts the string to a house number.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a house number to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out HouseNumber result)
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
            if (IsValid(s, formatProvider))
            {
                result = new HouseNumber { m_Value = int.Parse(s, formatProvider) };
                return true;
            }
            return false;
        }



        /// <summary>Creates a house number from a Int32. </summary >
        /// <param name="val" >
        /// A decimal describing a house number.
        /// </param >
        /// <exception cref="FormatException" >
        /// val is not a valid house number.
        /// </exception >
        public static HouseNumber Create(Int32? val)
        {
            HouseNumber result;
            if (TryCreate(val, out result))
            {
                return result;
            }
            throw new ArgumentOutOfRangeException("val", QowaivMessages.FormatExceptionHouseNumber);
        }

        /// <summary>Creates a house number from a Int32.
        /// A return value indicates whether the conversion succeeded.
        /// </summary >
        /// <param name="val" >
        /// A decimal describing a house number.
        /// </param >
        /// <returns >
        /// A house number if the creation was successfully, otherwise Empty.
        /// </returns >
        public static HouseNumber TryCreate(Int32? val)
        {
            if (TryCreate(val, out HouseNumber result))
            {
                return result;
            }
            return Empty;
        }

        /// <summary>Creates a house number from a Int32.
        /// A return value indicates whether the creation succeeded.
        /// </summary >
        /// <param name="val" >
        /// A Int32 describing a house number.
        /// </param >
        /// <param name="result" >
        /// The result of the creation.
        /// </param >
        /// <returns >
        /// True if a house number was created successfully, otherwise false.
        /// </returns >
        public static bool TryCreate(Int32? val, out HouseNumber result)
        {
            result = Empty;

            if (!val.HasValue)
            {
                return true;
            }
            if (IsValid(val.Value))
            {
                result = new HouseNumber { m_Value = val.Value };
                return true;
            }
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid house number, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid house number, otherwise false.</summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "formatProvider",
            Justification = "Satisfies the static Qowaiv SVO contract.")]
        public static bool IsValid(string val, IFormatProvider formatProvider) => Pattern.IsMatch(val ?? string.Empty);

        /// <summary>Returns true if the val represents a valid house number, otherwise false.</summary>
        public static bool IsValid(Int32? val)
        {
            if (!val.HasValue) { return false; }

            return val.Value >= MinValue.m_Value && val.Value <= MaxValue.m_Value;
        }
        #endregion
    }
}
