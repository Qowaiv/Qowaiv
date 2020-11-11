﻿using System;
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
    public abstract class SvoTypeConverter<TSvo> : TypeConverter
        where TSvo : struct, IFormattable
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
         => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => value is null || value is string
            ? FromString(value as string, culture)
            : base.ConvertFrom(context, culture, value);

        /// <summary>Converts from <see cref="string"/>.</summary>
        protected abstract TSvo FromString(string str, CultureInfo culture);
    }
}
