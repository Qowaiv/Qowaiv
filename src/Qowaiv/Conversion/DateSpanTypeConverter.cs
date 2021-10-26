using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a date span.</summary>
    public class DateSpanTypeConverter : TypeConverter
    {
        /// <summary>Returns whether this converter can convert an string to
        /// a date span, using the specified context.
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
        [Pure]
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        /// <summary>Converts a string to a date span, using the specified
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
        [Pure]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (value is null || str != null)
            {
                return DateSpan.Parse(str, culture);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
