#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a year.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(short))]
    [OpenApiDataType(description: "Year(-only) notation.", type: "integer", format: "year", nullable: true)]
    [TypeConverter(typeof(YearTypeConverter))]
    public partial struct Year : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Year>, IComparable, IComparable<Year>
    { 
        /// <summary>Gets a culture dependent message when a <see cref="FormatException"/> occurs.</summary>
        private static readonly string FormatExceptionMessage = QowaivMessages.FormatExceptionYear;

        /// <summary>Represents the pattern of a (potential) valid year.</summary>
        public static readonly Regex Pattern = new Regex(@"(^[0-9]{1,4}$)(?<!^0+$)", RegexOptions.Compiled);

        /// <summary>Represents an empty/not set year.</summary>
        public static readonly Year Empty;

        /// <summary>Represents an unknown (but set) year.</summary>
        public static readonly Year Unknown = new Year(short.MaxValue);

        /// <summary>Represents the smallest possible year 1.</summary>
        public static readonly Year MinValue = new Year(1);

        /// <summary>Represents the largest possible year 9999.</summary>
        public static readonly Year MaxValue = new Year(9999);

        /// <summary>Returns an indication whether the specified year is a leap year.</summary>
        /// <returns>
        /// true if year is a leap year; otherwise, false.
        /// </returns>
        public bool IsLeapYear
        {
            get => !IsEmptyOrUnknown() && DateTime.IsLeapYear(m_Value);
        }

        /// <summary>Generates a year from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = default;

        /// <summary>Generates a year from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the year.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString) => m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;

        /// <summary>Generates a year from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the year.
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger) => m_Value = Create((int)jsonInteger).m_Value;

        /// <summary>Generates a year from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the year.
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber) => m_Value = Create((int)jsonNumber).m_Value;

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

        /// <summary>Returns a <see cref="string"/> that represents the current year for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty()) { return "Year: (empty)"; }
                if (IsUnknown()) { return "Year: (unknown)"; }
                return string.Format(CultureInfo.InvariantCulture, "Year: {0}", m_Value);
            }
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

        /// <summary>Gets an XML string representation of the @FullName.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts a year to a <see cref="string"/>.</summary>
        public static explicit operator string(Year val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a year.</summary>
        public static explicit operator Year(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts a year to a System.Int32.</summary>
        public static explicit operator int(Year val) => val.m_Value;
        /// <summary>Casts an System.Int32 to a year.</summary>
        public static implicit operator Year(int val) => Create(val);

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
            result = default;
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
                result = new Year(short.Parse(s, formatProvider));
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
        public static Year Create(int? val)
        {
            if (TryCreate(val, out Year result))
            {
                return result;
            }
            throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionYear);
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
        public static Year TryCreate(int? val)
        {
            if (TryCreate(val, out Year result))
            {
                return result;
            }
            return Empty;
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
        public static bool TryCreate(int? val, out Year result)
        {
            result = default;

            if (!val.HasValue)
            {
                return true;
            }
            if (IsValid(val.Value))
            {
                result = new Year((short)val.Value);
                return true;
            }
            return false;
        }

        /// <summary>Returns true if the val represents a valid year, otherwise false.</summary>
        public static bool IsValid(int? val)
        {
            if (!val.HasValue) { return false; }
            return val.Value >= MinValue.m_Value && val.Value <= MaxValue.m_Value;
        }

        /// <summary>Creates the year based on an XML string.</summary>
        /// <param name="xmlString">
        /// The XML string representing the year.
        /// </param>
        private static Year FromXml(string xmlString) => Parse(xmlString, CultureInfo.InvariantCulture);

    }
}
