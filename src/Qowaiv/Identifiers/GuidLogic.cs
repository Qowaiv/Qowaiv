using Qowaiv.Conversion;
using Qowaiv.Text;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierLogic"/> for an identifier based on <see cref="Guid"/>.</summary>
    public abstract class GuidLogic : IIdentifierLogic
    {
        internal static readonly GuidLogic Instance = new Default();

        /// <summary>Returns the type of the underlying value (<see cref="Guid"/>).</summary>
        public Type BaseType => typeof(Guid);

        /// <inheritdoc/>
        public virtual TypeConverter Converter { get; } = new GuidTypeConverter();

        /// <summary>Gets the default format used to represent the <see cref="Guid"/> as <see cref="string"/>.</summary>
        protected virtual string DefaultFormat => "d";

        /// <inheritdoc/>
        public virtual int Compare(object x, object y) => Id(x).CompareTo(Id(y));

        /// <inheritdoc/>
        public virtual new bool Equals(object x, object y) => Id(x).Equals(Id(y));

        /// <inheritdoc/>
        public virtual int GetHashCode(object obj) => Id(obj).GetHashCode();

        /// <inheritdoc/>
        public virtual byte[] ToByteArray(object obj) => obj is Guid guid ? guid.ToByteArray() : Array.Empty<byte>();

        /// <inheritdoc/>
        public virtual object FromBytes(byte[] bytes) => new Guid(bytes);

        /// <summary>Returns a formatted <see cref="string"/> that represents the <see cref="Guid"/>.</summary>
        /// <param name="obj">
        /// The object that is expected to be a <see cref="Guid"/>.
        /// </param>
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
        /// 
        /// H
        /// 26 base32 chars:
        /// ABCDEFGHIJKLMNOPQRSTUVWXYZ234567
        /// 
        /// N
        /// 32 digits:
        /// 00000000000000000000000000000000
        /// 
        /// D
        /// 32 digits separated by hyphens:
        /// 00000000-0000-0000-0000-000000000000
        /// 
        /// B
        /// 32 digits separated by hyphens, enclosed in braces:
        /// {00000000-0000-0000-0000-000000000000}
        /// 
        /// P
        /// 32 digits separated by hyphens, enclosed in parentheses:
        /// (00000000-0000-0000-0000-000000000000)
        /// 
        /// X
        /// Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces:
        /// {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
        /// 
        /// the lowercase formats are lowercase (except the the 's').
        /// </remarks>
        public virtual string ToString(object obj, string format, IFormatProvider formatProvider)
        {
            var id = Id(obj);

            if (id == Guid.Empty) { return string.Empty; }

            format = string.IsNullOrEmpty(format) ? DefaultFormat : format;

            switch (format)
            {
                case null:
                case "":
                case "s":
                case "S":
                    // avoid invalid URL characters
                    return Convert.ToBase64String(id.ToByteArray()).Replace('+', '-').Replace('/', '_').Substring(0, 22);

                case "h": return Base32.ToString(id.ToByteArray(), true);
                case "H": return Base32.ToString(id.ToByteArray(), false);

                case "N":
                case "D":
                case "B":
                case "P": return id.ToString(format, formatProvider).ToUpperInvariant();
                case "X": return id.ToString(format, formatProvider).ToUpperInvariant().Replace('X', 'x');

                case "n":
                case "d":
                case "b":
                case "p":
                case "x":
                    return id.ToString(format, formatProvider);

                default:
                    throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
            }
        }

        /// <inheritdoc/>
        public virtual object FromJson(long obj) => throw new NotSupportedException();

        /// <inheritdoc/>
        public virtual object ToJson(object obj) => ToString(obj, DefaultFormat, CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public virtual bool TryParse(string str, out object id)
        {
            id = default;

            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            if (Guid.TryParse(str, out Guid guid))
            {
                id = guid == Guid.Empty ? null : (object)guid;
                return true;
            }

            if (Uuid.Pattern.IsMatch(str))
            {
                var bytes = Convert.FromBase64String(str.Replace('-', '+').Replace('_', '/').Substring(0, 22) + "==");
                id = new Guid(bytes);

                if (Guid.Empty.Equals(id))
                {
                    id = null;
                }
                return true;
            }
            if (str.Length == 26 && Base32.TryGetBytes(str, out var b32))
            {
                id = new Guid(b32);

                if (Guid.Empty.Equals(id))
                {
                    id = null;
                }
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public virtual bool TryCreate(object obj, out object id)
        {
            id = default;

            if (obj is Guid guid)
            {
                id = guid == Guid.Empty ? null : (object)guid;
                return true;
            }

            if (obj is Uuid uuid)
            {
                id = guid == Guid.Empty ? null : (object)(Guid)uuid;
                return true;
            }
            if (obj is string str && TryParse(str, out id))
            {
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public virtual object Next() => Guid.NewGuid();

        private static Guid Id(object obj) => obj is Guid guid ? guid : Guid.Empty;

        private class Default : GuidLogic { }
    }
}
