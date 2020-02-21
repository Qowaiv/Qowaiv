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
#define NotGetHashCodeClass
namespace Qowaiv.IO
{
    using System;

    public partial struct StreamSize
    {
#if !NotField
        private StreamSize(long value) => m_Value = value;
        /// <summary>The inner value of the stream size.</summary>
        private long m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  stream size is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  stream size is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  stream size is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv.IO
{
    using System;

    public partial struct StreamSize : IEquatable<StreamSize>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is StreamSize other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other stream size are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "StreamSize"/> to compare with.</param>
        public bool Equals(StreamSize other) => m_Value == other.m_Value;
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
        public static bool operator !=(StreamSize left, StreamSize right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(StreamSize left, StreamSize right) => left.Equals(right);
    }
}

namespace Qowaiv.IO
{
    using System;
    using System.Collections.Generic;

    public partial struct StreamSize : IComparable, IComparable<StreamSize>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is StreamSize other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

#if !NotEqualsSvo
        /// <inheritdoc/>
        public int CompareTo(StreamSize other) => Comparer<long>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(StreamSize l, StreamSize r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(StreamSize l, StreamSize r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(StreamSize l, StreamSize r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(StreamSize l, StreamSize r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv.IO
{
    using System;
    using System.Runtime.Serialization;

    public partial struct StreamSize : ISerializable
    {
        /// <summary>Initializes a new instance of the stream size based on the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        private StreamSize(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (long)info.GetValue("Value", typeof(long));
        }

        /// <summary>Adds the underlying property of the stream size to the serialization info.</summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }
    }
}

namespace Qowaiv.IO
{
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct StreamSize : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the stream size.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the stream size from an <see href = "XmlReader"/>.</summary>
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

        partial void OnReadXml(StreamSize other);
        /// <summary>Writes the stream size to an <see href = "XmlWriter"/>.</summary>
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

namespace Qowaiv.IO
{
    using System;
    using System.Globalization;
    using Qowaiv.Json;

    public partial struct StreamSize
    {
        /// <summary>Creates the stream size from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized stream size.
        /// </returns>
        
#if !NotCultureDependent
        public static StreamSize FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        public static StreamSize FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv.IO
{
    using System;
    using System.Globalization;

    public partial struct StreamSize
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "StreamSize"/>.</summary>
        /// <param name = "s">
        /// A string containing the stream size to convert.
        /// </param>
        /// <returns>
        /// The parsed stream size.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static StreamSize Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "StreamSize"/>.</summary>
        /// <param name = "s">
        /// A string containing the stream size to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed stream size.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static StreamSize Parse(string s, IFormatProvider formatProvider)
        {
            return TryParse(s, formatProvider, out StreamSize val) ? val : throw new FormatException(QowaivMessages.FormatExceptionStreamSize);
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "StreamSize"/>.</summary>
        /// <param name = "s">
        /// A string containing the stream size to convert.
        /// </param>
        /// <returns>
        /// The stream size if the string was converted successfully, otherwise default.
        /// </returns>
        public static StreamSize TryParse(string s)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out StreamSize val) ? val : default;
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "StreamSize"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the stream size to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out StreamSize result) => TryParse(s, CultureInfo.CurrentCulture, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="StreamSize"/>.</summary>
        /// <param name="s">
        /// A string containing the stream size to convert.
        /// </param>
        /// <returns>
        /// The parsed stream size.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static StreamSize Parse(string s)
        {
            return TryParse(s, out StreamSize val)
                ? val
                : throw new FormatException(QowaivMessages.FormatExceptionStreamSize);
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="StreamSize"/>.</summary>
        /// <param name="s">
        /// A string containing the stream size to convert.
        /// </param>
        /// <returns>
        /// The stream size if the string was converted successfully, otherwise default.
        /// </returns>
        public static StreamSize TryParse(string s)
        {
            return TryParse(s, out StreamSize val) ? val : default;
        }
#endif
    }
}

namespace Qowaiv.IO
{
    using System;
    using System.Globalization;

    public partial struct StreamSize
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid stream size.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);
        /// <summary>Returns true if the value represents a valid stream size.</summary>
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
        /// <summary>Returns true if the value represents a valid stream size.</summary>
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