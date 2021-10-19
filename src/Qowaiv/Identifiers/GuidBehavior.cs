using Qowaiv.Text;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="Guid"/>.</summary>
    public abstract class GuidBehavior : IdentifierBehavior
    {
        internal static readonly GuidBehavior Instance = new Default();

        /// <summary>Returns the type of the underlying value (<see cref="Guid"/>).</summary>
        public sealed override Type BaseType => typeof(Guid);

        /// <summary>Gets the default format used to represent the <see cref="Guid"/> as <see cref="string"/>.</summary>
        protected virtual string DefaultFormat => "d";

        /// <inheritdoc/>
        public override int Compare(object x, object y) => Id(x).CompareTo(Id(y));

        /// <inheritdoc/>
        public override bool Equals(object x, object y) => Id(x).Equals(Id(y));

        /// <inheritdoc/>
        public override int GetHashCode(object obj) => Id(obj).GetHashCode();

        /// <inheritdoc/>
        public override byte[] ToByteArray(object obj) 
            => obj is Guid guid ? guid.ToByteArray() : Array.Empty<byte>();

        /// <inheritdoc/>
        public override object FromBytes(byte[] bytes) => new Guid(bytes);

        /// <summary>Returns a formatted <see cref="string"/> that represents the <see cref="Guid"/>.</summary>
        /// <param name="obj">
        /// The object that is expected to be a <see cref="Guid"/>.
        /// </param>
        /// <param name="format">
        /// The format that describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// S => 22 base64 chars:  0123465798aAbBcCdDeE_-
        /// 
        /// H => 26 base32 chars: ABCDEFGHIJKLMNOPQRSTUVWXYZ234567
        /// 
        /// N => 32 digits: 00000000000000000000000000000000
        /// 
        /// D => 32 digits separated by hyphens: 00000000-0000-0000-0000-000000000000
        /// 
        /// B => 32 digits separated by hyphens, enclosed in braces: {00000000-0000-0000-0000-000000000000}
        /// 
        /// P => 32 digits separated by hyphens, enclosed in parentheses: (00000000-0000-0000-0000-000000000000)
        /// 
        /// X => Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values 
        ///     that is also enclosed in braces: {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
        /// 
        /// the lowercase formats are lowercase (except the 's').
        /// </remarks>
        public override string ToString(object obj, string format, IFormatProvider formatProvider)
        {
            var id = Id(obj);

            if (id == Guid.Empty) return string.Empty;
            else
            {
                format = string.IsNullOrEmpty(format) ? DefaultFormat : format;
                return format switch
                {
                    null or "" or "s" or "S" => Convert.ToBase64String(id.ToByteArray()).Replace('+', '-').Replace('/', '_').Substring(0, 22),// avoid invalid URL characters
                    "h" => Base32.ToString(id.ToByteArray(), true),
                    "H" => Base32.ToString(id.ToByteArray(), false),
                    "N" or "D" or "B" or "P" => id.ToString(format, formatProvider).ToUpperInvariant(),
                    "X" => id.ToString(format, formatProvider).ToUpperInvariant().Replace('X', 'x'),
                    "n" or "d" or "b" or "p" or "x" => id.ToString(format, formatProvider),
                    _ => throw new FormatException(QowaivMessages.FormatException_InvalidFormat),
                };
            }
        }

        /// <inheritdoc/>
        public override object ToJson(object obj) => ToString(obj, DefaultFormat, CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public override bool TryParse(string str, out object id)
        {
            if (string.IsNullOrEmpty(str))
            {
                id = default;
                return true;
            }
            else if (Guid.TryParse(str, out Guid guid))
            {
                id = guid == Guid.Empty ? null : (object)guid;
                return true;
            }
            else if (Uuid.Pattern.IsMatch(str))
            {
                var bytes = Convert.FromBase64String(str.Replace('-', '+').Replace('_', '/').Substring(0, 22) + "==");
                id = new Guid(bytes);

                if (Guid.Empty.Equals(id))
                {
                    id = null;
                }
                return true;
            }
            else if (str.Length == 26 && Base32.TryGetBytes(str, out var b32))
            {
                id = new Guid(b32);

                if (Guid.Empty.Equals(id))
                {
                    id = null;
                }
                return true;
            }
            else
            {
                id = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public override bool TryCreate(object obj, out object id)
        {
            if (obj is Guid guid)
            {
                id = guid == Guid.Empty ? null : (object)guid;
                return true;
            }

            else if (obj is Uuid uuid)
            {
                id = uuid == Uuid.Empty ? null : (object)(Guid)uuid;
                return true;
            }
            else if (obj is string str && TryParse(str, out id))
            {
                return true;
            }
            else
            {
                id = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public override object Next() => Guid.NewGuid();

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(Guid)
            || sourceType == typeof(Uuid)
            || base.CanConvertFrom(context, sourceType);

        private static Guid Id(object obj) => obj is Guid guid ? guid : Guid.Empty;

        private sealed class Default : GuidBehavior { }
    }
}
