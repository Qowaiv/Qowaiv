#if NET10_0_OR_GREATER

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a date.</summary>
internal sealed class DateJsonConverter : JsonConverter<Date>
{
    /// <inheritdoc />
    public override Date Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.String => Read(ref reader),
            JsonTokenType.Null => default,
            _ => reader.UnexpectedToken<Date>(),
        };

    /// <inheritdoc />
    public override Date ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Read(ref reader);

    [Pure]
    private static Date Read(ref Utf8JsonReader reader)
        => reader.TryGetDateTime(out var date)
        ? (Date)date
        : throw new JsonException(QowaivMessages.FormatExceptionDate);

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Date value, JsonSerializerOptions options)
        => Write(writer, value);

    /// <inheritdoc />
    public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] Date value, JsonSerializerOptions options)
        => Write(writer, value);

    private static void Write(Utf8JsonWriter writer, Date value)
    {
        // yyyy-MM-dd
        Span<byte> buffer = stackalloc byte[10];
        ((DateTime)value).TryFormat(buffer, out _, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        writer.WriteStringValue(buffer);
    }
}

#endif
