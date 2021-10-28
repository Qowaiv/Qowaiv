using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="long"/>.</summary>
    public abstract class Int64IdBehavior : IdentifierBehavior
    {
        /// <summary>Returns the type of the underlying value (<see cref="long"/>).</summary>
        public sealed override Type BaseType => typeof(long);

        /// <inheritdoc/>
        [Pure]
        public override int Compare(object x, object y) => Id(x).CompareTo(Id(y));

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object x, object y) => Id(x).Equals(Id(y));

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode(object obj) => Id(obj).GetHashCode();

        /// <inheritdoc/>
        [Pure]
        public override byte[] ToByteArray(object obj)
            => obj is long num 
            ? BitConverter.GetBytes(num) 
            : Array.Empty<byte>();

        /// <inheritdoc/>
        [Pure]
        public override object FromBytes(byte[] bytes)
            => BitConverter.ToInt64(bytes, 0);

        /// <inheritdoc/>
        [Pure]
        public override string ToString(object obj, string format, IFormatProvider formatProvider)
            => Id(obj).ToString(format, formatProvider);

        /// <inheritdoc/>
        [Pure]
        public sealed override object FromJson(long obj)
        {
            if (obj == 0) return null;
            else if (obj > 0) return obj;
            else throw Exceptions.InvalidCast(typeof(long), typeof(Id<>).MakeGenericType(GetType()));
        }

        /// <inheritdoc/>
        [Pure]
        public override object ToJson(object obj) => Id(obj);

        /// <inheritdoc/>
        public override bool TryParse(string str, out object id)
        {
            if (long.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number) && number >= 0)
            {
                id = number == 0 ? null : (object)number;
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
            if (obj is long num)
            {
                id = num == 0L ? null : (object)num;
                return true;
            }
            else if (obj is int n)
            {
                id = n == 0 ? null : (object)(long)n;
                return true;
            }
            else if (TryParse(obj?.ToString(), out id))
            {
                return true;
            }
            else return false;
        }

        /// <inheritdoc />
        [Pure]
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(long)
            || sourceType == typeof(int)
            || base.CanConvertFrom(context, sourceType);

        [Pure]
        private static long Id(object obj) => obj is long number ? number : 0;
    }
}
