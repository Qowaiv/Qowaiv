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
namespace Qowaiv.Globalization
{
    using System;

    public partial struct Country
    {
#if !NotField
        private Country(string value) => m_Value = value;
        /// <summary>The inner value of the country.</summary>
        private string m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  country is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  country is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  country is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv.Globalization
{
    using System;

    public partial struct Country : IEquatable<Country>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Country other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other country are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Country"/> to compare with.</param>
        public bool Equals(Country other) => m_Value == other.m_Value;
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
        public static bool operator !=(Country left, Country right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Country left, Country right) => left.Equals(right);
    }
}

namespace Qowaiv.Globalization
{
    using System;
    using System.Collections.Generic;

    public partial struct Country : IComparable, IComparable<Country>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is Country other)
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
        public int CompareTo(Country other) => Comparer<string>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Country l, Country r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Country l, Country r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Country l, Country r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Country l, Country r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv.Globalization
{
    using System;
    using System.Runtime.Serialization;

    public partial struct Country : ISerializable
    {
        /// <summary>Initializes a new instance of the country based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private Country(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (string)info.GetValue("Value", typeof(string));
        }

        /// <summary>Adds the underlying property of the country to the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => Guard.NotNull(info, nameof(info)).AddValue("Value", m_Value);
    }
}

namespace Qowaiv.Globalization
{
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct Country : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the country.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the country from an <see href = "XmlReader"/>.</summary>
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

        partial void OnReadXml(Country other);
        /// <summary>Writes the country to an <see href = "XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses <see cref = "ToXmlString()"/>.
        /// </remarks>
        /// <param name = "writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer) => Guard.NotNull(writer, nameof(writer)).WriteString(ToXmlString());
    }
}

namespace Qowaiv.Globalization
{
    using System;
    using System.Globalization;
    using Qowaiv.Json;

    public partial struct Country
    {
        /// <summary>Creates the country from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized country.
        /// </returns>
        
#if !NotCultureDependent
        public static Country FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        public static Country FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv.Globalization
{
    using System;
    using System.Globalization;

    public partial struct Country : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the country.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the country.</summary>
        /// <param name = "format">
        /// The format that describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the country.</summary>
        /// <param name = "provider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider provider) => ToString(string.Empty, provider);
    }
}

namespace Qowaiv.Globalization
{
    using System;
    using System.Globalization;

    public partial struct Country
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Country"/>.</summary>
        /// <param name = "s">
        /// A string containing the country to convert.
        /// </param>
        /// <returns>
        /// The parsed country.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Country Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Country"/>.</summary>
        /// <param name = "s">
        /// A string containing the country to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed country.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Country Parse(string s, IFormatProvider formatProvider) => TryParse(s, formatProvider, out Country val) ? val : throw new FormatException(QowaivMessages.FormatExceptionCountry);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Country"/>.</summary>
        /// <param name = "s">
        /// A string containing the country to convert.
        /// </param>
        /// <returns>
        /// The country if the string was converted successfully, otherwise default.
        /// </returns>
        public static Country TryParse(string s) => TryParse(s, null, out Country val) ? val : default;
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Country"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the country to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Country result) => TryParse(s, null, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Country"/>.</summary>
        /// <param name="s">
        /// A string containing the country to convert.
        /// </param>
        /// <returns>
        /// The parsed country.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Country Parse(string s)
            => TryParse(s, out Country val)
            ? val
            : throw new FormatException(QowaivMessages.FormatExceptionCountry);

        /// <summary>Converts the <see cref="string"/> to <see cref="Country"/>.</summary>
        /// <param name="s">
        /// A string containing the country to convert.
        /// </param>
        /// <returns>
        /// The country if the string was converted successfully, otherwise default.
        /// </returns>
        public static Country TryParse(string s) => TryParse(s, out Country val) ? val : default;
#endif
    }
}

namespace Qowaiv.Globalization
{
    using System;
    using System.Globalization;

    public partial struct Country
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid country.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, (IFormatProvider)null);
        /// <summary>Returns true if the value represents a valid country.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        /// <param name = "formatProvider">
        /// The <see cref = "IFormatProvider"/> to interpret the <see cref = "string "/> value with.
        /// </param>
        public static bool IsValid(string val, IFormatProvider formatProvider) => !string.IsNullOrWhiteSpace(val) && TryParse(val, formatProvider, out _);
#else
        /// <summary>Returns true if the value represents a valid country.</summary>
        /// <param name="val">
        /// The <see cref="string"/> to validate.
        /// </param>
        public static bool IsValid(string val)
            => !string.IsNullOrWhiteSpace(val)
            && TryParse(val, out _);
#endif
    }
}