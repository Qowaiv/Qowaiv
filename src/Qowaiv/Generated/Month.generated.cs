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

    public partial struct Month
    {
#if !NotField
        private Month(byte value) => m_Value = value;
        /// <summary>The inner value of the month.</summary>
        private byte m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  month is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  month is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  month is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv
{
    using System;

    public partial struct Month : IEquatable<Month>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Month other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other month are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Month"/> to compare with.</param>
        public bool Equals(Month other) => m_Value == other.m_Value;
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
        public static bool operator !=(Month left, Month right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Month left, Month right) => left.Equals(right);
    }
}

namespace Qowaiv
{
    using System;
    using System.Collections.Generic;

    public partial struct Month : IComparable, IComparable<Month>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is Month other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

#if !NotEqualsSvo
        /// <inheritdoc/>
        public int CompareTo(Month other) => Comparer<byte>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Month l, Month r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Month l, Month r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Month l, Month r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Month l, Month r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Runtime.Serialization;

    public partial struct Month : ISerializable
    {
        /// <summary>Initializes a new instance of the month based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private Month(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (byte)info.GetValue("Value", typeof(byte));
        }

        /// <summary>Adds the underlying property of the month to the serialization info.</summary>
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

    public partial struct Month : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the month.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the month from an <see href = "XmlReader"/>.</summary>
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

        partial void OnReadXml(Month other);
        /// <summary>Writes the month to an <see href = "XmlWriter"/>.</summary>
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
    using Qowaiv.Json;

    public partial struct Month : IJsonSerializable
    {
        /// <inheritdoc/>
        [Obsolete("Use FromJson(object) instead.")]
        void IJsonSerializable.FromJson() => FromJson(null);
        /// <inheritdoc/>
        [Obsolete("Use FromJson(object) instead.")]
        void IJsonSerializable.FromJson(string jsonString) => FromJson(jsonString);
        /// <inheritdoc/>
        [Obsolete("Use FromJson(object) instead.")]
        void IJsonSerializable.FromJson(long jsonInteger) => FromJson(jsonInteger);
        /// <inheritdoc/>
        [Obsolete("Use FromJson(object) instead.")]
        void IJsonSerializable.FromJson(double jsonNumber) => FromJson(jsonNumber);
        /// <inheritdoc/>
        [Obsolete("Use FromJson(object) instead.")]
        void IJsonSerializable.FromJson(DateTime jsonDate) => FromJson(jsonDate);
        /// <inheritdoc/>
        void IJsonSerializable.FromJson(object json) => FromJson(json);
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Month : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the month.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the month.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the month.</summary>
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

    public partial struct Month
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Month"/>.</summary>
        /// <param name = "s">
        /// A string containing the month to convert.
        /// </param>
        /// <returns>
        /// The parsed month.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Month Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Month"/>.</summary>
        /// <param name = "s">
        /// A string containing the month to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed month.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Month Parse(string s, IFormatProvider formatProvider)
        {
            return TryParse(s, formatProvider, out Month val) ? val : throw new FormatException(QowaivMessages.FormatExceptionMonth);
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Month"/>.</summary>
        /// <param name = "s">
        /// A string containing the month to convert.
        /// </param>
        /// <returns>
        /// The month if the string was converted successfully, otherwise default.
        /// </returns>
        public static Month TryParse(string s)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out Month val) ? val : default;
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Month"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the month to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Month result) => TryParse(s, CultureInfo.CurrentCulture, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Month"/>.</summary>
        /// <param name="s">
        /// A string containing the month to convert.
        /// </param>
        /// <returns>
        /// The parsed month.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Month Parse(string s)
        {
            return TryParse(s, out Month val)
                ? val
                : throw new FormatException(QowaivMessages.FormatExceptionMonth);
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="Month"/>.</summary>
        /// <param name="s">
        /// A string containing the month to convert.
        /// </param>
        /// <returns>
        /// The month if the string was converted successfully, otherwise default.
        /// </returns>
        public static Month TryParse(string s)
        {
            return TryParse(s, out Month val) ? val : default;
        }
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Month
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid month.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);
        /// <summary>Returns true if the value represents a valid month.</summary>
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
        /// <summary>Returns true if the value represents a valid month.</summary>
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