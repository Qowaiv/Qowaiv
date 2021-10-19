using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Identifiers
{
    /// <summary>Base implementation for
    /// <see cref="GuidBehavior"/>,
    /// <see cref="Int32IdBehavior"/>,
    /// <see cref="Int64IdBehavior"/>, and
    /// <see cref="StringIdBehavior"/>
    /// solving the type conversion part.
    /// </summary>
    public abstract class IdentifierBehavior : TypeConverter, IIdentifierBehavior
    {
        /// <inheritdoc />
        public virtual TypeConverter Converter => this;

        /// <inheritdoc/>
        public abstract Type BaseType { get; }

        /// <inheritdoc/>
        public abstract int Compare(object x, object y);

        /// <inheritdoc/>
        public new abstract bool Equals(object x, object y);

        /// <inheritdoc/>
        public abstract object FromBytes(byte[] bytes);

        /// <inheritdoc/>
        public virtual object FromJson(long obj) => throw new NotSupportedException();

        /// <inheritdoc/>
        public abstract int GetHashCode(object obj);

        /// <inheritdoc/>
        public virtual object Next() => throw new NotSupportedException();

        /// <inheritdoc/>
        public abstract byte[] ToByteArray(object obj);

        /// <inheritdoc/>
        public abstract object ToJson(object obj);

        /// <inheritdoc/>
        public abstract string ToString(object obj, string format, IFormatProvider formatProvider);

        /// <inheritdoc/>
        public abstract bool TryCreate(object obj, out object id);

        /// <inheritdoc/>
        public abstract bool TryParse(string str, out object id);

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        /// <inheritdoc />
        public sealed override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => TryCreate(value, out var result)
            ? result : base.ConvertFrom(context, culture, value);
    }
}
