namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for Single Value Objects.</summary>
/// <remarks>
/// The SVO Type Converter supports conversion from and to <see cref="string"/>.
/// Furthermore it support conversion from and to the underlying 'raw' type.
/// 
/// The conversion to Raw is handled by <see cref="ToRaw(TSvo)"/>,
/// for conversion from Raw is handled by <see cref="FromRaw(TRaw)"/>.
/// </remarks>
public abstract class SvoTypeConverter<TSvo, TRaw> : SvoTypeConverter<TSvo>
    where TSvo : struct, IFormattable
    where TRaw : struct, IFormattable
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(TRaw)
        || sourceType == typeof(TRaw?)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        => destinationType == typeof(TRaw)
        || destinationType == typeof(TRaw?)
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        => value is TRaw raw
        ? FromRaw(raw)
        : base.ConvertFrom(context, culture, value);

    /// <inheritdoc />
    [Pure]
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof(TRaw) || destinationType == typeof(TRaw?))
        {
            var svo = Guard.IsInstanceOf<TSvo>(value, nameof(value));
            return ToRaw(svo);
        }
        else return base.ConvertTo(context, culture, value, destinationType);
    }

    /// <summary>Converts from the raw/underlying type.</summary>
    [Pure]
    protected abstract TSvo FromRaw(TRaw raw);

    /// <summary>Converts to the raw/underlying type.</summary>
    [Pure]
    protected abstract TRaw ToRaw(TSvo svo);
}
