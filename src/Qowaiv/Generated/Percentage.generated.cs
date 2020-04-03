﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#define NotIsEmpty
#define NotIsUnknown
#define NotIsEmptyOrUnknown
#define NotGetHashCodeClass
namespace Qowaiv
{
    using System;

    public partial struct Percentage
    {
#if !NotField
        private Percentage(decimal value) => m_Value = value;
        /// <summary>The inner value of the percentage.</summary>
        private decimal m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  percentage is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  percentage is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  percentage is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv
{
    using System;

    public partial struct Percentage : IEquatable<Percentage>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Percentage other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other percentage are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Percentage"/> to compare with.</param>
        public bool Equals(Percentage other) => m_Value == other.m_Value;
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
        public static bool operator !=(Percentage left, Percentage right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Percentage left, Percentage right) => left.Equals(right);
    }
}

namespace Qowaiv
{
    using System;
    using System.Collections.Generic;

    public partial struct Percentage : IComparable, IComparable<Percentage>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is Percentage other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

#if !NotEqualsSvo
        /// <inheritdoc/>
        public int CompareTo(Percentage other) => Comparer<decimal>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Percentage l, Percentage r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Percentage l, Percentage r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Percentage l, Percentage r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Percentage l, Percentage r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Runtime.Serialization;

    public partial struct Percentage : ISerializable
    {
        /// <summary>Initializes a new instance of the percentage based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private Percentage(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (decimal)info.GetValue("Value", typeof(decimal));
        }

        /// <summary>Adds the underlying property of the percentage to the serialization info.</summary>
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

    public partial struct Percentage : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the percentage.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the percentage from an <see href = "XmlReader"/>.</summary>
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

        partial void OnReadXml(Percentage other);
        /// <summary>Writes the percentage to an <see href = "XmlWriter"/>.</summary>
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

    public partial struct Percentage
    {
        /// <summary>Creates the percentage from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized percentage.
        /// </returns>
        
#if !NotCultureDependent
        public static Percentage FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        public static Percentage FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial struct Percentage : IConvertible
    {
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convertable.ToType(conversionType, provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convertable.ToBoolean(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        byte IConvertible.ToByte(IFormatProvider provider) => Convertable.ToByte(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        char IConvertible.ToChar(IFormatProvider provider) => Convertable.ToChar(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convertable.ToDateTime(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convertable.ToDecimal(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        double IConvertible.ToDouble(IFormatProvider provider) => Convertable.ToDouble(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        short IConvertible.ToInt16(IFormatProvider provider) => Convertable.ToInt16(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        int IConvertible.ToInt32(IFormatProvider provider) => Convertable.ToInt32(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        long IConvertible.ToInt64(IFormatProvider provider) => Convertable.ToInt64(provider);
        /// <inheritdoc/>
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convertable.ToSByte(provider);
        /// <inheritdoc/>
        float IConvertible.ToSingle(IFormatProvider provider) => Convertable.ToSingle(provider);
        /// <inheritdoc/>
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convertable.ToUInt16(provider);
        /// <inheritdoc/>
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convertable.ToUInt32(provider);
        /// <inheritdoc/>
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convertable.ToUInt64(provider);
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Percentage
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Percentage"/>.</summary>
        /// <param name = "s">
        /// A string containing the percentage to convert.
        /// </param>
        /// <returns>
        /// The parsed percentage.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Percentage Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Percentage"/>.</summary>
        /// <param name = "s">
        /// A string containing the percentage to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed percentage.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Percentage Parse(string s, IFormatProvider formatProvider)
        {
            return TryParse(s, formatProvider, out Percentage val) ? val : throw new FormatException(QowaivMessages.FormatExceptionPercentage);
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Percentage"/>.</summary>
        /// <param name = "s">
        /// A string containing the percentage to convert.
        /// </param>
        /// <returns>
        /// The percentage if the string was converted successfully, otherwise default.
        /// </returns>
        public static Percentage TryParse(string s)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out Percentage val) ? val : default;
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Percentage"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the percentage to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Percentage result) => TryParse(s, CultureInfo.CurrentCulture, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Percentage"/>.</summary>
        /// <param name="s">
        /// A string containing the percentage to convert.
        /// </param>
        /// <returns>
        /// The parsed percentage.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Percentage Parse(string s)
        {
            return TryParse(s, out Percentage val)
                ? val
                : throw new FormatException(QowaivMessages.FormatExceptionPercentage);
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="Percentage"/>.</summary>
        /// <param name="s">
        /// A string containing the percentage to convert.
        /// </param>
        /// <returns>
        /// The percentage if the string was converted successfully, otherwise default.
        /// </returns>
        public static Percentage TryParse(string s)
        {
            return TryParse(s, out Percentage val) ? val : default;
        }
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Percentage
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid percentage.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);
        /// <summary>Returns true if the value represents a valid percentage.</summary>
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
        /// <summary>Returns true if the value represents a valid percentage.</summary>
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