#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

namespace Qowaiv
{
    using Qowaiv.Formatting;
    using Qowaiv.Globalization;
    using Qowaiv.Json;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>Represents a telephone number.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [OpenApiDataType(description: "telephone number", type: "string", format: "telephone-number", pattern: "\\+?[0-9]{3,}")]
    [TypeConverter(typeof(Conversion.TelephoneNumberTypeConverter))]
    public partial struct TelephoneNumber : ISerializable, IXmlSerializable, IFormattable, IEquatable<TelephoneNumber>, IComparable, IComparable<TelephoneNumber>
    {
        /// <summary>Represents an empty/not set telephone number.</summary>
        public static readonly TelephoneNumber Empty;

        /// <summary>Represents an unknown (but set) telephone number.</summary>
        public static readonly TelephoneNumber Unknown = new TelephoneNumber("?");

        /// <summary>Gets the number of characters of telephone number.</summary>
        public int Length => m_Value == null ? 0 : m_Value.Length;

        /// <summary>Gets the type of the telephone number.</summary>
        public TelephoneNumberType Type
        {
            get
            {
                if (IsEmpty())
                {
                    return TelephoneNumberType.None;
                }
                else if (IsUnknown())
                {
                    return TelephoneNumberType.Unknown;
                }
                else if (m_Value.StartsWith("+", StringComparison.InvariantCulture))
                {
                    return TelephoneNumberType.International;
                }
                else if (m_Value.StartsWith("0", StringComparison.InvariantCulture))
                {
                    return TelephoneNumberType.Regional;
                }
                else
                {
                    return TelephoneNumberType.Local;
                }
            }
        }

        /// <summary>Gets the country linked to the international telephone number.</summary>
        /// <remarks>
        /// Returns <see cref="Country.Empty"/> for numbers that are not international
        /// and <see cref="Country.Unknown"/> if no country could be linked.
        /// </remarks>
        public Country Country
        {
            get
            {
                var str = m_Value;
                return Type == TelephoneNumberType.International
                    ? Country.All
                        .OrderByDescending(c => c.CallingCode.Length)
                        .FirstOrDefault(c => str.StartsWith(c.CallingCode, StringComparison.InvariantCulture))
                    : Country.Empty;
            }
        }

        /// <summary>Returns a <see cref = "string "/> that represents the telephone number for DEBUG purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a formatted <see cref = "string "/> that represents the telephone number.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name = "formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            return m_Value ?? string.Empty;
        }

        /// <summary>Gets an XML string representation of the telephone number.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);
        /// <summary>Serializes the telephone number to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

        #region (Explicit) casting
        /// <summary>Casts the telephone number to a <see cref = "string "/>.</summary>
        public static explicit operator string(TelephoneNumber val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref = "string "/> to a telephone number.</summary>
        public static explicit operator TelephoneNumber(string str) => Parse(str, CultureInfo.CurrentCulture);

        #endregion

        /// <summary>Converts the <see cref = "string "/> to <see cref = "TelephoneNumber"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the telephone number to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out TelephoneNumber result)
        {
            result = default;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            else if (Qowaiv.Unknown.IsUnknown(s, formatProvider as CultureInfo))
            {
                result = Unknown;
                return true;
            }
            else
            {
                var parsed = TelephoneParser.Parse(s);
                result = new TelephoneNumber(parsed);
                return parsed is object;
            }
        }
    }
}
