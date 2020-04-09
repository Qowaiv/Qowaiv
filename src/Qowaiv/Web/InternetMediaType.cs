﻿#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Web;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
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
    [OpenApiDataType(description: "Media type notation as defined by RFC 6838, for example, text/html.", type: "string", format: "internet-media-type", nullable: true)]
    [TypeConverter(typeof(InternetMediaTypeTypeConverter))]
    public partial struct InternetMediaType : ISerializable, IXmlSerializable, IFormattable, IEquatable<InternetMediaType>, IComparable, IComparable<InternetMediaType>
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
        public static readonly InternetMediaType Unknown = new InternetMediaType("application/octet-stream");

        /// <summary>Gets the number of characters of the Internet media type.</summary>
        public int Length => IsEmpty() ? 0 : m_Value.Length;

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

        /// <summary>Serializes the Internet media type to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => m_Value;

        /// <summary>Returns a <see cref="string"/> that represents the current Internet media type for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => IsEmpty() ? "{empty}" : ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Internet media type.</summary>
        /// <param name="format">
        /// The format that describes the formatting.
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

        /// <summary>Gets an XML string representation of the Internet media type.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts an Internet media type to a <see cref="string"/>.</summary>
        public static explicit operator string(InternetMediaType val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a Internet media type.</summary>
        public static explicit operator InternetMediaType(string str) => Cast.InvariantString<InternetMediaType>(TryParse, str);

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
        public static bool TryParse(string s, out InternetMediaType result)
        {
            result = default;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            if (Qowaiv.Unknown.IsUnknown(s, CultureInfo.InvariantCulture))
            {
                result = Unknown;
                return true;
            }
            if (Pattern.IsMatch(s))
            {
                result = new InternetMediaType(s.ToLowerInvariant());
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
                return default;
            }
            var str = ResourceManager.GetString(Path.GetExtension(filename).ToUpperInvariant());
            return string.IsNullOrEmpty(str) ? Unknown : new InternetMediaType(str);
        }
        internal readonly static ResourceManager ResourceManager = new ResourceManager("Qowaiv.Web.InternetMediaType.FromFile", typeof(InternetMediaType).Assembly);
    }
}
