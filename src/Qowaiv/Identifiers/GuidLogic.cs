using Qowaiv.Conversion;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierLogic"/> for an identifier based on <see cref="Guid"/>.</summary>
    public abstract class GuidLogic : IIdentifierLogic
    {
        /// <inheritdoc/>
        public virtual TypeConverter Converter { get; } = new UuidTypeConverter();

        /// <summary>Gets the default format used to represent the <see cref="Guid"/> as <see cref="string"/>.</summary>
        protected virtual string DefaultFormat => "D";

        /// <inheritdoc/>
        public virtual int Compare(object x, object y) => Id(x).CompareTo(Id(y));

        /// <inheritdoc/>
        public virtual new bool Equals(object x, object y) => Id(x).Equals(Id(y));

        /// <inheritdoc/>
        public virtual int GetHashCode(object obj) => Id(obj).GetHashCode();

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
