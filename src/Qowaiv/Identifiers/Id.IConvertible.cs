using System;
using System.Diagnostics;
using System.Globalization;

namespace Qowaiv.Identifiers
{
    public partial struct Id<TIdentifier> : IConvertible
    {
        /// <summary>Represents the underlying value as <see cref="IConvertible"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IConvertible Convertable
        {
            get
            {
                var code = ((IConvertible)this).GetTypeCode();
                return code switch
                {
                    TypeCode.Int64 => IsEmpty() ? 0 : (long)m_Value,
                    TypeCode.Int32 => IsEmpty() ? 0 : (int)m_Value,

                    _ => ToString(CultureInfo.InvariantCulture),
                };
            }
        }

        /// <inheritdoc/>
        TypeCode IConvertible.GetTypeCode()
        {
            var tp = logic.BaseType;

            if (tp == typeof(long)) { return TypeCode.Int64; }
            if (tp == typeof(int)) { return TypeCode.Int32; }

            return TypeCode.String;
        }

        /// <inheritdoc/>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convertable.ToType(conversionType, provider);
        /// <inheritdoc/>
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convertable.ToBoolean(provider);
        /// <inheritdoc/>
        byte IConvertible.ToByte(IFormatProvider provider) => Convertable.ToByte(provider);
        /// <inheritdoc/>
        char IConvertible.ToChar(IFormatProvider provider) => Convertable.ToChar(provider);
        /// <inheritdoc/>
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convertable.ToDateTime(provider);
        /// <inheritdoc/>
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convertable.ToDecimal(provider);
        /// <inheritdoc/>
        double IConvertible.ToDouble(IFormatProvider provider) => Convertable.ToDouble(provider);
        /// <inheritdoc/>
        short IConvertible.ToInt16(IFormatProvider provider) => Convertable.ToInt16(provider);
        /// <inheritdoc/>
        int IConvertible.ToInt32(IFormatProvider provider) => Convertable.ToInt32(provider);
        /// <inheritdoc/>
        long IConvertible.ToInt64(IFormatProvider provider) => Convertable.ToInt64(provider);
        /// <inheritdoc/>
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convertable.ToSByte(provider);
        /// <inheritdoc/>
        float IConvertible.ToSingle(IFormatProvider provider) => Convertable.ToSingle(provider);
        /// <inheritdoc/>
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convertable.ToUInt16(provider);
        /// <inheritdoc/>
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convertable.ToUInt32(provider);
        /// <inheritdoc/>
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convertable.ToUInt64(provider);
    }
}
