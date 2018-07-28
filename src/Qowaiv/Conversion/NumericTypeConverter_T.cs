using Qowaiv.Reflection;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for numeric types.</summary>
    public abstract class NumericTypeConverter<Tsvo, T> : TypeConverter
        where Tsvo : struct, IFormattable
        where T: struct, IFormattable
    {
        #region Convert From

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return IsConvertable(sourceType) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is null)
            {
                return Activator.CreateInstance<Tsvo>();
            }
            if (value is string str)
            {
                return FromString(str, culture);
            }
            if(IsConvertable(value.GetType()))
            {
                var raw = (T)Convert.ChangeType(value, typeof(T));
                return FromRaw(raw);
            }
            return base.ConvertFrom(context, culture, value);
        }

        #endregion

        #region Convert To

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType != typeof(Tsvo) && QowaivType.IsDate(destinationType)) ||
                base.CanConvertTo(context, destinationType);
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

            if (destinationType == typeof(string))
            {
                var typed = Guard.IsTypeOf<Tsvo>(value, nameof(value));
                return typed.ToString(string.Empty, culture);
            }

            if(IsConvertable(destinationType))
            {
                var typed = Guard.IsTypeOf<Tsvo>(value, nameof(value));
                var raw = ToRaw(typed);
                return Convert.ChangeType(raw, QowaivType.GetNotNullableType(destinationType));
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion

        /// <summary>Converts from <see cref="string"/>.</summary>
        protected abstract Tsvo FromString(string str, CultureInfo culture);
        /// <summary>Converts from the raw/underlying type.</summary>
        protected abstract Tsvo FromRaw(T raw);
        /// <summary>Converts to the raw/underlying type.</summary>
        protected abstract T ToRaw(Tsvo svo);

        /// <summary>Returns true if the conversion is supported.</summary>
        protected virtual bool IsConvertable(Type type)
        {
            return type != typeof(Tsvo) &&
                (type == typeof(string) || QowaivType.IsNumeric(type));
        }
    }
}
