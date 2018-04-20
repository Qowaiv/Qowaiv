using Qowaiv.Reflection;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a Date types.</summary>
    public abstract class DateTypeConverter<T> : TypeConverter where T : struct, IFormattable
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
            if (value is string str)
            {
                return FromString(str, culture);
            }
            if(!(value is null) && IsConvertable(value.GetType()))
            {
                if(value is DateTime dateTime)
                {
                    return FromDateTime(dateTime);
                }
                if (value is DateTimeOffset offset)
                {
                    return FromDateTimeOffset(offset);
                }
                if (value is LocalDateTime local)
                {
                    return FromLocalDateTime(local);
                }
                if (value is Date date)
                {
                    return FromDate(date);
                }
                if (value is WeekDate weekDate)
                {
                    return FromWeekDate(weekDate);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
        #endregion

        #region Convert To

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType != typeof(T) && QowaivType.IsDate(destinationType)) ||
                base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(destinationType == typeof(string))
            {
                var typed = Guard.IsTypeOf<T>(value, nameof(value));
                return typed.ToString(string.Empty, culture);
            }
            if(!(destinationType is null) &&  IsConvertable(destinationType))
            {
                var date = Guard.IsTypeOf<T>(value, nameof(value));

                if (destinationType == typeof(DateTime ))
                {
                    return ToDateTime(date);
                }
                if (destinationType == typeof(DateTimeOffset))
                {
                    return ToDateTimeOffset(date);
                }
                if (destinationType == typeof(LocalDateTime))
                {
                    return ToLocalDateTime(date);
                }
                if (destinationType == typeof(Date))
                {
                    return ToDate(date);
                }
                if (destinationType == typeof(WeekDate))
                {
                    return ToWeekDate(date);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #region From's

        /// <summary>Converts from <see cref="string"/>.</summary>
        protected abstract T FromString(string str, CultureInfo culture);

        /// <summary>Converts from <see cref="DateTime"/>.</summary>
        protected abstract T FromDateTime(DateTime dateTime);

        /// <summary>Converts from <see cref="DateTimeOffset"/>.</summary>
        protected abstract T FromDateTimeOffset(DateTimeOffset offset);
        
        /// <summary>Converts from <see cref="LocalDateTime"/>.</summary>
        protected abstract T FromLocalDateTime(LocalDateTime local);

        /// <summary>Converts from <see cref="Date"/>.</summary>
        protected abstract T FromDate(Date date);

        /// <summary>Converts from <see cref="WeekDate"/>.</summary>
        protected abstract T FromWeekDate(WeekDate weekDate);

        #endregion

        #region To's

        /// <summary>Converts to <see cref="DateTime"/>.</summary>
        protected abstract DateTime ToDateTime(T date);

        /// <summary>Converts to <see cref="DateTimeOffset"/>.</summary>
        protected abstract DateTimeOffset ToDateTimeOffset(T date);

        /// <summary>Converts to <see cref="LocalDateTime"/>.</summary>
        protected abstract LocalDateTime ToLocalDateTime(T date);

        /// <summary>Converts to <see cref="Date"/>.</summary>
        protected abstract Date ToDate(T date);

        /// <summary>Converts to <see cref="WeekDate"/>.</summary>
        protected abstract WeekDate ToWeekDate(T date);

        #endregion

        private static bool IsConvertable(Type type)
        {
            return type != typeof(T) &&
                (type == typeof(string) || QowaivType.IsDate(type));
        }

        #endregion
    }
}
