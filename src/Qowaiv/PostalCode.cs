#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a postal code.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [OpenApiDataType(description: "Postal code notation.", type: "string", format: "postal-code", nullable: true)]
    [TypeConverter(typeof(PostalCodeTypeConverter))]
    public partial struct PostalCode : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<PostalCode>, IComparable, IComparable<PostalCode>
    {
        /// <summary>Represents the pattern of a (potential) valid postal code.</summary>
        public static readonly Regex Pattern = new Regex(@"^.{2,10}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents an empty/not set postal code.</summary>
        public static readonly PostalCode Empty;

        /// <summary>Represents an unknown (but set) postal code.</summary>
        public static readonly PostalCode Unknown = new PostalCode("ZZZZZZZZZ");

        /// <summary>Gets the number of characters of postal code.</summary>
        public int Length => m_Value is null ? 0 : m_Value.Length;

        /// <summary>Returns true if the postal code is valid for the specified country, otherwise false.</summary>
        /// <param name="country">
        /// The country to valid for.
        /// </param>
        /// <remarks>
        /// Returns false if the country does not have postal codes.
        /// </remarks>
        public bool IsValid(Country country)
        {
            return IsValid(m_Value, country);
        }

        /// <summary>Returns a collection countries where the postal code is valid for.</summary>
        public IEnumerable<Country> IsValidFor()
        {
            var postalcode = m_Value;
            return Country.All.Where(country => IsValid(postalcode, country));
        }

        /// <summary>Generates a postal code from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson()
        {
            m_Value = default;
        }

        /// <summary>Generates a postal code from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the postal code.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a postal code from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the postal code.
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger) => throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates a postal code from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the postal code.
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a postal code from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the postal code.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a postal code into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => m_Value;

        /// <summary>Returns a <see cref="string"/> that represents the current postal code for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "PostalCode: {0}{1}",
                    ToString(),
                    this == Empty ? "(empty)" : "");
            }
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current postal code.</summary>
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
            return ToString(Country.TryParse(format));
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current postal code.</summary>
        /// <param name="country">
        /// The country to format for.
        /// </param>
        /// <remarks>
        /// If the postal code is not valid for the country,
        /// or the country does not have special formattings, 
        /// the unformatted value is returned.
        /// </remarks>
        public string ToString(Country country)
        {
            // send a question mark in case of Unknown.
            var normalized = Unknown.m_Value == m_Value ? "?" : m_Value;
            return PostalCodeCountryInfo.GetInstance(country).Format(normalized);
        }

        /// <summary>Gets an XML string representation of the postal code.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts a postal code to a <see cref="string"/>.</summary>
        public static explicit operator string(PostalCode val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a postal code.</summary>
        public static explicit operator PostalCode(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to a postal code.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a postal code to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out PostalCode result)
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
                result = new PostalCode(Parsing.ClearSpacingAndMarkupToUpper(s));
                return true;
            }
            return false;
        }

        /// <summary>Returns true if the postal code is valid for the specified country, otherwise false.</summary>
        /// <param name="postalcode">
        /// The postal code to test.
        /// </param>
        /// <param name="country">
        /// The country to valid for.
        /// </param>
        /// <remarks>
        /// Returns false if the country does not have postal codes.
        /// </remarks>
        public static bool IsValid(string postalcode, Country country)
        {
            return PostalCodeCountryInfo.GetInstance(country).IsValid(postalcode);
        }
    }
}
