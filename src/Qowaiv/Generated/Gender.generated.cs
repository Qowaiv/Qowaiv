﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#define NoComparisonOperators
#define NotGetHashCodeClass
namespace Qowaiv
{
    using System;

    public partial struct Gender
    {
#if !NotField
        private Gender(byte value) => m_Value = value;
        /// <summary>The inner value of the gender.</summary>
        private byte m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  gender is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  gender is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  gender is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv
{
    using System;

    public partial struct Gender : IEquatable<Gender>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Gender other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other gender are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Gender"/> to compare with.</param>
        public bool Equals(Gender other) => m_Value == other.m_Value;
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
        public static bool operator !=(Gender left, Gender right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Gender left, Gender right) => left.Equals(right);
    }
}

namespace Qowaiv
{
    using System;
    using System.Collections.Generic;

    public partial struct Gender : IComparable, IComparable<Gender>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is Gender other)
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
        public int CompareTo(Gender other) => Comparer<byte>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Gender l, Gender r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Gender l, Gender r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Gender l, Gender r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Gender l, Gender r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Runtime.Serialization;

    public partial struct Gender : ISerializable
    {
        /// <summary>Initializes a new instance of the gender based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private Gender(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (byte)info.GetValue("Value", typeof(byte));
        }

        /// <summary>Adds the underlying property of the gender to the serialization info.</summary>
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

    public partial struct Gender : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the gender.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the gender from an <see href = "XmlReader"/>.</summary>
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

        partial void OnReadXml(Gender other);
        /// <summary>Writes the gender to an <see href = "XmlWriter"/>.</summary>
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

    public partial struct Gender
    {
        /// <summary>Creates the gender from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized gender.
        /// </returns>
        
#if !NotCultureDependent
        public static Gender FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        public static Gender FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Gender : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the gender.</summary>
        public override string ToString() => ToString((IFormatProvider)null);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the gender.</summary>
        /// <param name = "format">
        /// The format that describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, null);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the gender.</summary>
        /// <param name = "provider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider provider) => ToString(null, provider);
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Gender
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Gender"/>.</summary>
        /// <param name = "s">
        /// A string containing the gender to convert.
        /// </param>
        /// <returns>
        /// The parsed gender.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Gender Parse(string s) => Parse(s, null);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Gender"/>.</summary>
        /// <param name = "s">
        /// A string containing the gender to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed gender.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Gender Parse(string s, IFormatProvider formatProvider) => TryParse(s, formatProvider, out Gender val) ? val : throw new FormatException(QowaivMessages.FormatExceptionGender);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Gender"/>.</summary>
        /// <param name = "s">
        /// A string containing the gender to convert.
        /// </param>
        /// <returns>
        /// The gender if the string was converted successfully, otherwise default.
        /// </returns>
        public static Gender TryParse(string s) => TryParse(s, null, out Gender val) ? val : default;
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Gender"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the gender to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Gender result) => TryParse(s, null, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Gender"/>.</summary>
        /// <param name="s">
        /// A string containing the gender to convert.
        /// </param>
        /// <returns>
        /// The parsed gender.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Gender Parse(string s)
            => TryParse(s, out Gender val)
            ? val
            : throw new FormatException(QowaivMessages.FormatExceptionGender);

        /// <summary>Converts the <see cref="string"/> to <see cref="Gender"/>.</summary>
        /// <param name="s">
        /// A string containing the gender to convert.
        /// </param>
        /// <returns>
        /// The gender if the string was converted successfully, otherwise default.
        /// </returns>
        public static Gender TryParse(string s) => TryParse(s, out Gender val) ? val : default;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Gender
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid gender.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, (IFormatProvider)null);
        /// <summary>Returns true if the value represents a valid gender.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        /// <param name = "formatProvider">
        /// The <see cref = "IFormatProvider"/> to interpret the <see cref = "string "/> value with.
        /// </param>
        public static bool IsValid(string val, IFormatProvider formatProvider) => !string.IsNullOrWhiteSpace(val) && TryParse(val, formatProvider, out _);
#else
        /// <summary>Returns true if the value represents a valid gender.</summary>
        /// <param name="val">
        /// The <see cref="string"/> to validate.
        /// </param>
        public static bool IsValid(string val)
            => !string.IsNullOrWhiteSpace(val)
            && TryParse(val, out _);
#endif
    }
}