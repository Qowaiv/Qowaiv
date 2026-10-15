#if NET10_0_OR_GREATER

using Qowaiv.Mathematics;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a percentage.</summary>
internal sealed class PercentageJsonConverter : JsonConverter<Percentage>
{
    /// <inheritdoc />
    public override Percentage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.String => ReadString(ref reader),
            JsonTokenType.Number => ReadNumber(ref reader),
            JsonTokenType.Null => default,
            _ => reader.UnexpectedToken<Percentage>(),
        };

    /// <inheritdoc />
    public override Percentage ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
        => ReadString(ref reader);

    private static Percentage ReadString(ref Utf8JsonReader reader)
    {
        var span = reader.ValueSpan;
        return true switch
        {
            _ when span[^1] is Symbol
                && Utf8Parser.TryParse(span[..^1], out decimal dec, out int bytesConsumed)
                && bytesConsumed == span.Length - 1 => dec.Percent(),

            _ when Percentage.TryParse(reader.GetString(), CultureInfo.InvariantCulture) is { } percentage
                => percentage,
            _ => throw new JsonException(QowaivMessages.FormatExceptionPercentage),
        };
    }

    private static Percentage ReadNumber(ref Utf8JsonReader reader)
        => reader.TryGetDecimal(out var dec)
        && dec >= (decimal)Percentage.MinValue && dec <= (decimal)Percentage.MaxValue
            ? Percentage.Create(dec)
            : throw new JsonException(QowaivMessages.ArgumentOutOfRange_Percentage);

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Percentage value, JsonSerializerOptions options)
        => Write(writer, value);

    // <inheritdoc />
    public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] Percentage value, JsonSerializerOptions options)
        => Write(writer, value);

    /// <remarks>
    /// Uses <see cref="Utf8JsonWriter.WriteRawValue(string, bool)"/>:
    /// Writes '"'
    /// Writes decimal (decimal point moved left 2 positions)
    /// Writes '%'
    /// Writes '"'.
    /// The buffer length:
    /// 29 bytes for decimal (precision)
    ///  2 bytes for quotes
    ///  1 byte  for percentage symbol
    ///  1 byte  for decimal separator
    ///  1 byte  for minus sign.
    /// </remarks>
    private static void Write(Utf8JsonWriter writer, Percentage value)
    {
        var dec = DecimalMath.ChangeScale((decimal)value, 2);

        Span<byte> buffer = stackalloc byte[34];
        buffer[0] = ASCII.Quote;
        Utf8Formatter.TryFormat(dec, buffer[1..], out var length);
        buffer[++length] = Symbol;
        buffer[++length] = ASCII.Quote;
        writer.WriteRawValue(buffer[..++length], skipInputValidation: true);
    }

    private const byte Symbol = (byte)'%';
}

#endif
