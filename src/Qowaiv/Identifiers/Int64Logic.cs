using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierLogic"/> for an identifier based on <see cref="long"/>.</summary>
    public abstract class Int64Logic : IIdentifierLogic
    {
        /// <inheritdoc/>
        public virtual TypeConverter Converter { get; } = TypeDescriptor.GetConverter(typeof(long));

        /// <inheritdoc/>
        public virtual int Compare(object x, object y) => Id(x).CompareTo(Id(y));

        /// <inheritdoc/>
        public virtual new bool Equals(object x, object y) => Id(x).Equals(Id(y));

        /// <inheritdoc/>
        public virtual int GetHashCode(object obj) => Id(obj).GetHashCode();

        /// <inheritdoc/>
        public virtual string ToString(object obj, string format, IFormatProvider formatProvider) => Id(obj).ToString(format, formatProvider);

        /// <inheritdoc/>
        public virtual object ToJson(object obj) => Id(obj);

        /// <inheritdoc/>
        public virtual bool TryParse(string str, out object id)
        {
            id = default;

            if (long.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number) && number >= 0)
            {
                id = number == 0 ? null : (object)number;
                return true;
            }
            return false;
        }

        private static long Id(object obj) => obj is long number ? number : 0;
    }
}
