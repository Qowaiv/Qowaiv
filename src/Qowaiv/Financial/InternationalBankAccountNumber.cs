using Qowaiv.Conversion.Financial;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
    ///<summary>The International Bank Account Number (IBAN) is an international standard
    /// for identifying bank accounts across national borders with a minimal risk
    /// of propagating transcription errors. It was originally adopted by the European
    /// Committee for Banking Standards (ECBS), and was later adopted as an international
    /// standard under ISO 13616:1997 and now as ISO 13616-1:2007.
    /// </summary>
    /// <remarks>
    /// The official IBAN registrar under ISO 13616-2:2007 is SWIFT.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for an IBAN.")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [TypeConverter(typeof(InternationalBankAccountNumberTypeConverter))]
    public partial struct InternationalBankAccountNumber : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<InternationalBankAccountNumber>, IComparable, IComparable<InternationalBankAccountNumber>
    {
        /// <summary>Represents the pattern of a (potential) valid IBAN.</summary>
        /// <remarks>
        /// Pairs of IBAN characters can be divided by maximum 2 spacing characters.
        /// </remarks>
        public static readonly Regex Pattern = new Regex(@"^[A-Z]\s{0,2}[A-Z]\s{0,2}[0-9]\s{0,2}[0-9](\s{0,2}[0-9A-Z]){8,32}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents an empty/not set IBAN.</summary>
        public static readonly InternationalBankAccountNumber Empty;

        /// <summary>Represents an unknown (but set) IBAN.</summary>
        public static readonly InternationalBankAccountNumber Unknown = new InternationalBankAccountNumber { m_Value = "ZZ" };

        #region Properties

        /// <summary>The inner value of the IBAN.</summary>
        private string m_Value;

        /// <summary>Gets the number of characters of IBAN.</summary>
        public int Length => IsEmptyOrUnknown() ? 0 : m_Value.Length;


        /// <summary>Gets the country of IBAN.</summary>
        public Country Country
        {
            get
            {
                if (m_Value == default(string))
                {
                    return Country.Empty;
                }
                if (m_Value == Unknown.m_Value)
                {
                    return Country.Unknown;
                }
                return Country.Parse(m_Value.Substring(0, 2), CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region Methods

        /// <summary>Returns true if the IBAN is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default(string);

        /// <summary>Returns true if the Gender is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;

        /// <summary>Returns true if the IBAN is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of IBAN based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private InternationalBankAccountNumber(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, "info");
            m_Value = info.GetString("Value");
        }

        /// <summary>Adds the underlying property of IBAN to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, "info");
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize an IBAN.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the IBAN from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of IBAN.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, "reader");
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the IBAN to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of IBAN.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, "writer");
            writer.WriteString(ToUnformattedString());
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates an IBAN from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = default(string);

        /// <summary>Generates an IBAN from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the IBAN.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates an IBAN from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the IBAN.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger) => new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates an IBAN from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the IBAN.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates an IBAN from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the IBAN.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts an IBAN into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return m_Value == default(string) ? null : ToUnformattedString();
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current IBAN for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                if (m_Value == default(string))
                {
                    return "IBAN: (empty)";
                }
                if (m_Value == Unknown.m_Value)
                {
                    return "IBAN: (unknown)";
                }
                return "IBAN: " + ToFormattedString();
            }
        }

        /// <summary>Formats the IBAN without spaces.</summary>
        private string ToUnformattedString()
        {
            if (m_Value == default(string))
            {
                return string.Empty;
            }
            if (m_Value == Unknown.m_Value)
            {
                return "?";
            }
            return m_Value;
        }
        /// <summary>Formats the IBAN without spaces as lowercase.</summary>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
            Justification = "This is not about normalization but formatting.")]
        private string ToUnformattedLowercaseString() => ToUnformattedString().ToLowerInvariant();


        /// <summary>Formats the IBAN with spaces.</summary>
        private string ToFormattedString()
        {
            if (m_Value == default(string))
            {
                return string.Empty;
            }
            if (m_Value == Unknown.m_Value)
            {
                return "?";
            }
            return FormattedPattern.Replace(m_Value, "$0 ");
        }
        /// <summary>Formats the IBAN with spaces as lowercase.</summary>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
            Justification = "This is not about normalization but formatting.")]
        private string ToFormattedLowercaseString() => ToFormattedString().ToLowerInvariant();


        private static readonly Regex FormattedPattern = new Regex(@"\w{4}(?!$)", RegexOptions.Compiled);

        /// <summary>Returns a <see cref="string"/> that represents the current IBAN.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current IBAN.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current IBAN.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current IBAN.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// The formats:
        /// 
        /// u: as unformatted lowercase.
        /// U: as unformatted uppercase.
        /// f: as formatted lowercase.
        /// F: as formatted uppercase.
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            // If no format specified, use the default format.
            if (string.IsNullOrEmpty(format))
            {
                return ToUnformattedString();
            }
            // Apply the format.
            return StringFormatter.Apply(this, format, formatProvider, FormatTokens);
        }

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<InternationalBankAccountNumber, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<InternationalBankAccountNumber, IFormatProvider, string>>()
        {
            { 'u', (svo, provider) => svo.ToUnformattedLowercaseString() },
            { 'U', (svo, provider) => svo.ToUnformattedString() },
            { 'f', (svo, provider) => svo.ToFormattedLowercaseString() },
            { 'F', (svo, provider) => svo.ToFormattedString() },
        };

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is InternationalBankAccountNumber && Equals((InternationalBankAccountNumber)obj);

        /// <summary>Returns true if this instance and the other <see cref="InternationalBankAccountNumber"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="InternationalBankAccountNumber"/> to compare with.</param>
        public bool Equals(InternationalBankAccountNumber other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this IBAN.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value == null ? 0 : m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(InternationalBankAccountNumber left, InternationalBankAccountNumber right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(InternationalBankAccountNumber left, InternationalBankAccountNumber right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a IBAN.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a IBAN.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is InternationalBankAccountNumber)
            {
                return CompareTo((InternationalBankAccountNumber)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "an IBAN"), "obj");
        }

        /// <summary>Compares this instance with a specified IBAN and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified IBAN.
        /// </summary>
        /// <param name="other">
        /// The IBAN to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(InternationalBankAccountNumber other) => string.Compare(m_Value, other.m_Value, StringComparison.Ordinal);

        #endregion

        #region (Explicit) casting

        /// <summary>Casts an IBAN to a <see cref="string"/>.</summary>
        public static explicit operator string(InternationalBankAccountNumber val) => val.ToString();
        /// <summary>Casts a <see cref="string"/> to a IBAN.</summary>
        public static explicit operator InternationalBankAccountNumber(string str) => Parse(str, CultureInfo.InvariantCulture);

        #endregion

        #region Factory methods

        /// <summary>Converts the string to an IBAN.</summary>
        /// <param name="s">
        /// A string containing an IBAN to convert.
        /// </param>
        /// <returns>
        /// An IBAN.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static InternationalBankAccountNumber Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to an IBAN.</summary>
        /// <param name="s">
        /// A string containing an IBAN to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// An IBAN.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static InternationalBankAccountNumber Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out InternationalBankAccountNumber val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionInternationalBankAccountNumber);
        }

        /// <summary>Converts the string to an IBAN.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an IBAN to convert.
        /// </param>
        /// <returns>
        /// The IBAN if the string was converted successfully, otherwise InternationalBankAccountNumber.Empty.
        /// </returns>
        public static InternationalBankAccountNumber TryParse(string s)
        {
            if (TryParse(s, CultureInfo.CurrentCulture, out InternationalBankAccountNumber val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to an IBAN.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an IBAN to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out InternationalBankAccountNumber result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to an IBAN.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an IBAN to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out InternationalBankAccountNumber result)
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
            if (Pattern.IsMatch(s))
            {
                var str = Parsing.ClearSpacingToUpper(s);
                var country = Country.TryParse(str.Substring(0, 2));


                if (str.Length > 11 && !country.IsEmptyOrUnknown() &&
                    (!LocalizedPatterns.TryGetValue(country, out Regex localizedPattern) || localizedPattern.IsMatch(str)))
                {
                    var validation = Alphanumeric.Replace(str.Substring(4) + str.Substring(0, 4), AlphanumericToNumeric);

                    int sum = 0;
                    int exp = 1;

                    for (int pos = validation.Length - 1; pos >= 0; pos--)
                    {
                        sum += exp * AlphanumericAndNumericLookup.IndexOf(validation[pos]);
                        exp = (exp * 10) % 97;
                    }
                    if ((sum % 97) == 1)
                    {
                        result = new InternationalBankAccountNumber { m_Value = str };
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid IBAN, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid IBAN, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            InternationalBankAccountNumber iban;
            return
                !string.IsNullOrEmpty(val) &&
                !Qowaiv.Unknown.IsUnknown(val, formatProvider as CultureInfo) &&
                TryParse(val, formatProvider, out iban);
        }

        /// <summary>Contains a lookup for alphanumeric and numeric chars.</summary>
        private const string AlphanumericAndNumericLookup = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>Matches on Alphanumeric uppercase chars.</summary>
        private static readonly Regex Alphanumeric = new Regex("[A-Z]", RegexOptions.Compiled);

        /// <summary>Replaces A by 11, B by 12 ect.</summary>
        private static string AlphanumericToNumeric(Match match)
        {
            return AlphanumericAndNumericLookup
                .IndexOf(match.Value, StringComparison.Ordinal)
                .ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
