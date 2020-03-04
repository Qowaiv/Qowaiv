#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Identifiers;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.Identifiers
{
    /// <summary>Represents a strongly typed identifier.</summary>
    /// <typeparam name="TIdentifier">
    /// The type of the <see cref="IIdentifierLogic"/> that deals with the
    /// identifier specific logic.
    /// </typeparam>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [OpenApiDataType(description: "identifier", type: "string/int")]
    [TypeConverter(typeof(IdTypeConverter))]
    public partial struct Id<TIdentifier> : ISerializable, IXmlSerializable, IFormattable, IEquatable<Id<TIdentifier>>, IComparable, IComparable<Id<TIdentifier>>
        where TIdentifier : IIdentifierLogic, new()
    {
        /// <summary>An singleton instance that deals with the identifier
        /// specific logic.
        /// </summary>
        private static readonly TIdentifier logic = new TIdentifier();

        /// <summary>Represents an empty/not set identifier.</summary>
        public static readonly Id<TIdentifier> Empty;

        /// <summary>Returns a <see cref="string"/> that represents the identifier for DEBUG purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => IsEmpty() ? "{empty}" : ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the identifier.</summary>
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

            return IsEmpty() ? string.Empty : logic.ToString(m_Value, format, formatProvider);
        }

        /// <summary>Gets an XML string representation of the identifier.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Serializes the identifier to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON node.
        /// </returns>
        public object ToJson() => IsEmpty() ? null : logic.ToJson(m_Value);

        /// <summary>Casts the identifier to a <see cref="string"/>.</summary>
        public static explicit operator string(Id<TIdentifier> id) => id.m_Value is string str ? str : null;

        /// <summary>Casts the identifier to a <see cref="long"/>.</summary>
        public static explicit operator long(Id<TIdentifier> id) => id.m_Value is long num ? num : 0;

        /// <summary>Casts the identifier to a <see cref="Guid"/>.</summary>
        public static explicit operator Guid(Id<TIdentifier> id) => id.m_Value is Guid guid ? guid : Guid.Empty;

        /// <summary>Casts the identifier to a <see cref="Uuid"/>.</summary>
        public static explicit operator Uuid(Id<TIdentifier> id) => id.m_Value is Uuid uuid ? uuid : Uuid.Empty;

        /// <summary>Converts the <see cref="string"/> to <see cref = "Id{TIdentifier}"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing the identifier to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Id<TIdentifier> result)
        {
            result = default;

            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            if (logic.TryParse(s, out var id))
            {
                result = new Id<TIdentifier>(id);
                return true;
            }
            return false;
        }
    }
}
