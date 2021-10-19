using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="string"/>.</summary>
    public abstract class StringIdBehavior : IdentifierBehavior
    {
        /// <summary>Returns the type of the underlying value (<see cref="string"/>).</summary>
        public sealed override Type BaseType => typeof(string);

        /// <inheritdoc/>
        public override int Compare(object x, object y) => string.Compare(Id(x), Id(y), StringComparison.InvariantCulture);

        /// <inheritdoc/>
        public override bool Equals(object x, object y) => string.Equals(Id(x), Id(y), StringComparison.InvariantCulture);

        /// <inheritdoc/>
        public override int GetHashCode(object obj) => (Id(obj) ?? string.Empty).GetHashCode();

        /// <inheritdoc/>
        public override byte[] ToByteArray(object obj) => obj is string str ? Encoding.ASCII.GetBytes(str) : Array.Empty<byte>();

        /// <inheritdoc/>
        public override object FromBytes(byte[] bytes) => Encoding.ASCII.GetString(bytes);

        /// <inheritdoc/>
        public override string ToString(object obj, string format, IFormatProvider formatProvider) => Id(obj);

        /// <inheritdoc/>
        public override object FromJson(long obj) => obj.ToString(CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public override object ToJson(object obj) => Id(obj);

        /// <inheritdoc/>
        public override bool TryParse(string str, out object id)
        {
            if (IsValid(str, out var normalized))
            {
                id = normalized;
                return true;
            }
            else
            {
                id = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public override bool TryCreate(object obj, out object id) => TryParse(obj?.ToString(), out id);

        /// <summary>Validates if the string matches the constrains.</summary>
        /// <param name="str">
        /// The string representing the identifier.
        /// </param>
        /// <param name="normalized">
        /// The normalized string representing the identifier.
        /// </param>
        /// <returns>
        /// True if valid.
        /// </returns>
        protected virtual bool IsValid(string str, out string normalized)
        {
            normalized = str;
            return true;
        }

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(Guid)
            || sourceType == typeof(Uuid)
            || sourceType == typeof(long)
            || sourceType == typeof(long)
            || sourceType == typeof(int)
            || base.CanConvertFrom(context, sourceType);

        private static string Id(object obj) => obj is string str ? str : null;
    }
}
