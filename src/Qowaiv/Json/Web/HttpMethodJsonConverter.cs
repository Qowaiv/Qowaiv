#if NET6_0_OR_GREATER

using Qowaiv.Web;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json.Web;

/// <summary>Provides a JSON conversion for an HTTP Method.</summary>
[Inheritable]
public class HttpMethodJsonConverter : JsonConverter<HttpMethod>
{
    /// <inheritdoc />
    [Pure]
    public override HttpMethod? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString() is { Length: > 0 } s
            ? HttpMethodParser.Parse(s)
            : null;

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, HttpMethod value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Method);
}
#endif
