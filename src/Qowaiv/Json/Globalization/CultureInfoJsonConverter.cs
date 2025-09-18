#if NET6_0_OR_GREATER

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json.Globalization;

/// <summary>Implements a <see cref="JsonConverter"/> for <see cref="CultureInfo"/>.</summary>
public sealed class CultureInfoJsonConverter : JsonConverter<CultureInfo>
{
    /// <inheritdoc />
    public override CultureInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(reader.GetString()!);

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Name);

    /// <inheritdoc />
    public override CultureInfo ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Read(ref reader, typeToConvert, options);

    /// <inheritdoc />
    public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] CultureInfo value, JsonSerializerOptions options) => writer.WritePropertyName(value.Name);
}
#endif
