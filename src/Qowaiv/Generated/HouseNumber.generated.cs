﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#define NotGetHashCodeClass
namespace Qowaiv
{
    using System;

    public partial struct HouseNumber
    {
#if !NotField
        private HouseNumber(int value) => m_Value = value;
        /// <summary>The inner value of the house number.</summary>
        private int m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  house number is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  house number is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  house number is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv
{
    using System;

    public partial struct HouseNumber : IEquatable<HouseNumber>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is HouseNumber other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other house number are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "HouseNumber"/> to compare with.</param>
        public bool Equals(HouseNumber other) => m_Value == other.m_Value;
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
        public static bool operator !=(HouseNumber left, HouseNumber right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(HouseNumber left, HouseNumber right) => left.Equals(right);
    }
}

namespace Qowaiv
{
    using System;
    using System.Collections.Generic;

    public partial struct HouseNumber : IComparable, IComparable<HouseNumber>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is HouseNumber other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

#if !NotEqualsSvo
        /// <inheritdoc/>
        public int CompareTo(HouseNumber other) => Comparer<int>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(HouseNumber l, HouseNumber r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(HouseNumber l, HouseNumber r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(HouseNumber l, HouseNumber r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(HouseNumber l, HouseNumber r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Runtime.Serialization;

    public partial struct HouseNumber : ISerializable
    {
        /// <summary>Initializes a new instance of the house number based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private HouseNumber(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (int)info.GetValue("Value", typeof(int));
        }

        /// <summary>Adds the underlying property of the house number to the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }
    }
}

namespace Qowaiv
{
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct HouseNumber : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the house number.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the house number from an <see href = "XmlReader"/>.</summary>
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
            m_Value = val.m_Value;
            OnReadXml(val);
        }

        partial void OnReadXml(HouseNumber other);
        /// <summary>Writes the house number to an <see href = "XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses <see cref = "ToXmlString()"/>.
        /// </remarks>
        /// <param name = "writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToXmlString());
        }
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;
    using Qowaiv.Json;

    public partial struct HouseNumber
    {
        /// <summary>Creates the house number from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized house number.
        /// </returns>
        
#if !NotCultureDependent
        public static HouseNumber FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        public static HouseNumber FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct HouseNumber : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the house number.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the house number.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the house number.</summary>
        /// <param name = "formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString(string.Empty, formatProvider);
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct HouseNumber
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "HouseNumber"/>.</summary>
        /// <param name = "s">
        /// A string containing the house number to convert.
        /// </param>
        /// <returns>
        /// The parsed house number.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static HouseNumber Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "HouseNumber"/>.</summary>
        /// <param name = "s">
        /// A string containing the house number to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed house number.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static HouseNumber Parse(string s, IFormatProvider formatProvider)
        {
            return TryParse(s, formatProvider, out HouseNumber val) ? val : throw new FormatException(QowaivMessages.FormatExceptionHouseNumber);
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "HouseNumber"/>.</summary>
        /// <param name = "s">
        /// A string containing the house number to convert.
        /// </param>
        /// <returns>
        /// The house number if the string was converted successfully, otherwise default.
        /// </returns>
        public static HouseNumber TryParse(string s)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out HouseNumber val) ? val : default;
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "HouseNumber"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the house number to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out HouseNumber result) => TryParse(s, CultureInfo.CurrentCulture, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="HouseNumber"/>.</summary>
        /// <param name="s">
        /// A string containing the house number to convert.
        /// </param>
        /// <returns>
        /// The parsed house number.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static HouseNumber Parse(string s)
        {
            return TryParse(s, out HouseNumber val)
                ? val
                : throw new FormatException(QowaivMessages.FormatExceptionHouseNumber);
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="HouseNumber"/>.</summary>
        /// <param name="s">
        /// A string containing the house number to convert.
        /// </param>
        /// <returns>
        /// The house number if the string was converted successfully, otherwise default.
        /// </returns>
        public static HouseNumber TryParse(string s)
        {
            return TryParse(s, out HouseNumber val) ? val : default;
        }
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct HouseNumber
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid house number.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);
        /// <summary>Returns true if the value represents a valid house number.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        /// <param name = "formatProvider">
        /// The <see cref = "IFormatProvider"/> to interpret the <see cref = "string "/> value with.
        /// </param>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return !string.IsNullOrWhiteSpace(val) && TryParse(val, formatProvider, out _);
        }
#else
        /// <summary>Returns true if the value represents a valid house number.</summary>
        /// <param name="val">
        /// The <see cref="string"/> to validate.
        /// </param>
        public static bool IsValid(string val)
        {
            return !string.IsNullOrWhiteSpace(val) && TryParse(val, out _);
        }
#endif
    }
}