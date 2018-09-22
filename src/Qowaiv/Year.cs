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
    /// <summary>Represents a year.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for a year.")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(Int16))]
    [TypeConverter(typeof(YearTypeConverter))]
    public struct Year : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Year>, IComparable, IComparable<Year>
    {
        /// <summary>Represents the pattern of a (potential) valid year.</summary>
        public static readonly Regex Pattern = new Regex(@"(^[0-9]{1,4}$)(?<!^0+$)", RegexOptions.Compiled);

        /// <summary>Represents an empty/not set year.</summary>
        public static readonly Year Empty;

        /// <summary>Represents an unknown (but set) year.</summary>
        public static readonly Year Unknown = new Year { m_Value = Int16.MaxValue };

        /// <summary>Represents the smallest possible year 1.</summary>
        public static readonly Year MinValue = new Year { m_Value = 1 };

        /// <summary>Represents the largest possible year 9999.</summary>
        public static readonly Year MaxValue = new Year { m_Value = 9999 };

        #region Properties

        /// <summary>The inner value of the year.</summary>
        private Int16 m_Value;

        /// <summary>Returns an indication whether the specified year is a leap year.</summary>
        /// <returns>
        /// true if year is a leap year; otherwise, false.
        /// </returns>
        public bool IsLeapYear
        {
            get
            {
                return !IsEmptyOrUnknown() && DateTime.IsLeapYear(m_Value);
            }
        }

        #endregion

        #region Methods

        /// <summary>Returns true if the year is empty, otherwise false.</summary>
        public bool IsEmpty() { return m_Value == default(Int16); }

        /// <summary>Returns true if the year is unknown, otherwise false.</summary>
        public bool IsUnknown() { return m_Value == Year.Unknown.m_Value; }

        /// <summary>Returns true if the year is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of year based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Year(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, "info");
            m_Value = info.GetInt16("Value");
        }

        /// <summary>Adds the underlying property of year to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, "info");
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a year.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the year from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of year.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, "reader");
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the year to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of year.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, "writer");
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a year from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson()
        {
            m_Value = default(Int32);
        }

        /// <summary>Generates a year from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the year.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a year from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the year.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger)
        {
            m_Value = Create((Int32)jsonInteger).m_Value;
        }

        /// <summary>Generates a year from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the year.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber)
        {
            m_Value = Create((Int32)jsonNumber).m_Value;
        }

        /// <summary>Generates a year from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the year.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a year into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            if (IsEmpty()) { return null; }
            if (IsUnknown()) { return "?"; }
            return m_Value;
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current year for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty()) { return "Year: (empty)"; }
                if (IsUnknown()) { return "Year: (unknown)"; }
                return string.Format(CultureInfo.InvariantCulture, "Year: {0}", m_Value);
            }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current year.</summary>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current year.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current year.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("", formatProvider);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current year.</summary>
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
            if (IsEmpty()) { return string.Empty; }
            if (IsUnknown()) { return "?"; }
            return m_Value.ToString(format, formatProvider);
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) { return obj is Year && Equals((Year)obj); }

        /// <summary>Returns true if this instance and the other <see cref="Year"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="Year"/> to compare with.</param>
        public bool Equals(Year other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this year.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() { return m_Value; }

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Year left, Year right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Year left, Year right)
        {
            return !(left == right);
        }

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a year.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a year.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Year)
            {
                return CompareTo((Year)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a year"), "obj");
        }

        /// <summary>Compares this instance with a specified year and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified year.
        /// </summary>
        /// <param name="other">
        /// The year to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Year other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Year l, Year r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Year l, Year r) { return l.m_Value > r.m_Value; }

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Year l, Year r) { return l.m_Value <= r.m_Value; }

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Year l, Year r) { return l.m_Value >= r.m_Value; }

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a year to a <see cref="string"/>.</summary>
        public static explicit operator string(Year val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a year.</summary>
        public static explicit operator Year(string str) { return Year.Parse(str, CultureInfo.CurrentCulture); }



        /// <summary>Casts a year to a System.Int32.</summary>
        public static explicit operator Int32(Year val) => val.m_Value;
        /// <summary>Casts an System.Int32 to a year.</summary>
        public static implicit operator Year(Int32 val) { return Year.Create(val); }

        #endregion

        #region Factory methods

        /// <summary>Converts the string to a year.</summary>
        /// <param name="s">
        /// A string containing a year to convert.
        /// </param>
        /// <returns>
        /// A year.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Year Parse(string s)
        {
            return Parse(s, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the string to a year.</summary>
        /// <param name="s">
        /// A string containing a year to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A year.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Year Parse(string s, IFormatProvider formatProvider)
        {
            Year val;
            if (Year.TryParse(s, formatProvider, out val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionYear);
        }

        /// <summary>Converts the string to a year.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a year to convert.
        /// </param>
        /// <returns>
        /// The year if the string was converted successfully, otherwise Year.Empty.
        /// </returns>
        public static Year TryParse(string s)
        {
            Year val;
            if (Year.TryParse(s, out val))
            {
                return val;
            }
            return Year.Empty;
        }

        /// <summary>Converts the string to a year.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a year to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Year result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to a year.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a year to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out Year result)
        {
            result = Year.Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
            if (Qowaiv.Unknown.IsUnknown(s, culture))
            {
                result = Year.Unknown;
                return true;
            }
            if (IsValid(s, formatProvider))
            {
                result = new Year { m_Value = Int16.Parse(s, formatProvider) };
                return true;
            }
            return false;
        }



        /// <summary>Creates a year from a Int32. </summary >
        /// <param name="val" >
        /// A decimal describing a year.
        /// </param >
        /// <exception cref="FormatException" >
        /// val is not a valid year.
        /// </exception >
        public static Year Create(Int32? val)
        {
            Year result;
            if (Year.TryCreate(val, out result))
            {
                return result;
            }
            throw new ArgumentOutOfRangeException("val", QowaivMessages.FormatExceptionYear);
        }

        /// <summary>Creates a year from a Int32.
        /// A return value indicates whether the conversion succeeded.
        /// </summary >
        /// <param name="val" >
        /// A decimal describing a year.
        /// </param >
        /// <returns >
        /// A year if the creation was successfully, otherwise Year.Empty.
        /// </returns >
        public static Year TryCreate(Int32? val)
        {
            Year result;
            if (TryCreate(val, out result))
            {
                return result;
            }
            return Year.Empty;
        }

        /// <summary>Creates a year from a Int32.
        /// A return value indicates whether the creation succeeded.
        /// </summary >
        /// <param name="val" >
        /// A Int32 describing a year.
        /// </param >
        /// <param name="result" >
        /// The result of the creation.
        /// </param >
        /// <returns >
        /// True if a year was created successfully, otherwise false.
        /// </returns >
        public static bool TryCreate(Int32? val, out Year result)
        {
            result = Year.Empty;

            if (!val.HasValue)
            {
                return true;
            }
            if (IsValid(val.Value))
            {
                result = new Year { m_Value = (Int16)val.Value };
                return true;
            }
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid year, otherwise false.</summary>
        public static bool IsValid(string val)
        {
            return IsValid(val, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns true if the val represents a valid year, otherwise false.</summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "formatProvider",
            Justification = "Satisfies the static Qowaiv SVO contract.")]
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return Pattern.IsMatch(val ?? string.Empty);
        }

        /// <summary>Returns true if the val represents a valid year, otherwise false.</summary>
        public static bool IsValid(Int32? val)
        {
            if (!val.HasValue) { return false; }
            return val.Value >= MinValue.m_Value && val.Value <= MaxValue.m_Value;
        }
        #endregion
    }
}
