using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="string"/>.</summary>
    public abstract class StringIdBehavior : IIdentifierBehavior
    {
        /// <summary>Returns the type of the underlying value (<see cref="string"/>).</summary>
        public Type BaseType => typeof(string);

        /// <summary>Returns a type converter for the type of the underlying value.</summary>
        public virtual TypeConverter Converter { get; } = TypeDescriptor.GetConverter(typeof(string));

        /// <inheritdoc/>
        public virtual int Compare(object x, object y) => string.Compare(Id(x), Id(y), StringComparison.InvariantCulture);

        /// <inheritdoc/>
        public virtual new bool Equals(object x, object y) => string.Equals(Id(x), Id(y), StringComparison.InvariantCulture);

        /// <inheritdoc/>
        public virtual int GetHashCode(object obj) => (Id(obj) ?? string.Empty).GetHashCode();

        /// <inheritdoc/>
        public virtual byte[] ToByteArray(object obj) => obj is string str ? Encoding.ASCII.GetBytes(str) : Array.Empty<byte>();

        /// <inheritdoc/>
        public virtual object FromBytes(byte[] bytes) => Encoding.ASCII.GetString(bytes);

        /// <inheritdoc/>
        public virtual string ToString(object obj, string format, IFormatProvider formatProvider) => Id(obj);

        /// <inheritdoc/>
        public virtual object FromJson(long obj) => obj.ToString(CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public virtual object ToJson(object obj) => Id(obj);

        /// <inheritdoc/>
        public virtual bool TryParse(string str, out object id)
        {
            id = default;

            if (IsValid(str, out var normalized))
            {
                id = normalized;
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public virtual bool TryCreate(object obj, out object id) => TryParse(obj?.ToString(), out id);

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

        /// <inheritdoc/>
        public virtual object Next() => throw new NotSupportedException();

        private static string Id(object obj) => obj is string str ? str : null;
    }
}
