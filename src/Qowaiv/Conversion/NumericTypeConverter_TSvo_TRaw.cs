using Qowaiv.Reflection;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for numeric types.</summary>
    public abstract class NumericTypeConverter<TSvo, TRaw> : SvoTypeConverter<TSvo, TRaw>
        where TSvo : struct, IFormattable
        where TRaw : struct, IFormattable
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return IsConvertable(sourceType) || base.CanConvertFrom(context, sourceType);
        }
        
        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return IsConvertable(destinationType) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is null)
            {
                return Activator.CreateInstance<TSvo>();
            }
            if(IsConvertable(value.GetType()))
            {
                var raw = (TRaw)Convert.ChangeType(value, typeof(TRaw));
                return FromRaw(raw);
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Guard.NotNull(destinationType, nameof(destinationType));

            // If the value is null or default value.
            if (QowaivType.IsNullOrDefaultValue(value))
            {
                return QowaivType.IsNullable(destinationType) ? null : Activator.CreateInstance(destinationType);
            }

            if(IsConvertable(destinationType))
            {
                var svo = Guard.IsTypeOf<TSvo>(value, nameof(value));
                var raw = ToRaw(svo);
                return Convert.ChangeType(raw, QowaivType.GetNotNullableType(destinationType));
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>Returns true if the conversion is supported.</summary>
        protected virtual bool IsConvertable(Type type) => type != typeof(TSvo) && QowaivType.IsNumeric(type);
    }
}
