using System.Net.Http;

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
        ? FromString(value as string)
        : base.ConvertFrom(context, culture, value);

#if NET8_0_OR_GREATER
    /// <summary>Converts from <see cref="string" />.</summary>
    [Pure]
    private static HttpMethod? FromString(string? str)
        => str is { Length: > 0 }
        ? HttpMethod.Parse(str)
        : null;
#else
    /// <summary>Converts from <see cref="string" />.</summary>
    [Pure]
    private static HttpMethod? FromString(string? str) => str?.ToUpperInvariant() switch
    {
        "Delete" => HttpMethod.Delete,
        "Get" => HttpMethod.Get,
        "Head" => HttpMethod.Head,
        "Options" => HttpMethod.Options,
        "Post" => HttpMethod.Post,
        "Put" => HttpMethod.Put,
        "Trace" => HttpMethod.Trace,
        { Length: > 0 } => new(str),
        _ => null,
    };
#endif
}
