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
namespace Qowaiv.Financial
{
    using System;
    using System.Diagnostics.Contracts;

    public partial struct Amount
    {
#if !NotField
        private Amount(decimal value) => m_Value = value;
        /// <summary>The inner value of the amount.</summary>
        private decimal m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  amount is empty, otherwise false.</summary>
        [Pure]
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  amount is unknown, otherwise false.</summary>
        [Pure]
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  amount is empty or unknown, otherwise false.</summary>
        [Pure]
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Diagnostics.Contracts;

    public partial struct Amount : IEquatable<Amount>
    {
        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) => obj is Amount other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other amount are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Amount"/> to compare with.</param>
        [Pure]
        public bool Equals(Amount other) => m_Value == other.m_Value;
#if !NotGetHashCodeStruct
        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => m_Value.GetHashCode();
#endif
#if !NotGetHashCodeClass
        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => m_Value is null ? 0 : m_Value.GetHashCode();
#endif
#endif
        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator !=(Amount left, Amount right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Amount left, Amount right) => left.Equals(right);
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public partial struct Amount : IComparable, IComparable<Amount>
    {
        /// <inheritdoc/>
        [Pure]
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is Amount other)
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
        public int CompareTo(Amount other) => Comparer<decimal>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Amount l, Amount r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Amount l, Amount r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Amount l, Amount r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Amount l, Amount r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;

    public partial struct Amount : ISerializable
    {
        /// <summary>Initializes a new instance of the amount based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private Amount(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (decimal)info.GetValue("Value", typeof(decimal));
        }

        /// <summary>Adds the underlying property of the amount to the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => Guard.NotNull(info, nameof(info)).AddValue("Value", m_Value);
    }
}

namespace Qowaiv.Financial
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct Amount : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the amount.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        [Pure]
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the amount from an <see href = "XmlReader"/>.</summary>
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

        partial void OnReadXml(Amount other);
        /// <summary>Writes the amount to an <see href = "XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses <see cref = "ToXmlString()"/>.
        /// </remarks>
        /// <param name = "writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer) => Guard.NotNull(writer, nameof(writer)).WriteString(ToXmlString());
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Qowaiv.Json;

    public partial struct Amount
    {
        /// <summary>Creates the amount from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized amount.
        /// </returns>
        
#if !NotCultureDependent
        [Pure]
        public static Amount FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        [Pure]
        public static Amount FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public partial struct Amount : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the amount.</summary>
        [Pure]
        public override string ToString() => ToString((IFormatProvider)null);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the amount.</summary>
        /// <param name = "format">
        /// The format that describes the formatting.
        /// </param>
        [Pure]
        public string ToString(string format) => ToString(format, null);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the amount.</summary>
        /// <param name = "provider">
        /// The format provider.
        /// </param>
        [Pure]
        public string ToString(IFormatProvider provider) => ToString(null, provider);
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public partial struct Amount
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Amount"/>.</summary>
        /// <param name = "s">
        /// A string containing the amount to convert.
        /// </param>
        /// <returns>
        /// The parsed amount.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        [Pure]
        public static Amount Parse(string s) => Parse(s, null);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Amount"/>.</summary>
        /// <param name = "s">
        /// A string containing the amount to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed amount.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        [Pure]
        public static Amount Parse(string s, IFormatProvider formatProvider) => TryParse(s, formatProvider, out Amount val) ? val : throw new FormatException(QowaivMessages.FormatExceptionFinancialAmount);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Amount"/>.</summary>
        /// <param name = "s">
        /// A string containing the amount to convert.
        /// </param>
        /// <returns>
        /// The amount if the string was converted successfully, otherwise default.
        /// </returns>
        [Pure]
        public static Amount TryParse(string s) => TryParse(s, null, out Amount val) ? val : default;
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Amount"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the amount to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        [Pure]
        public static bool TryParse(string s, out Amount result) => TryParse(s, null, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Amount"/>.</summary>
        /// <param name="s">
        /// A string containing the amount to convert.
        /// </param>
        /// <returns>
        /// The parsed amount.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        [Pure]
        public static Amount Parse(string s)
            => TryParse(s, out Amount val)
            ? val
            : throw new FormatException(QowaivMessages.FormatExceptionFinancialAmount);

        /// <summary>Converts the <see cref="string"/> to <see cref="Amount"/>.</summary>
        /// <param name="s">
        /// A string containing the amount to convert.
        /// </param>
        /// <returns>
        /// The amount if the string was converted successfully, otherwise default.
        /// </returns>
        [Pure]
        public static Amount TryParse(string s) => TryParse(s, out Amount val) ? val : default;
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public partial struct Amount
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid amount.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        [Pure]
        public static bool IsValid(string val) => IsValid(val, (IFormatProvider)null);
        /// <summary>Returns true if the value represents a valid amount.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        /// <param name = "formatProvider">
        /// The <see cref = "IFormatProvider"/> to interpret the <see cref = "string "/> value with.
        /// </param>
        [Pure]
        public static bool IsValid(string val, IFormatProvider formatProvider) => !string.IsNullOrWhiteSpace(val) && TryParse(val, formatProvider, out _);
#else
        /// <summary>Returns true if the value represents a valid amount.</summary>
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