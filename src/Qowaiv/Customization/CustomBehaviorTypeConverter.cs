namespace Qowaiv.Customization;

public abstract class CustomBehaviorTypeConverter<TSvo, TRaw, TConverter> : TypeConverter
    where TSvo : struct
    where TConverter : TypeConverter, new()
{
    private static readonly TConverter Converter = new();

    /// <inheritdoc />
    [Pure]
    public sealed override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => Converter.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public sealed override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => Converter.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public sealed override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => FromRaw(Guard.IsInstanceOf<TRaw>(Converter.ConvertFrom(context, culture, value)));

    /// <inheritdoc />
    [Pure]
    public sealed override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => Converter.ConvertTo(context, culture, ToRaw(Guard.IsInstanceOf<TSvo>(value)), destinationType);

    /// <summary>Converts from the raw/underlying type.</summary>
    [Pure]
    protected abstract TSvo FromRaw(TRaw raw);

    /// <summary>Converts to the raw/underlying type.</summary>
    [Pure]
    protected abstract TRaw ToRaw(TSvo svo);
}
