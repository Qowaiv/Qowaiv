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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a UUID (Universally unique identifier) aka GUID (Globally unique identifier).</summary>
    /// <remarks>
    /// The main difference between this UUID and the default System.GUID is 
    /// the default string representation. For this, that is a 22 char long
    /// Base64 representation.
    /// 
    /// The reason not to call this a GUID but an UUID it just to prevent users of 
    /// Qowaiv to be forced to specify the namespace of there GUID of choice 
    /// everywhere.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.AllExcludingCulture ^ SingleValueStaticOptions.HasUnknownValue, typeof(Guid))]
    [OpenApiDataType(description: "Universally unique identifier, Base64 encoded, for example lmZO_haEOTCwGsCcbIZFFg.", type: "string", format: "uuid-base64", nullable: true)]
    [TypeConverter(typeof(UuidTypeConverter))]
    public partial struct Uuid : ISerializable, IXmlSerializable, IFormattable, IEquatable<Uuid>, IComparable, IComparable<Uuid>
    {
        /// <summary>Gets the size of the <see cref="byte"/> array representation.</summary>
        public static readonly int ArraySize = 16;

        /// <summary>The index of the byte containing the version of a UUID is 7.</summary>
        internal const int IndexOfVersion = 7;

        /// <summary>Represents the pattern of a (potential) valid GUID.</summary>
        public static readonly Regex Pattern = new Regex(@"^[a-zA-Z0-9_-]{22}(=){0,2}$", RegexOptions.Compiled);

        /// <summary>Represents an empty/not set UUID.</summary>
        public static readonly Uuid Empty;

        /// <summary>Get the version of the UUID.</summary>
        public UuidVersion Version => m_Value.GetVersion();

        /// <summary>Returns a 16-element byte array that contains the value of this instance.</summary>
        public byte[] ToByteArray() => m_Value.ToByteArray();

        /// <summary>Serializes the UUID to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current UUID for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current UUID.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// S
        /// 22 base64 chars:
        /// 0123465798aAbBcCdDeE_-
        /// N
        /// 32 digits:
        /// 00000000000000000000000000000000
        /// D
        /// 32 digits separated by hyphens:
        /// 00000000-0000-0000-0000-000000000000
        /// B
        /// 32 digits separated by hyphens, enclosed in braces:
        /// {00000000-0000-0000-0000-000000000000}
        /// P
        /// 32 digits separated by hyphens, enclosed in parentheses:
        /// (00000000-0000-0000-0000-000000000000)
        /// X
        /// Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces:
        /// {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
        /// 
        /// the lowercase formats are lowercase (except the the 's').
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            if (IsEmpty()) { return string.Empty; }

            switch (format)
            {
                case null:
                case "":
                case "s":
                case "S":
                    // avoid invalid URL characters
                    return Convert.ToBase64String(ToByteArray()).Replace('+', '-').Replace('/', '_').Substring(0, 22);
                case "N":
                case "D":
                case "B":
                case "P": return m_Value.ToString(format, formatProvider).ToUpperInvariant();
                case "X": return m_Value.ToString(format, formatProvider).ToUpperInvariant().Replace('X', 'x');

                case "n":
                case "d":
                case "b":
                case "p":
                case "x":
                    return m_Value.ToString(format, formatProvider);

                default: throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
            }
        }

        /// <summary>Gets an XML string representation of the @FullName.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts a UUID to a <see cref="string"/>.</summary>
        public static explicit operator string(Uuid val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a UUID.</summary>
        public static explicit operator Uuid(string str) => Parse(str);

        /// <summary>Casts a Qowaiv.UUID to a System.GUID.</summary>
        public static implicit operator Guid(Uuid val) => val.m_Value;
        /// <summary>Casts a System.GUID to a Qowaiv.UUID.</summary>
        public static implicit operator Uuid(Guid val) => new Uuid(val);

        /// <summary>Initializes a new instance of a UUID.</summary>
        public static Uuid NewUuid() => new Uuid(Guid.NewGuid());

        /// <summary>Initializes a new  instance of a UUID that is sequential.</summary>
        /// <remarks>
        /// * The first 7 bytes are based on ticks from <see cref="Clock.UtcNow()"/>.
        /// * The UUID's generated are sequential between 1970-01-01 and  9276-12-03.
        /// * Withing a timespan of 32 ticks (0.32 nanoseconds) the sequential part
        /// of UUID's are identical.
        /// </remarks>
        public static Uuid NewSequential() => NewSequential(UuidComparer.Default);

        /// <summary>Initializes a new  instance of a UUID that is sequential.</summary>
        /// <param name="comparer">
        /// The comparer that determines the order to put the generated bytes in.
        /// </param>
        /// <remarks>
        /// * The first 7 bytes are based on ticks from <see cref="Clock.UtcNow()"/>.
        /// * The UUID's generated are sequential between 1970-01-01 and  9276-12-03.
        /// * Withing a timespan of 32 ticks (0.32 nanoseconds) the sequential part
        /// of UUID's are identical.
        /// </remarks>
        public static Uuid NewSequential(UuidComparer comparer)
        {
            var prioritizer = (comparer ?? UuidComparer.Default).Priority;
            var sequential = (Clock.UtcNow().Ticks - TicksYear1970) >> 5;

            // replace the version byte with a fully random one.
            var random = Guid.NewGuid().ToByteArray();
            random[IndexOfVersion] = random[0];

            // set the sequential part.
            for (var i = 6; i >= 0; i--)
            {
                random[i] = (byte)sequential;
                sequential >>= 8;
            }

            var bytes = new byte[ArraySize];

            // setting the priority.
            for (var index = 0; index < ArraySize; index++)
            {
                var prio = prioritizer[index];
                bytes[prio] = random[index];
            }

            // setting the version.
            UuidExtensions.SetVersion(bytes, UuidVersion.Sequential);

            return new Guid(bytes);
        }
        private const long TicksYear1970 = 0x89F_7FF5_F7B5_8000;

        /// <summary>Converts the string to a UUID.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a UUID to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Uuid result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            if (Pattern.IsMatch(s))
            {
                var bytes = Convert.FromBase64String(s.Replace('-', '+').Replace('_', '/').Substring(0, 22) + "==");
                result = new Uuid(new Guid(bytes));
                return true;
            }

            if (Guid.TryParse(s, out Guid id))
            {
                result = new Uuid(id);
                return true;
            }
            return false;
        }

        /// <summary>Generates an <see cref="Uuid"/> applying a <see cref="MD5"/> hash on the data.</summary>
        public static Uuid GenerateWithMD5(byte[] data)
        {
            Guard.NotNull(data, nameof(data));
            
            using var md5 = MD5.Create();
            
            var hash = md5.ComputeHash(data);
            UuidExtensions.SetVersion(hash, UuidVersion.MD5);
            return new Guid(hash);
        }

        /// <summary>Generates an <see cref="Uuid"/> applying a <see cref="SHA1"/> hash on the data.</summary>
        public static Uuid GenerateWithSHA1(byte[] data)
        {
            Guard.NotNull(data, nameof(data));
            
            using var sha1 = SHA1.Create();
            
            var bytes = sha1.ComputeHash(data);
            var hash = new byte[ArraySize];
            Array.Copy(bytes, hash, ArraySize);
            UuidExtensions.SetVersion(hash, UuidVersion.SHA1);
            return new Guid(hash);
        }
    }
}
