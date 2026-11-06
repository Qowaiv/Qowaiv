#if NET10_0_OR_GREATER

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks.Dataflow;

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a yes-no.</summary>
internal sealed class YesNoJsonConverter : JsonConverter<YesNo>
{
    /// <inheritdoc />
    public override YesNo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.True => YesNo.Yes,
            JsonTokenType.False => YesNo.No,
            JsonTokenType.Null => default,
            JsonTokenType.String => ReadString(reader),
            JsonTokenType.Number => ReadNumber(reader),
            _ => reader.UnexpectedToken<YesNo>(),
        };

    public override YesNo ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => ReadString(reader);

    [Pure]
    private static YesNo ReadString(Utf8JsonReader reader) => true switch
    {
        _ when reader.IsUknown => YesNo.Unknown,
        _ when reader.ValueSpan.SequenceEqual("yes"u8) => YesNo.Yes,
        _ when reader.ValueSpan.SequenceEqual("no"u8) => YesNo.No,
        _ when YesNo.TryParse(reader.GetString(), CultureInfo.InvariantCulture, out var yn) => yn,
        _ => throw new JsonException(QowaivMessages.FormatExceptionYesNo),
    };

    [Pure]
    private static YesNo ReadNumber(Utf8JsonReader reader)
    {
        var succes = reader.TryGetInt64(out var number);

        if (!succes && reader.TryGetDecimal(out var dec) && dec is 0 or 1)
        {
            succes = true;
            number = (long)dec;
        }

        return number switch
        {
            0 when succes => YesNo.No,
            1 => YesNo.Yes,
            byte.MaxValue or
            short.MaxValue or
            int.MaxValue or
            long.MaxValue => YesNo.Unknown,
            _ => throw new JsonException(QowaivMessages.FormatExceptionYesNo),
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, YesNo value, JsonSerializerOptions options)
        => Write(writer, value);

    /// <inheritdoc />
    public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] YesNo value, JsonSerializerOptions options)
        => Write(writer, value);

    private static void Write(Utf8JsonWriter writer, YesNo value)
        => writer.WriteRawValue(value.ToUtf8Json(), skipInputValidation: true);
}

#endif
