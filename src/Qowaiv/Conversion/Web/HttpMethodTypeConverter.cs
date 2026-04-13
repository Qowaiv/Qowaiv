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
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) => value switch
    {
        null => null,
        string str => HttpMethodParser.Parse(str),
        _ => base.ConvertFrom(context, culture, value),
    };
}
