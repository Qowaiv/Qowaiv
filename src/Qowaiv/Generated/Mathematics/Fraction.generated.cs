﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#define NotField
#define NotIsEmpty
#define NotIsUnknown
#define NotIsEmptyOrUnknown
#define NotEqualsSvo
#define NotGetHashCodeClass
namespace Qowaiv.Mathematics
{
    using System;

    public partial struct Fraction
    {
#if !NotField
        private Fraction(long value) => m_Value = value;
        /// <summary>The inner value of the fraction.</summary>
        private long m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  fraction is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  fraction is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  fraction is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv.Mathematics
{
    using System;

    public partial struct Fraction : IEquatable<Fraction>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Fraction other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other fraction are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Fraction"/> to compare with.</param>
        public bool Equals(Fraction other) => m_Value == other.m_Value;
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
        public static bool operator !=(Fraction left, Fraction right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Fraction left, Fraction right) => left.Equals(right);
    }
}

namespace Qowaiv.Mathematics
{
    using System;
    using System.Collections.Generic;

    public partial struct Fraction : IComparable, IComparable<Fraction>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is Fraction other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

#if !NotEqualsSvo
        /// <inheritdoc/>
        public int CompareTo(Fraction other) => Comparer<long>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Fraction l, Fraction r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Fraction l, Fraction r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Fraction l, Fraction r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Fraction l, Fraction r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv.Mathematics
{
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct Fraction : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the fraction.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the fraction from an <see href = "XmlReader"/>.</summary>
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

        partial void OnReadXml(Fraction other);
        /// <summary>Writes the fraction to an <see href = "XmlWriter"/>.</summary>
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

namespace Qowaiv.Mathematics
{
    using System;
    using System.Globalization;
    using Qowaiv.Json;

    public partial struct Fraction
    {
        /// <summary>Creates the fraction from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized fraction.
        /// </returns>
        
#if !NotCultureDependent
        public static Fraction FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        public static Fraction FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv.Mathematics
{
    using System;
    using System.Globalization;

    public partial struct Fraction : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the fraction.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the fraction.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the fraction.</summary>
        /// <param name = "provider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider provider) => ToString(string.Empty, provider);
    }
}

namespace Qowaiv.Mathematics
{
    using System;
    using System.Collections.Generic;

    public partial struct Fraction : IConvertible
    {
        /// <inheritdoc/>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convertable.ToType(conversionType, provider);
        /// <inheritdoc/>
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convertable.ToBoolean(provider);
        /// <inheritdoc/>
        byte IConvertible.ToByte(IFormatProvider provider) => Convertable.ToByte(provider);
        /// <inheritdoc/>
        char IConvertible.ToChar(IFormatProvider provider) => Convertable.ToChar(provider);
        /// <inheritdoc/>
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convertable.ToDateTime(provider);
        /// <inheritdoc/>
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convertable.ToDecimal(provider);
        /// <inheritdoc/>
        double IConvertible.ToDouble(IFormatProvider provider) => Convertable.ToDouble(provider);
        /// <inheritdoc/>
        short IConvertible.ToInt16(IFormatProvider provider) => Convertable.ToInt16(provider);
        /// <inheritdoc/>
        int IConvertible.ToInt32(IFormatProvider provider) => Convertable.ToInt32(provider);
        /// <inheritdoc/>
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

namespace Qowaiv.Mathematics
{
    using System;
    using System.Globalization;

    public partial struct Fraction
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Fraction"/>.</summary>
        /// <param name = "s">
        /// A string containing the fraction to convert.
        /// </param>
        /// <returns>
        /// The parsed fraction.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Fraction Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Fraction"/>.</summary>
        /// <param name = "s">
        /// A string containing the fraction to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed fraction.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Fraction Parse(string s, IFormatProvider formatProvider)
        {
            return TryParse(s, formatProvider, out Fraction val) ? val : throw new FormatException(QowaivMessages.FormatExceptionFraction);
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Fraction"/>.</summary>
        /// <param name = "s">
        /// A string containing the fraction to convert.
        /// </param>
        /// <returns>
        /// The fraction if the string was converted successfully, otherwise default.
        /// </returns>
        public static Fraction TryParse(string s)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out Fraction val) ? val : default;
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Fraction"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the fraction to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Fraction result) => TryParse(s, CultureInfo.CurrentCulture, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Fraction"/>.</summary>
        /// <param name="s">
        /// A string containing the fraction to convert.
        /// </param>
        /// <returns>
        /// The parsed fraction.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Fraction Parse(string s)
        {
            return TryParse(s, out Fraction val)
                ? val
                : throw new FormatException(QowaivMessages.FormatExceptionFraction);
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="Fraction"/>.</summary>
        /// <param name="s">
        /// A string containing the fraction to convert.
        /// </param>
        /// <returns>
        /// The fraction if the string was converted successfully, otherwise default.
        /// </returns>
        public static Fraction TryParse(string s)
        {
            return TryParse(s, out Fraction val) ? val : default;
        }
#endif
    }
}

namespace Qowaiv.Mathematics
{
    using System;
    using System.Globalization;

    public partial struct Fraction
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid fraction.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);
        /// <summary>Returns true if the value represents a valid fraction.</summary>
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
        /// <summary>Returns true if the value represents a valid fraction.</summary>
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