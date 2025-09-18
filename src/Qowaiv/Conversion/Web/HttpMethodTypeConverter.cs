using Qowaiv.Web;

namespace Qowaiv.Conversion.Web;

/// <summary>Provides a conversion for an HTTP method.</summary>
public sealed class HttpMethodTypeConverter : TypeConverter
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
        => value is null || value is string
        ? HttpMethodParser.Parse(value as string)
        : base.ConvertFrom(context, culture, value);
}
