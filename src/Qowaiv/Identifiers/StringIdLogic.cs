using System;
using System.ComponentModel;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierLogic"/> for an identifier based on <see cref="string"/>.</summary>
    public abstract class StringIdLogic : IIdentifierLogic
    {
        /// <inheritdoc/>
        public virtual TypeConverter Converter { get; } = TypeDescriptor.GetConverter(typeof(string));

        /// <inheritdoc/>
        public virtual int Compare(object x, object y) => string.Compare(Id(x), Id(y), StringComparison.InvariantCulture);

        /// <inheritdoc/>
        public virtual new bool Equals(object x, object y) => string.Equals(Id(x), Id(y), StringComparison.InvariantCulture);

        /// <inheritdoc/>
        public virtual int GetHashCode(object obj) => (Id(obj) ?? string.Empty).GetHashCode();

        /// <inheritdoc/>
        public virtual string ToString(object obj, string format, IFormatProvider formatProvider) => Id(obj);

        /// <inheritdoc/>
        public virtual object ToJson(object obj) => Id(obj);

        /// <inheritdoc/>
        public virtual bool TryParse(string str, out object id)
        {
            id = str;
            return true;
        }

        private static string Id(object obj) => obj is string str ? str : null;
    }
}
