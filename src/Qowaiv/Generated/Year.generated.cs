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

    public partial struct Year
    {
#if !NotField
        private Year(short value) => m_Value = value;
        /// <summary>The inner value of the year.</summary>
        private short m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  year is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  year is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  year is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv
{
    using System;

    public partial struct Year : IEquatable<Year>
    {
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other year are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Year"/> to compare with.</param>
        public bool Equals(Year other) => m_Value == other.m_Value;
#endif
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Year other && Equals(other);
#if !NotGetHashCodeStruct
        /// <inheritdoc/>
        public override int GetHashCode() => m_Value.GetHashCode();
#endif
#if !NotGetHashCodeClass
        /// <inheritdoc/>
        public override int GetHashCode() => m_Value is null ? 0 : m_Value.GetHashCode();
#endif
        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator !=(Year left, Year right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Year left, Year right) => left.Equals(right);
    }
}

namespace Qowaiv
{
    using System;
    using System.Collections.Generic;

    public partial struct Year : IComparable, IComparable<Year>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is Year other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

        /// <inheritdoc/>
        public int CompareTo(Year other) => Comparer<short>.Default.Compare(m_Value, other.m_Value);
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Year l, Year r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Year l, Year r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Year l, Year r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Year l, Year r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Runtime.Serialization;

    public partial struct Year : ISerializable
    {
        /// <summary>Initializes a new instance of the year based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private Year(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (short)info.GetValue("Value", typeof(short));
        }

        /// <summary>Adds the underlying property of the year to the serialization info.</summary>
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
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct Year : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the year.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the year from an <see href = "XmlReader"/>.</summary>
        /// <remarks>
        /// Uses <see cref = "FromXml(string)"/>.
        /// </remarks>
        /// <param name = "reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = FromXml(s);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the year to an <see href = "XmlWriter"/>.</summary>
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

    public partial struct Year : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the year.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the year.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the year.</summary>
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

    public partial struct Year
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Year"/>.</summary>
        /// <param name = "s">
        /// A string containing the year to convert.
        /// </param>
        /// <returns>
        /// The parsed year.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Year Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Year"/>.</summary>
        /// <param name = "s">
        /// A string containing the year to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed year.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Year Parse(string s, IFormatProvider formatProvider)
        {
            return TryParse(s, formatProvider, out Year val) ? val : throw new FormatException(FormatExceptionMessage);
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Year"/>.</summary>
        /// <param name = "s">
        /// A string containing the year to convert.
        /// </param>
        /// <returns>
        /// The year if the string was converted successfully, otherwise default.
        /// </returns>
        public static Year TryParse(string s)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out Year val) ? val : default;
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Year"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the year to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Year result) => TryParse(s, CultureInfo.CurrentCulture, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Year"/>.</summary>
        /// <param name="s">
        /// A string containing the year to convert.
        /// </param>
        /// <returns>
        /// The parsed year.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Year Parse(string s)
        {
            return TryParse(s, out Year val)
                ? val
                : throw new FormatException(FormatExceptionMessage);
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="Year"/>.</summary>
        /// <param name="s">
        /// A string containing the year to convert.
        /// </param>
        /// <returns>
        /// The year if the string was converted successfully, otherwise default.
        /// </returns>
        public static Year TryParse(string s)
        {
            return TryParse(s, out Year val) ? val : default;
        }
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Globalization;

    public partial struct Year
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid year.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);
        /// <summary>Returns true if the value represents a valid year.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        /// <param name = "formatProvider">
        /// The <see cref = "IFormatProvider"/> to interpret the <see cref = "string "/> value with.
        /// </param>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return !string.IsNullOrWhiteSpace(val) 
#if !NotIsUnknown
            && !Qowaiv.Unknown.IsUnknown(val, formatProvider as CultureInfo) 
#endif
            && TryParse(val, formatProvider, out _);
        }
#else
        /// <summary>Returns true if the value represents a valid year.</summary>
        /// <param name="val">
        /// The <see cref="string"/> to validate.
        /// </param>
        public static bool IsValid(string val)
        {
            return !string.IsNullOrWhiteSpace(val)
#if !NotIsUnknown
                && !Qowaiv.Unknown.IsUnknown(val, CultureInfo.InvariantCulture)
#endif
                && TryParse(val, out _);
        }
#endif
    }
}