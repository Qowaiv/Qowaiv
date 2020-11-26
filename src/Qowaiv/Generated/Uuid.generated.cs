﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#define NotCultureDependent
#define NotIsUnknown
#define NotIsEmptyOrUnknown
#define NoComparisonOperators
#define NotGetHashCodeClass
namespace Qowaiv
{
    using System;

    public partial struct Uuid
    {
#if !NotField
        private Uuid(Guid value) => m_Value = value;
        /// <summary>The inner value of the UUID.</summary>
        private Guid m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  UUID is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  UUID is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  UUID is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv
{
    using System;

    public partial struct Uuid : IEquatable<Uuid>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Uuid other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other UUID are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Uuid"/> to compare with.</param>
        public bool Equals(Uuid other) => m_Value == other.m_Value;
#if !NotGetHashCodeStruct
        /// <inheritdoc/>
        public override int GetHashCode() => m_Value.GetHashCode();
#endif
#if !NotGetHashCodeClass
        /// <inheritdoc/>
        public override int GetHashCode() => m_Value is null ? 0 : m_Value.GetHashCode();
#endif
#endif
        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator !=(Uuid left, Uuid right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Uuid left, Uuid right) => left.Equals(right);
    }
}

namespace Qowaiv
{
    using System;
    using System.Collections.Generic;

    public partial struct Uuid : IComparable, IComparable<Uuid>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is Uuid other)
            {
                return CompareTo(other);
            }
            else
            {
                throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
            }
        }

#if !NotEqualsSvo
        /// <inheritdoc/>
        public int CompareTo(Uuid other) => Comparer<Guid>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Uuid l, Uuid r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Uuid l, Uuid r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Uuid l, Uuid r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Uuid l, Uuid r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Runtime.Serialization;

    public partial struct Uuid : ISerializable
    {
        /// <summary>Initializes a new instance of the UUID based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private Uuid(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (Guid)info.GetValue("Value", typeof(Guid));
        }

        /// <summary>Adds the underlying property of the UUID to the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => Guard.NotNull(info, nameof(info)).AddValue("Value", m_Value);
    }
}

namespace Qowaiv
{
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct Uuid : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the UUID.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the UUID from an <see href = "XmlReader"/>.</summary>
        /// <param name = "reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var xml = reader.ReadElementString();
#if !NotCultureDependent
            var val = Parse(xml, CultureInfo.InvariantCulture);
#else
            var val = Parse(xml);
#endif
#if !NotField
            m_Value = val.m_Value;
#endif
            OnReadXml(val);
        }

        partial void OnReadXml(Uuid other);
        /// <summary>Writes the UUID to an <see href = "XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses <see cref = "ToXmlString()"/>.
        /// </remarks>
        /// <param name = "writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer) => Guard.NotNull(writer, nameof(writer)).WriteString(ToXmlString());
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;
    using Qowaiv.Json;

    public partial struct Uuid
    {
        /// <summary>Creates the UUID from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized UUID.
        /// </returns>
        
#if !NotCultureDependent
        public static Uuid FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        public static Uuid FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Uuid : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the UUID.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the UUID.</summary>
        /// <param name = "format">
        /// The format that describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the UUID.</summary>
        /// <param name = "provider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider provider) => ToString(string.Empty, provider);
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Uuid
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Uuid"/>.</summary>
        /// <param name = "s">
        /// A string containing the UUID to convert.
        /// </param>
        /// <returns>
        /// The parsed UUID.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Uuid Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Uuid"/>.</summary>
        /// <param name = "s">
        /// A string containing the UUID to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed UUID.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Uuid Parse(string s, IFormatProvider formatProvider) => TryParse(s, formatProvider, out Uuid val) ? val : throw new FormatException(QowaivMessages.FormatExceptionUuid);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Uuid"/>.</summary>
        /// <param name = "s">
        /// A string containing the UUID to convert.
        /// </param>
        /// <returns>
        /// The UUID if the string was converted successfully, otherwise default.
        /// </returns>
        public static Uuid TryParse(string s) => TryParse(s, null, out Uuid val) ? val : default;
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Uuid"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the UUID to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Uuid result) => TryParse(s, null, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Uuid"/>.</summary>
        /// <param name="s">
        /// A string containing the UUID to convert.
        /// </param>
        /// <returns>
        /// The parsed UUID.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Uuid Parse(string s)
            => TryParse(s, out Uuid val)
            ? val
            : throw new FormatException(QowaivMessages.FormatExceptionUuid);

        /// <summary>Converts the <see cref="string"/> to <see cref="Uuid"/>.</summary>
        /// <param name="s">
        /// A string containing the UUID to convert.
        /// </param>
        /// <returns>
        /// The UUID if the string was converted successfully, otherwise default.
        /// </returns>
        public static Uuid TryParse(string s) => TryParse(s, out Uuid val) ? val : default;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Uuid
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid UUID.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, (IFormatProvider)null);
        /// <summary>Returns true if the value represents a valid UUID.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        /// <param name = "formatProvider">
        /// The <see cref = "IFormatProvider"/> to interpret the <see cref = "string "/> value with.
        /// </param>
        public static bool IsValid(string val, IFormatProvider formatProvider) => !string.IsNullOrWhiteSpace(val) && TryParse(val, formatProvider, out _);
#else
        /// <summary>Returns true if the value represents a valid UUID.</summary>
        /// <param name="val">
        /// The <see cref="string"/> to validate.
        /// </param>
        public static bool IsValid(string val)
            => !string.IsNullOrWhiteSpace(val)
            && TryParse(val, out _);
#endif
    }
}