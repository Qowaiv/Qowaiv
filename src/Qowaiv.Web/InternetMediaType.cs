#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Formatting;
using Qowaiv.Json;
using Qowaiv.Web.Conversion;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Web
{
    /// <summary>Represents an Internet media type.</summary>
    /// <remarks>
    /// An Internet media type is a standard identifier used on the Internet to
    /// indicate the type of data that a file contains. Common uses include the
    /// following:
    /// email clients use them to identify attachment files, web browsers use them
    /// to determine how to display or output files that are not in HTML format,
    /// search engines use them to classify data files on the web.
    /// 
    /// A media type is composed of a type, a subtype, and zero or more optional
    /// parameters. As an example, an HTML file might be designated text/html;
    /// charset=UTF-8.
    /// 
    /// In this example text is the type, html is the subtype, and charset=UTF-8
    /// is an optional parameter indicating the character encoding.
    /// 
    /// IANA manages the official registry of media types.
    /// The identifiers were originally defined in RFC 2046, and were called MIME
    /// types because they referred to the non-ASCII parts of email messages that
    /// were composed using the MIME (Multipurpose Internet Mail Extensions)
    /// specification. They are also sometimes referred to as Content-types.
    /// 
    /// Their use has expanded from email sent through SMTP, to other protocols
    /// such as HTTP, RTP and SIP.
    /// New media types can be created with the procedures outlined in RFC 6838.
    /// 
    /// See http://tools.ietf.org/html/rfc2046
    /// See http://tools.ietf.org/html/rfc6838
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.AllExcludingCulture, typeof(string))]
    [TypeConverter(typeof(InternetMediaTypeTypeConverter))]
    public struct InternetMediaType : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<InternetMediaType>, IComparable, IComparable<InternetMediaType>
    {
        /// <summary>Represents the pattern of a (potential) valid Internet media type.</summary>
        public static readonly Regex Pattern = new Regex('^' + PatternTopLevel + '/' + PatternSubtype + PatternSuffix + '$', RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>The pattern of the top level.</summary>
        private const string PatternTopLevel = @"(?<toplevel>(x\-[a-z]+|application|audio|example|image|message|model|multipart|text|video))";
        /// <summary>The pattern of the subtype.</summary>
        private const string PatternSubtype = @"(?<subtype>[a-z0-9]+([\-\.][a-z0-9]+)*)";
        /// <summary>The pattern of the suffix.</summary>
        private const string PatternSuffix = @"(\+(?<suffix>(xml|json|ber|der|fastinfoset|wbxml|zip|cbor)))?";

        /// <summary>Represents an empty/not set Internet media type.</summary>
        public static readonly InternetMediaType Empty;

        /// <summary>Represents an unknown (but set) Internet media type.</summary>
        public static readonly InternetMediaType Unknown = new InternetMediaType { m_Value = "application/octet-stream" };

        #region Properties

        /// <summary>The inner value of the Internet media type.</summary>
        private string m_Value;

        /// <summary>Gets the number of characters of the Internet media type.</summary>
        public int Length { get { return IsEmpty() ? 0 : m_Value.Length; } }

        /// <summary>Gets the top-level of the Internet media type.</summary>
        public string TopLevel
        {
            get
            {
                if (IsEmpty()) { return string.Empty; }
                return Pattern.Match(m_Value).Groups["toplevel"].Value;
            }
        }

        /// <summary>Gets the top-level type of the Internet media type.</summary>
        /// <remarks>
        /// If the top level start with "x-" unregistered is returned.
        /// </remarks>
        public InternetMediaTopLevelType TopLevelType
        {
            get
            {
                if (IsEmpty())
                {
                    return InternetMediaTopLevelType.None;
                }
                return
                    Enum.TryParse(TopLevel, true, out InternetMediaTopLevelType type)
                    ? type
                    : InternetMediaTopLevelType.Unregistered;
            }
        }

        /// <summary>Gets the subtype of the Internet media type.</summary>
        public string Subtype
        {
            get
            {
                if (IsEmpty()) { return string.Empty; }
                return Pattern.Match(m_Value).Groups["subtype"].Value;
            }
        }

        /// <summary>Gets the suffix of the Internet media type.</summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults",
            MessageId = "System.Enum.TryParse<Qowaiv.Web.InternetMediaSuffixType>(System.String,System.Boolean,Qowaiv.Web.InternetMediaSuffixType@)",
            Justification = "If the parsing succeeded or not, is ignored on purpose.")]
        public InternetMediaSuffixType Suffix
        {
            get
            {
                Enum.TryParse(Pattern.Match(m_Value ?? string.Empty).Groups["suffix"].Value, true, out InternetMediaSuffixType type);
                return type;
            }
        }
        /// <summary>Returns true if Internet media type is a registered type, otherwise false.</summary>
        /// <remarks>
        /// This is based on a naming convention, not on actual registration.
        /// </remarks>
        public bool IsRegistered
        {
            get
            {
                return
                    TopLevelType != InternetMediaTopLevelType.None &&
                    TopLevelType != InternetMediaTopLevelType.Unregistered &&
                    !Subtype.StartsWith("x-", StringComparison.Ordinal) &&
                    !Subtype.StartsWith("x.", StringComparison.Ordinal);
            }
        }

        #endregion

        #region Methods

        /// <summary>Returns true if the Internet media type is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default(string);

        /// <summary>Returns true if the Internet media type is unknown, otherwise false.</summary>
        public bool IsUnknown() { return m_Value == InternetMediaType.Unknown.m_Value; }

        /// <summary>Returns true if the Internet media type is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of Internet media type based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private InternetMediaType(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetString("Value");
        }

        /// <summary>Adds the underlying property of Internet media type to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize an Internet media type.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the Internet media type from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of Internet media type.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the Internet media type to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of Internet media type.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates an Internet media type from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson()
        {
            m_Value = default(string);
        }

        /// <summary>Generates an Internet media type from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the Internet media type.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString).m_Value;
        }

        /// <summary>Generates an Internet media type from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the Internet media type.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger) => new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates an Internet media type from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the Internet media type.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates an Internet media type from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the Internet media type.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts an Internet media type into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return m_Value == default(string) ? null : ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current Internet media type for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty()) { return "Internet Media Type: (empty)"; }
                return ToString();
            }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current Internet media type.</summary>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Internet media type.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Internet media type.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("", formatProvider);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Internet media type.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            return m_Value ?? string.Empty;
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) { return obj is InternetMediaType && Equals((InternetMediaType)obj); }

        /// <summary>Returns true if this instance and the other <see cref="InternetMediaType"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="InternetMediaType"/> to compare with.</param>
        public bool Equals(InternetMediaType other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this Internet media type.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value == null ? 0 : m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(InternetMediaType left, InternetMediaType right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(InternetMediaType left, InternetMediaType right)
        {
            return !(left == right);
        }

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a Internet media type.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a Internet media type.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is InternetMediaType)
            {
                return CompareTo((InternetMediaType)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "an Internet media type"), "obj");
        }

        /// <summary>Compares this instance with a specified Internet media type and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified Internet media type.
        /// </summary>
        /// <param name="other">
        /// The Internet media type to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(InternetMediaType other) { return String.Compare(m_Value, other.m_Value, StringComparison.Ordinal); }

        #endregion

        #region (Explicit) casting

        /// <summary>Casts an Internet media type to a <see cref="string"/>.</summary>
        public static explicit operator string(InternetMediaType val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a Internet media type.</summary>
        public static explicit operator InternetMediaType(string str) { return InternetMediaType.Parse(str); }


        #endregion

        #region Factory methods

        /// <summary>Converts the string to an Internet media type.</summary>
        /// <param name="s">
        /// A string containing an Internet media type to convert.
        /// </param>
        /// <returns>
        /// An Internet media type.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static InternetMediaType Parse(string s)
        {
            if (TryParse(s, out InternetMediaType val))
            {
                return val;
            }
            throw new FormatException(QowaivWebMessages.FormatExceptionInternetMediaType);
        }

        /// <summary>Converts the string to an Internet media type.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Internet media type to convert.
        /// </param>
        /// <returns>
        /// The Internet media type if the string was converted successfully, otherwise InternetMediaType.Empty.
        /// </returns>
        public static InternetMediaType TryParse(string s)
        {
            if (TryParse(s, out InternetMediaType val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to an Internet media type.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Internet media type to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
            Justification = "Internet media types are represented in lowercase by default.")]
        public static bool TryParse(string s, out InternetMediaType result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            if (Qowaiv.Unknown.IsUnknown(s, CultureInfo.InvariantCulture))
            {
                result = Unknown;
                return true;
            }
            if (IsValid(s))
            {
                result = new InternetMediaType { m_Value = s.ToLowerInvariant() };
                return true;
            }
            return false;
        }

        /// <summary>Gets the Internet media type base on the file.</summary>
        /// <param name="file">
        /// The file to retrieve the Internet media type from.
        /// </param>
        /// <remarks>
        /// Based on the extension of the file.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "This has no meaning for other derived classes of FileSystemInfo.")]
        public static InternetMediaType FromFile(FileInfo file)
        {
            return
                file is null
                ? Empty
                : FromFile(file.Name);
        }

        /// <summary>Gets the Internet media type base on the filename.</summary>
        /// <param name="filename">
        /// The filename to retrieve the Internet media type from.
        /// </param>
        /// <remarks>
        /// Based on the extension of the filename.
        /// </remarks>
        public static InternetMediaType FromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return Empty;
            }
            var str = ResourceManager.GetString(Path.GetExtension(filename).ToUpperInvariant());
            return string.IsNullOrEmpty(str) ? InternetMediaType.Unknown : new InternetMediaType { m_Value = str };
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid Internet media type, otherwise false.</summary>
        public static bool IsValid(string val)
        {
            return Pattern.IsMatch(val ?? string.Empty);
        }

        #endregion

        #region Resources

        internal readonly static ResourceManager ResourceManager = new ResourceManager("Qowaiv.Web.InternetMediaType.FromFile", typeof(InternetMediaType).Assembly);

        #endregion
    }
}
