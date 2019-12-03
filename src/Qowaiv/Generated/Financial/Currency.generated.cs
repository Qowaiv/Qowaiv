﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#define NoComparisonOperators
#define NotGetHashCodeStruct
namespace Qowaiv.Financial
{
    using System;

    public partial struct Currency
    {
#if !NotField
        private Currency(string value) => m_Value = value;
        /// <summary>The inner value of the currency.</summary>
        private string m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  currency is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  currency is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  currency is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;

    public partial struct Currency : IEquatable<Currency>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Currency other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other currency are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Currency"/> to compare with.</param>
        public bool Equals(Currency other) => m_Value == other.m_Value;
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
        public static bool operator !=(Currency left, Currency right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Currency left, Currency right) => left.Equals(right);
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Collections.Generic;

    public partial struct Currency : IComparable, IComparable<Currency>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is Currency other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

#if !NotEqualsSvo
        /// <inheritdoc/>
        public int CompareTo(Currency other) => Comparer<string>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Currency l, Currency r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Currency l, Currency r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Currency l, Currency r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Currency l, Currency r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Runtime.Serialization;

    public partial struct Currency : ISerializable
    {
        /// <summary>Initializes a new instance of the currency based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private Currency(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (string)info.GetValue("Value", typeof(string));
        }

        /// <summary>Adds the underlying property of the currency to the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }
    }
}

namespace Qowaiv.Financial
{
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct Currency : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the currency.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the currency from an <see href = "XmlReader"/>.</summary>
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
        }

        /// <summary>Writes the currency to an <see href = "XmlWriter"/>.</summary>
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

namespace Qowaiv.Financial
{
    using System;
    using System.Globalization;

    public partial struct Currency : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the currency.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the currency.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the currency.</summary>
        /// <param name = "formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString(string.Empty, formatProvider);
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Globalization;

    public partial struct Currency
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Currency"/>.</summary>
        /// <param name = "s">
        /// A string containing the currency to convert.
        /// </param>
        /// <returns>
        /// The parsed currency.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Currency Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Currency"/>.</summary>
        /// <param name = "s">
        /// A string containing the currency to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed currency.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Currency Parse(string s, IFormatProvider formatProvider)
        {
            return TryParse(s, formatProvider, out Currency val) ? val : throw new FormatException(QowaivMessages.FormatExceptionCurrency);
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Currency"/>.</summary>
        /// <param name = "s">
        /// A string containing the currency to convert.
        /// </param>
        /// <returns>
        /// The currency if the string was converted successfully, otherwise default.
        /// </returns>
        public static Currency TryParse(string s)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out Currency val) ? val : default;
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Currency"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the currency to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Currency result) => TryParse(s, CultureInfo.CurrentCulture, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Currency"/>.</summary>
        /// <param name="s">
        /// A string containing the currency to convert.
        /// </param>
        /// <returns>
        /// The parsed currency.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Currency Parse(string s)
        {
            return TryParse(s, out Currency val)
                ? val
                : throw new FormatException(QowaivMessages.FormatExceptionCurrency);
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="Currency"/>.</summary>
        /// <param name="s">
        /// A string containing the currency to convert.
        /// </param>
        /// <returns>
        /// The currency if the string was converted successfully, otherwise default.
        /// </returns>
        public static Currency TryParse(string s)
        {
            return TryParse(s, out Currency val) ? val : default;
        }
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Globalization;

    public partial struct Currency
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid currency.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);
        /// <summary>Returns true if the value represents a valid currency.</summary>
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
        /// <summary>Returns true if the value represents a valid currency.</summary>
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