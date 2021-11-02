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
namespace Qowaiv
{
    using System;
    using System.Diagnostics.Contracts;

    public partial struct MonthSpan
    {
#if !NotField
        private MonthSpan(int value) => m_Value = value;
        /// <summary>The inner value of the month span.</summary>
        private int m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  month span is empty, otherwise false.</summary>
        [Pure]
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  month span is unknown, otherwise false.</summary>
        [Pure]
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  month span is empty or unknown, otherwise false.</summary>
        [Pure]
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Diagnostics.Contracts;
    using Qowaiv.Hashing;

    public partial struct MonthSpan : IEquatable<MonthSpan>
    {
        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) => obj is MonthSpan other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other month span are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "MonthSpan"/> to compare with.</param>
        [Pure]
        public bool Equals(MonthSpan other) => m_Value == other.m_Value;
#if !NotGetHashCode
        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => Hash.Code(m_Value);
#endif
#endif
        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator !=(MonthSpan left, MonthSpan right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(MonthSpan left, MonthSpan right) => left.Equals(right);
    }
}

namespace Qowaiv
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public partial struct MonthSpan : IComparable, IComparable<MonthSpan>
    {
        /// <inheritdoc/>
        [Pure]
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is MonthSpan other)
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
        [Pure]
        public int CompareTo(MonthSpan other) => Comparer<int>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(MonthSpan l, MonthSpan r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(MonthSpan l, MonthSpan r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(MonthSpan l, MonthSpan r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(MonthSpan l, MonthSpan r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;

    public partial struct MonthSpan : ISerializable
    {
        /// <summary>Initializes a new instance of the month span based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private MonthSpan(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (int)info.GetValue("Value", typeof(int));
        }

        /// <summary>Adds the underlying property of the month span to the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => Guard.NotNull(info, nameof(info)).AddValue("Value", m_Value);
    }
}

namespace Qowaiv
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct MonthSpan : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the month span.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        [Pure]
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the month span from an <see href = "XmlReader"/>.</summary>
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

        partial void OnReadXml(MonthSpan other);
        /// <summary>Writes the month span to an <see href = "XmlWriter"/>.</summary>
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
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Qowaiv.Json;

    public partial struct MonthSpan
    {
        /// <summary>Creates the month span from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized month span.
        /// </returns>
        
#if !NotCultureDependent
        [Pure]
        public static MonthSpan FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        [Pure]
        public static MonthSpan FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public partial struct MonthSpan : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the month span.</summary>
        [Pure]
        public override string ToString() => ToString((IFormatProvider)null);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the month span.</summary>
        /// <param name = "format">
        /// The format that describes the formatting.
        /// </param>
        [Pure]
        public string ToString(string format) => ToString(format, null);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the month span.</summary>
        /// <param name = "provider">
        /// The format provider.
        /// </param>
        [Pure]
        public string ToString(IFormatProvider provider) => ToString(null, provider);
    }
}

namespace Qowaiv
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public partial struct MonthSpan
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "MonthSpan"/>.</summary>
        /// <param name = "s">
        /// A string containing the month span to convert.
        /// </param>
        /// <returns>
        /// The parsed month span.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        [Pure]
        public static MonthSpan Parse(string s) => Parse(s, null);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "MonthSpan"/>.</summary>
        /// <param name = "s">
        /// A string containing the month span to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed month span.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        [Pure]
        public static MonthSpan Parse(string s, IFormatProvider formatProvider) => TryParse(s, formatProvider, out MonthSpan val) ? val : throw new FormatException(QowaivMessages.FormatExceptionMonthSpan);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "MonthSpan"/>.</summary>
        /// <param name = "s">
        /// A string containing the month span to convert.
        /// </param>
        /// <returns>
        /// The month span if the string was converted successfully, otherwise default.
        /// </returns>
        [Pure]
        public static MonthSpan TryParse(string s) => TryParse(s, null, out MonthSpan val) ? val : default;
        /// <summary>Converts the <see cref = "string "/> to <see cref = "MonthSpan"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the month span to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        [Pure]
        public static bool TryParse(string s, out MonthSpan result) => TryParse(s, null, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="MonthSpan"/>.</summary>
        /// <param name="s">
        /// A string containing the month span to convert.
        /// </param>
        /// <returns>
        /// The parsed month span.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        [Pure]
        public static MonthSpan Parse(string s)
            => TryParse(s, out MonthSpan val)
            ? val
            : throw new FormatException(QowaivMessages.FormatExceptionMonthSpan);

        /// <summary>Converts the <see cref="string"/> to <see cref="MonthSpan"/>.</summary>
        /// <param name="s">
        /// A string containing the month span to convert.
        /// </param>
        /// <returns>
        /// The month span if the string was converted successfully, otherwise default.
        /// </returns>
        [Pure]
        public static MonthSpan TryParse(string s) => TryParse(s, out MonthSpan val) ? val : default;
#endif
    }
}

namespace Qowaiv
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public partial struct MonthSpan
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid month span.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        [Pure]
        public static bool IsValid(string val) => IsValid(val, (IFormatProvider)null);
        /// <summary>Returns true if the value represents a valid month span.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        /// <param name = "formatProvider">
        /// The <see cref = "IFormatProvider"/> to interpret the <see cref = "string "/> value with.
        /// </param>
        [Pure]
        public static bool IsValid(string val, IFormatProvider formatProvider) => !string.IsNullOrWhiteSpace(val) && TryParse(val, formatProvider, out _);
#else
        /// <summary>Returns true if the value represents a valid month span.</summary>
        /// <param name="val">
        /// The <see cref="string"/> to validate.
        /// </param>
        [Pure]
        public static bool IsValid(string val)
            => !string.IsNullOrWhiteSpace(val)
            && TryParse(val, out _);
#endif
    }
}