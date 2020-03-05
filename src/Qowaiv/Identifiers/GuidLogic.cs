using Qowaiv.Conversion;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierLogic"/> for an identifier based on <see cref="Guid"/>.</summary>
    /// <remarks>
    /// For some logic, <see cref="Uuid"/> is used instead of <see cref="Guid"/>.
    /// By doing so, Base64 representation is supported out of the box.
    /// </remarks>
    public abstract class GuidLogic : IIdentifierLogic
    {
        /// <summary>Returns the type of the underlying value (<see cref="Guid"/>).</summary>
        public Type BaseType => typeof(Guid);

        /// <inheritdoc/>
        public virtual TypeConverter Converter { get; } = new UuidTypeConverter();

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

        /// <inheritdoc/>
        public virtual string ToString(object obj, string format, IFormatProvider formatProvider)
        {
            var uuid = (Uuid)Id(obj);
            var f = string.IsNullOrEmpty(format) ? DefaultFormat : format;
            return uuid.ToString(f, formatProvider);
        }

        /// <inheritdoc/>
        public virtual object ToJson(object obj) => ToString(obj, DefaultFormat, CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public virtual bool TryParse(string str, out object id)
        {
            id = null;
            if (Uuid.TryParse(str, out var uuid))
            {
                id = uuid.IsEmpty() ? null : (object)(Guid)uuid;
                return true;
            }
            return false;
        }

        private static Guid Id(object obj) => obj is Guid guid ? guid : Guid.Empty;
    }
}
