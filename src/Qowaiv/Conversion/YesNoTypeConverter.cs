namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a Yes-no.</summary>
[Inheritable]
public class YesNoTypeConverter : SvoTypeConverter<YesNo>
{
    /// <inheritdoc/>
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(bool)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc/>
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(bool)
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc/>
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) => value switch
    {
        null => YesNo.Empty,
        bool boolean => (YesNo)boolean,
        _ => base.ConvertFrom(context, culture, value),
    };

    /// <inheritdoc/>
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => destinationType == typeof(bool) && value is YesNo yesNo
        ? (bool)yesNo
        : base.ConvertTo(context, culture, value, destinationType);

    /// <inheritdoc/>
    [Pure]
    protected override YesNo FromString(string? str, CultureInfo? culture) => YesNo.Parse(str, culture);
}
