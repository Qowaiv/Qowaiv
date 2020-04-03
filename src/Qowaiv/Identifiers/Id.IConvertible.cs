using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
            var tp = behavior.BaseType;

            if (tp == typeof(long)) { return TypeCode.Int64; }
            if (tp == typeof(int)) { return TypeCode.Int32; }

            return TypeCode.String;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convertable.ToType(conversionType, provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convertable.ToBoolean(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        byte IConvertible.ToByte(IFormatProvider provider) => Convertable.ToByte(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        char IConvertible.ToChar(IFormatProvider provider) => Convertable.ToChar(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convertable.ToDateTime(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convertable.ToDecimal(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        double IConvertible.ToDouble(IFormatProvider provider) => Convertable.ToDouble(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        short IConvertible.ToInt16(IFormatProvider provider) => Convertable.ToInt16(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        int IConvertible.ToInt32(IFormatProvider provider) => Convertable.ToInt32(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        long IConvertible.ToInt64(IFormatProvider provider) => Convertable.ToInt64(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convertable.ToSByte(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        float IConvertible.ToSingle(IFormatProvider provider) => Convertable.ToSingle(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convertable.ToUInt16(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convertable.ToUInt32(provider);
        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convertable.ToUInt64(provider);
    }
}
