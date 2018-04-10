using Qowaiv.Reflection;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a house number.</summary>
    public class HouseNumberTypeConverter : TypeConverter
    {
        #region Convert From

        /// <summary>Returns whether this converter can convert a string or a number to
        /// a house number, using the specified context.
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that provides a format context.
        /// </param>
        /// <param name="sourceType">
        /// A System.Type that represents the type you want to convert from.
        /// </param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return 
                sourceType == typeof(string) ||
                QowaivType.IsNumeric(sourceType) ||
                base.CanConvertFrom(context, sourceType);
        }
        /// <summary>Converts a string or a numeric to a house number, using the specified
        /// context and culture information.
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that provides a format context.
        /// </param>
        /// <param name="culture">
        /// The System.Globalization.CultureInfo to use as the current culture.
        /// </param>
        /// <param name="value">
        /// The System.Object to convert.
        /// </param>
        /// <returns>
        /// An System.Object that represents the converted value.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// The conversion cannot be performed.
        /// </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (value == null || str != null)
            {
                return HouseNumber.Parse(str, culture);
            }
            if(QowaivType.IsNumeric(value.GetType()))
            {
                return value is null ? HouseNumber.Empty : HouseNumber.Create(Convert.ToInt32(value));
            }
            return base.ConvertFrom(context, culture, value);
        }
        #endregion

        #region Convert To

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return QowaivType.IsNumeric(destinationType) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>Converts a house number to string or number, using the specified context and culture information.</summary>
        /// <param name="culture">
        /// A System.Globalization.CultureInfo. If null is passed, the current culture is assumed.
        /// </param>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that provides a format context.
        /// </param>
        /// <param name="value">
        /// The house number to convert.
        /// </param>
        /// <param name="destinationType">
        /// The System.Type to convert the value parameter to.
        /// </param>
        /// <returns>
        /// A string that represents the converted house number.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The destinationType parameter is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The conversion cannot be performed.
        /// </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(QowaivType.IsNumeric(destinationType))
            {
                var typed = Guard.IsTypeOf<HouseNumber>(value, nameof(value));
                var number = (int)typed;

                return number == 0 && QowaivType.IsNullable(destinationType) ? null : Convert.ChangeType(number, destinationType);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion
    }
}
