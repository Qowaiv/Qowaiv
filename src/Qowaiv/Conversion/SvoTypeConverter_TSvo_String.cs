using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for Single Value Objects.</summary>
    /// <remarks>
    /// The SVO Type Converter supports conversion from and to <see cref="string"/>.
    /// 
    /// The conversion to <see cref="string"/> is handled by its base class,
    /// for conversion from <see cref="string"/> the <see cref="FromString(string, CultureInfo)"/>
    /// method has to be implemented.
    /// </remarks>
    public abstract class SvoTypeConverter<TSvo> : TypeConverter where TSvo : struct
    {
        #region Convert From

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (value is null || str != null)
            {
                return FromString(str, culture);
            }
            return base.ConvertFrom(context, culture, value);
        }

        #endregion

        /// <summary>Converts from <see cref="string"/>.</summary>
        protected abstract TSvo FromString(string str, CultureInfo culture);
    }
}
