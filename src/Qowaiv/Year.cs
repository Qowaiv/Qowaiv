#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Diagnostics;
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
    [OpenApiDataType(description: "Year(-only) notation.", example: 1983, type: "integer", format: "year", nullable: true)]
    [TypeConverter(typeof(YearTypeConverter))]
    public partial struct Year : ISerializable, IXmlSerializable, IFormattable, IEquatable<Year>, IComparable, IComparable<Year>
    {
        /// <summary>Represents the pattern of a (potential) valid year.</summary>
        [Obsolete("Will become private when the next major version is released.")]
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
        public bool IsLeapYear => !IsEmptyOrUnknown() && DateTime.IsLeapYear(m_Value);

        /// <summary>Deserializes the year from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized year.
        /// </returns>
        public static Year FromJson(double json) => Create(Cast.ToInt<Year>(json));

        /// <summary>Deserializes the year from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized year.
        /// </returns>
        public static Year FromJson(long json) => Create(Cast.ToInt<Year>(json));

        /// <summary>Serializes the year to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON node.
        /// </returns>
        public object ToJson()
        {
            if (IsEmpty())
            {
                return null;
            }
            else if (IsUnknown())
            {
                return "?";
            }
            else
            {
                return (long)m_Value;
            }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current year for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => this.DebuggerDisplay("{0}");

        /// <summary>Returns a formatted <see cref="string"/> that represents the current year.</summary>
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
            else if (IsEmpty()) { return string.Empty; }
            else if (IsUnknown()) { return "?"; }
            else { return m_Value.ToString(format, formatProvider); }
        }

        /// <summary>Gets an XML string representation of the @FullName.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Year l, Year r) => HaveValue(l, r) && l.CompareTo(r) < 0;
       
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Year l, Year r) => HaveValue(l, r) && l.CompareTo(r) > 0;
        
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Year l, Year r) => HaveValue(l, r) && l.CompareTo(r) <= 0;
        
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Year l, Year r) => HaveValue(l, r) && l.CompareTo(r) >= 0;

        private static bool HaveValue(Year l, Year r) => !l.IsEmptyOrUnknown() && !r.IsEmptyOrUnknown();

        /// <summary>Casts a year to a <see cref="string"/>.</summary>
        public static explicit operator string(Year val) => val.ToString(CultureInfo.CurrentCulture);
        
        /// <summary>Casts a <see cref="string"/> to a year.</summary>
        public static explicit operator Year(string str) => Cast.String<Year>(TryParse, str);

        /// <summary>Casts a year to a System.Int32.</summary>
        public static explicit operator int(Year val) => val.m_Value;

        /// <summary>Casts an System.Int32 to a year.</summary>
        public static implicit operator Year(int val) => Cast.Primitive<int, Year>(TryCreate, val);

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
            if (Pattern.IsMatch(s))
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
            => TryCreate(val, out Year result)
            ? result
            : throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionYear);

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
            => TryCreate(val, out Year result)
            ? result
            : default;

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
            else if (IsValid(val.Value))
            {
                result = new Year((short)val.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Returns true if the val represents a valid year, otherwise false.</summary>
        public static bool IsValid(int? val)
            => val.HasValue
            && val.Value >= MinValue.m_Value 
            && val.Value <= MaxValue.m_Value;
    }
}
