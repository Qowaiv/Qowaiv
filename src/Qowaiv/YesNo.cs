#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a Yes-no.</summary>
    /// <remarks>
    /// A Yes-no is a (bi-)polar that obviously has the values "yes" and "no". It also
    /// has an "empty"(unset) and "unknown" value.It maps easily with a <see cref="bool"/>, but
    /// Supports all kind of formatting(and both empty and unknown) that can not be
    /// achieved when modeling a property as <see cref="bool"/> instead of an <see cref="YesNo"/>.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(byte))]
    [TypeConverter(typeof(YesNoTypeConverter))]
    public struct YesNo : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<YesNo>, IComparable, IComparable<YesNo>
    {
        /// <summary>Represents an empty/not set Yes-no.</summary>
        public static readonly YesNo Empty;

        /// <summary>Represents an unknown (but set) Yes-no.</summary>
        public static readonly YesNo No = new YesNo { m_Value = 1 };

        /// <summary>Represents an unknown (but set) Yes-no.</summary>
        public static readonly YesNo Yes = new YesNo { m_Value = 2 };

        /// <summary>Represents an unknown (but set) Yes-no.</summary>
        public static readonly YesNo Unknown = new YesNo { m_Value = 3 };

        /// <summary>Contains yes and no.</summary>
        public static readonly IReadOnlyCollection<YesNo> YesAndNo = new ReadOnlyCollection<YesNo>(new List<YesNo> { Yes, No });
        
        #region Properties

        /// <summary>The inner value of the Yes-no.</summary>
        private byte m_Value;

        #endregion

        #region Methods

        /// <summary>Returns true if the Yes-no is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;

        /// <summary>Returns true if the Yes-no is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;

        /// <summary>Returns true if the Yes-no is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

        /// <summary>Returns true if the Yes-no value represents no, otherwise false.</summary>
        public bool IsNo() => m_Value == No.m_Value;

        /// <summary>Returns true if the Yes-no value represents yes, otherwise false.</summary>
        public bool IsYes() => m_Value == Yes.m_Value;

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of Yes-no based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private YesNo(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetByte("Value");
        }

        /// <summary>Adds the underlying property of Yes-no to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a Yes-no.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the Yes-no from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of Yes-no.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the Yes-no to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of Yes-no.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(SerializationValues[m_Value]);
        }

        #endregion
        
        #region (JSON) (De)serialization

        /// <summary>Generates a Yes-no from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = default;

        /// <summary>Generates a Yes-no from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the Yes-no.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString) => m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;

        /// <summary>Generates a Yes-no from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the Yes-no.
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger) => m_Value = Create((int)jsonInteger).m_Value;

        /// <summary>Generates a Yes-no from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the Yes-no.
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber) => m_Value = Create((int)jsonNumber).m_Value;
        
        /// <summary>Generates a Yes-no from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the Yes-no.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a Yes-no into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => SerializationValues[m_Value];

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current Yes-no for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty())
                {
                    return "{empty} (YesNo)";
                }
                if (IsUnknown())
                {
                    return "unknown (YesNo)";
                }
                return ToString(CultureInfo.InvariantCulture);
            }
        }

         /// <summary>Returns a <see cref="string"/> that represents the current Yes-no.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Yes-no.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Yes-no.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Yes-no.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// The formats:
        /// 
        /// i: as integer (note, unknown is a question mark sign).
        /// c/C: as single character (y/n/?) (Upper cased).
        /// f/F: as formatted string (Title cased).
        /// b/B: as boolean (true/false) (Title cased).
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            // If no format specified, use the default format.
            if (string.IsNullOrEmpty(format)) { format = "f"; }

            // Apply the format.
            return StringFormatter.Apply(this, format, formatProvider as CultureInfo ?? CultureInfo.CurrentCulture, FormatTokens);
        }

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<YesNo, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<YesNo, IFormatProvider, string>>()
        {
            { 'c', (svo, provider) => svo.GetResourceString("ch_", provider) },
            { 'C', (svo, provider) => svo.GetResourceString("ch_", provider).ToUpper(provider) },
            { 'i', (svo, provider) => svo.GetResourceString("int_", provider) },
            { 'f', (svo, provider) => svo.GetResourceString("f_", provider) },
            { 'F', (svo, provider) => svo.GetResourceString("f_", provider).ToTitleCase(provider) },
            { 'b', (svo, provider) => svo.GetResourceString("b_", provider) },
            { 'B', (svo, provider) => svo.GetResourceString("b_", provider).ToTitleCase(provider) },
        };

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is YesNo && Equals((YesNo)obj);

        /// <summary>Returns true if this instance and the other <see cref="YesNo" /> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="YesNo" /> to compare with.</param>
        public bool Equals(YesNo other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this Yes-no.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(YesNo left, YesNo right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(YesNo left, YesNo right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a Yes-no.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a Yes-no.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is YesNo)
            {
                return CompareTo((YesNo)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a Yes-no"), "obj");
        }

        /// <summary>Compares this instance with a specified Yes-no and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified Yes-no.
        /// </summary>
        /// <param name="other">
        /// The Yes-no to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(YesNo other) => m_Value.CompareTo(other.m_Value);

        #endregion
       
        #region (Explicit) casting

        /// <summary>Casts a Yes-no to a <see cref="string"/>.</summary>
        public static explicit operator string(YesNo val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a Yes-no.</summary>
        public static explicit operator YesNo(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts a Yes-no to a nullable <see cref="bool"/>.</summary>
        public static explicit operator bool?(YesNo val) => BooleanValues[val.m_Value];
        /// <summary>Casts a nullable <see cref="bool"/> to a Yes-no.</summary>
        public static explicit operator YesNo(bool? val)
        {
            if(val.HasValue)
            {
                return val.Value ? Yes : No;
            }
            return Empty;
        }

        private static readonly bool?[] BooleanValues = new bool?[] { null, false, true, null };

        #endregion

        #region Factory methods

        /// <summary>Converts the string to a Yes-no.</summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <returns>
        /// A Yes-no.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static YesNo Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to a Yes-no.</summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A Yes-no.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static YesNo Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out YesNo val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionYesNo);
        }

        /// <summary>Converts the string to a Yes-no.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <returns>
        /// The Yes-no if the string was converted successfully, otherwise Empty.
        /// </returns>
        public static YesNo TryParse(string s)
        {
            if (TryParse(s, out YesNo val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to a Yes-no.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out YesNo result) => TryParse(s, CultureInfo.CurrentCulture, out result);

        /// <summary>Converts the string to a Yes-no.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Yes-no to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out YesNo result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
            if (Qowaiv.Unknown.IsUnknown(s, culture))
            {
                result = Unknown;
                return true;
            }
            AddCulture(culture);

            var str = Parsing.ToUnified(s);

            if (Parsings[culture].TryGetValue(str, out byte val) ||
                Parsings[CultureInfo.InvariantCulture].TryGetValue(str, out val))
            {
                result = new YesNo { m_Value = val };
                return true;
            }
            return false;
        }

        /// <summary >Creates a Yes-no from a byte. </summary >
        /// <param name="val" >
        /// A decimal describing a Yes-no.
        /// </param >
        /// <exception cref="FormatException" >
        /// val is not a valid Yes-no.
        /// </exception >
        private static YesNo Create(int? val)
        {
            if (TryCreate(val, out YesNo result))
            {
                return result;
            }
            throw new ArgumentOutOfRangeException("val", QowaivMessages.FormatExceptionYesNo);
        }

        /// <summary >Creates a Yes-no from a byte.
        /// A return value indicates whether the creation succeeded.
        /// </summary >
        /// <param name="val" >
        /// A byte describing a Yes-no.
        /// </param >
        /// <param name="result" >
        /// The result of the creation.
        /// </param >
        /// <returns >
        /// True if a Yes-no was created successfully, otherwise false.
        /// </returns >
        private static bool TryCreate(int? val, out YesNo result)
        {
            result = Empty;

            if (!val.HasValue)
            {
                return true;
            }
            if(val == 0)
            {
                result = No;
                return true;
            }
            if(val == 1)
            {
                result = Yes;
                return true;
            }
            if(val == byte.MaxValue || val == short.MaxValue || val == int.MaxValue)
            {
                result = Unknown;
                return true;
            }
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid Yes-no, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid Yes-no, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(val)) { return false; }
            return TryParse(val, formatProvider, out YesNo result);
        }

        #endregion

        #region Resources

        private static ResourceManager ResourceManager = new ResourceManager("Qowaiv.YesNoLabels", typeof(YesNo).Assembly);

        /// <summary>Get resource string.</summary>
        /// <param name="prefix">
        /// The prefix of the resource key.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        internal string GetResourceString(string prefix, IFormatProvider formatProvider)
        {
            return GetResourceString(prefix, formatProvider as CultureInfo);
        }

        /// <summary>Get resource string.</summary>
        /// <param name="prefix">
        /// The prefix of the resource key.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        internal string GetResourceString(string prefix, CultureInfo culture)
        {
            if (IsEmpty()) { return string.Empty; }
            return ResourceManager.GetString(prefix + LookupSuffix[m_Value], culture ?? CultureInfo.CurrentCulture) ?? string.Empty;
        }

        #endregion

        #region Lookup

        /// <summary>Gets the yes-no labels.</summary>
        /// <remarks>
        /// Used for both serialization and resource lookups.
        /// </remarks>
        private static readonly string[] LookupSuffix = { null, "No", "Yes", "Unknown" };
        private static readonly string[] SerializationValues = { null, "no", "yes", "?" };

        /// <summary>Adds a culture to the parsings.</summary>
        /// <param name="culture">
        /// The culture to add.
        /// </param>
        private static void AddCulture(CultureInfo culture)
        {
            lock (locker)
            {
                if (Parsings.ContainsKey(culture)) { return; }

                Parsings[culture] = new Dictionary<string, byte>();

                foreach (var value in YesAndNo)
                {
                    var label = value.ToString("", culture).ToUpper(culture);
                    var @char = value.ToString("c", culture).ToUpper(culture);
                    var @bool = value.ToString("b", culture).ToUpper(culture);

                    Parsings[culture][label] = value.m_Value;
                    Parsings[culture][@char] = value.m_Value;
                    Parsings[culture][@bool] = value.m_Value;
                }
            }
        }

        /// <summary>Represents the parsing keys.</summary>
        private static readonly Dictionary<CultureInfo, Dictionary<string, byte>> Parsings = new Dictionary<CultureInfo, Dictionary<string, byte>>()
        {
            {
                CultureInfo.InvariantCulture, new Dictionary<string, byte>()
                {
                    { "", 0 },
                    { "0", 1 },
                    { "1", 2 },
                    { "FALSE", 1 },
                    { "TRUE", 2 },
                    { "NO", 1 },
                    { "YES", 2 },
                }
            }
        };

        /// <summary>The locker for adding a culture.</summary>
        private static readonly object locker = new object();

        #endregion
    }
}
