using System;
using System.ComponentModel;
using System.Text;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierLogic"/> for an identifier based on <see cref="string"/>.</summary>
    public abstract class StringIdLogic : IIdentifierLogic
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
