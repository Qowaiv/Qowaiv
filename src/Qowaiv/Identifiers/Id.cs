#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Identifiers;
using Qowaiv.Diagnostics;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Identifiers
{
    /// <summary>Represents a strongly typed identifier.</summary>
    /// <typeparam name="TIdentifier">
    /// The type of the <see cref="IIdentifierBehavior"/> that deals with the
    /// identifier specific behavior.
    /// </typeparam>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [SingleValueObject(SingleValueStaticOptions.AllExcludingCulture ^ SingleValueStaticOptions.HasUnknownValue, typeof(object))]
    [OpenApiDataType(description: "identifier", type: "string/int")]
    [TypeConverter(typeof(IdTypeConverter))]
    public partial struct Id<TIdentifier> : ISerializable, IXmlSerializable, IFormattable, IEquatable<Id<TIdentifier>>, IComparable, IComparable<Id<TIdentifier>>
        where TIdentifier : IIdentifierBehavior, new()
    {
        /// <summary>An singleton instance that deals with the identifier specific behavior.</summary>
        private static readonly TIdentifier behavior = new TIdentifier();

        /// <summary>Represents an empty/not set identifier.</summary>
        public static readonly Id<TIdentifier> Empty;

        /// <summary>Creates a new instance of the <see cref="Id{TIdentifier}"/> struct.</summary>
        private Id(object value) => m_Value = value;

        /// <summary>Initializes a new instance of the identifier based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Id(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetValue("Value", typeof(object));
        }

        /// <summary>The inner value of the identifier.</summary>
        private object m_Value;

        /// <summary>Returns true if the identifier is empty, otherwise false.</summary>
        public bool IsEmpty()
            => m_Value == default
            || m_Value.Equals(Guid.Empty)
            || m_Value.Equals(0L);

        /// <summary>Gets a <see cref="byte"/> array that represents the identifier.</summary>
        public byte[] ToByteArray() => IsEmpty() ? Array.Empty<byte>() : behavior.ToByteArray(m_Value);

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is Id<TIdentifier> other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be Id<{typeof(TIdentifier).Name}>.", nameof(obj));
        }

        /// <inheritdoc/>
        public int CompareTo(Id<TIdentifier> other)
        {
            var isEmpty = IsEmpty();
            var otherIsEmpty = other.IsEmpty();

            if (isEmpty || otherIsEmpty)
            {
                return otherIsEmpty.CompareTo(isEmpty);
            }
            return behavior.Compare(m_Value, other.m_Value);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Id<TIdentifier> other && Equals(other);

        /// <summary>Returns true if this instance and the other identifier are equal, otherwise false.</summary>
        /// <param name="other">
        /// The <see cref = "Id{TIdentifier}" /> to compare with.
        /// </param>
        public bool Equals(Id<TIdentifier> other)
        {
            var isEmpty = IsEmpty();
            var otherIsEmpty = other.IsEmpty();

            if (isEmpty || otherIsEmpty)
            {
                return isEmpty == otherIsEmpty;
            }

            return behavior.Equals(m_Value, other.m_Value);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => IsEmpty() ? 0 : behavior.GetHashCode(m_Value);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Id<TIdentifier> left, Id<TIdentifier> right) => !(left == right);

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Id<TIdentifier> left, Id<TIdentifier> right) => left.Equals(right);

        /// <summary>Returns a <see cref="string"/> that represents the identifier for DEBUG purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => IsEmpty()
            ? $"{DebugDisplay.Empty} ({typeof(TIdentifier).Name})"
            : $"{this} ({typeof(TIdentifier).Name})";
        
        /// <summary>Returns a <see cref = "string"/> that represents the identifier.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref = "string"/> that represents the identifier.</summary>
        /// <param name="format">
        /// The format that describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref = "string"/> that represents the identifier.</summary>
        /// <param name="provider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider provider) => ToString(string.Empty, provider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the identifier.</summary>
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

            return IsEmpty() ? string.Empty : behavior.ToString(m_Value, format, formatProvider);
        }

        /// <summary>Adds the underlying property of the identifier to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the identifier.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the identifier from an <see href = "XmlReader"/>.</summary>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var xml = reader.ReadElementString();
            m_Value = Parse(xml).m_Value;
        }

        /// <summary>Writes the identifier to an <see href = "XmlWriter"/>.</summary>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>Serializes the identifier to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON node.
        /// </returns>
        public object ToJson() => IsEmpty() ? null : behavior.ToJson(m_Value);

        private TPrimitive CastToPrimitive<TPrimitive, TTo>()
        {
            if (behavior.BaseType != typeof(TPrimitive))
            {
                throw Exceptions.InvalidCast<Id<TIdentifier>, TTo>();
            }
            return m_Value is null ? default : (TPrimitive)m_Value;
        }

        /// <summary>Casts the identifier to a <see cref="string"/>.</summary>
        public static explicit operator string(Id<TIdentifier> id) => id.CastToPrimitive<string, string>();

        /// <summary>Casts the identifier to a <see cref="int"/>.</summary>
        public static explicit operator int(Id<TIdentifier> id) => id.CastToPrimitive<int, int>();

        /// <summary>Casts the identifier to a <see cref="long"/>.</summary>
        public static explicit operator long(Id<TIdentifier> id) => id.CastToPrimitive<long, long>();

        /// <summary>Casts the identifier to a <see cref="Guid"/>.</summary>
        public static explicit operator Guid(Id<TIdentifier> id) => id.CastToPrimitive<Guid, Guid>();

        /// <summary>Casts the identifier to a <see cref="Uuid"/>.</summary>
        public static explicit operator Uuid(Id<TIdentifier> id) => id.CastToPrimitive<Guid, Uuid>();

        /// <summary>Casts the <see cref="string"/> to an identifier.</summary>
        public static explicit operator Id<TIdentifier>(string id) => Create(id);

        /// <summary>Casts the <see cref="int"/> to an identifier.</summary>
        public static explicit operator Id<TIdentifier>(int id) => Create(id);

        /// <summary>Casts the <see cref="long"/> to an identifier.</summary>
        public static explicit operator Id<TIdentifier>(long id) => Create(id);

        /// <summary>Casts the <see cref="Guid"/> to an identifier.</summary>
        public static explicit operator Id<TIdentifier>(Guid id) => Create(id);

        /// <summary>Casts the <see cref="Uuid"/> to an identifier.</summary>
        public static explicit operator Id<TIdentifier>(Uuid id) => Create(id);

        /// <summary>Converts the <see cref="string"/> to <see cref="Id{TIdentifier}"/>.</summary>
        /// <param name="s">
        /// A string containing the identifier to convert.
        /// </param>
        /// <returns>
        /// The parsed identifier.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Id<TIdentifier> Parse(string s)
        {
            return TryParse(s, out Id<TIdentifier> val)
                ? val
                : throw new FormatException();
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="Id{TIdentifier}"/>.</summary>
        /// <param name="s">
        /// A string containing the identifier to convert.
        /// </param>
        /// <returns>
        /// The identifier if the string was converted successfully, otherwise default.
        /// </returns>
        public static Id<TIdentifier> TryParse(string s)
        {
            return TryParse(s, out Id<TIdentifier> val) ? val : default;
        }

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
            if (behavior.TryParse(s, out var id))
            {
                result = new Id<TIdentifier>(id);
                return true;
            }
            return false;
        }

        /// <summary>Creates the identifier from a JSON string.</summary>
        /// <param name="json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized identifier.
        /// </returns>
        public static Id<TIdentifier> FromJson(string json) => Parse(json);

        /// <summary>Deserializes the date from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized date.
        /// </returns>
        public static Id<TIdentifier> FromJson(long json) => new Id<TIdentifier>(behavior.FromJson(json));

        /// <summary>Creates the identfier for the <see cref="byte"/> array.</summary>
        /// <param name="bytes">
        /// The <see cref="byte"/> array that represents the underlying value.
        /// </param>
        public static Id<TIdentifier> FromBytes(byte[] bytes)
        {
            return bytes is null || bytes.Length == 0
                ? Empty
                : new Id<TIdentifier>(behavior.FromBytes(bytes));
        }

        /// <summary>Creates an identifier from an <see cref="object"/>.</summary>
        /// <param name="obj">
        /// The <see cref="object"/> to create an indentifier from.
        /// </param>
        /// <exception cref="InvalidCastException">
        /// if the identifier could not be created from the <see cref="object"/>.
        /// </exception>
        public static Id<TIdentifier> Create(object obj)
        {
            if (TryCreate(obj, out var id))
            {
                return id;
            }
            throw Exceptions.InvalidCast(obj?.GetType(), typeof(Id<TIdentifier>));
        }

        /// <summary>Tries to create an identifier from an <see cref="object"/>.</summary>
        /// <param name="obj">
        /// The <see cref="object"/> to create an indentifier from.
        /// </param>
        /// <param name="id">
        /// The created identifier.
        /// </param>
        /// <returns>
        /// True if the identifier could be created.
        /// </returns>
        public static bool TryCreate(object obj, out Id<TIdentifier> id)
        {
            id = default;

            if (obj is null)
            {
                return true;
            }
            if (behavior.TryCreate(obj, out var underlying))
            {
                id = new Id<TIdentifier>(underlying);
                return true;
            }
            return false;
        }

        /// <summary>Creates a new identifier.</summary>
        public static Id<TIdentifier> Next() => new Id<TIdentifier>(behavior.Next());

        /// <summary>Returns true if the value represents a valid identifier.</summary>
        /// <param name="val">
        /// The <see cref="string"/> to validate.
        /// </param>
        public static bool IsValid(string val) => !string.IsNullOrWhiteSpace(val) && TryParse(val, out _);
    }
}
