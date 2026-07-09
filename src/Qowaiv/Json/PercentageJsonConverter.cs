#if NET8_0_OR_GREATER

using Qowaiv.Mathematics;
using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a percentage.</summary>
[Inheritable]
public class PercentageJsonConverter : SvoJsonConverter<Percentage>
{
    /// <inheritdoc />
    [Pure]
    protected override Percentage FromJson(string? json) => Percentage.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Percentage FromJson(long json) => Percentage.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Percentage FromJson(double json) => Percentage.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Percentage svo) => svo.ToJson();

    /// <inheritdoc />
    public override Percentage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var span = reader.ValueSpan;
            if (span[^1] is Sign
                && Utf8Parser.TryParse(span[..^1], out decimal dec, out int bytesConsumed)
                && bytesConsumed == span.Length - 1)
            {
                return dec.Percent();
            }
        }

        return base.Read(ref reader, typeToConvert, options);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Uses <see cref="Utf8JsonWriter.WriteRawValue(string, bool)"/>:
    /// Writes '"'
    /// Writes decimal (decimal point moved left 2 positions)
    /// Writes '%'
    /// Writes '"'.
    /// The buffer length:
    /// 29 bytes for decimal (precision)
    ///  2 bytes for quotes
    ///  1 byte  for percentage sign
    ///  1 byte  for decimal seperator
    ///  1 byte  for minius sign.
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, Percentage value, JsonSerializerOptions options)
    {
        Span<byte> buffer = stackalloc byte[34];
        buffer[0] = Quote;
        var dec = DecimalMath.ChangeScale((decimal)value, 2);
        Utf8Formatter.TryFormat(dec, buffer[1..], out var length);
        length++;
        buffer[length++] = Sign;
        buffer[length++] = Quote;
        writer.WriteRawValue(buffer[..length], true);
    }

    private const byte Quote = (byte)'"';
    private const byte Sign = (byte)'%';
}

#endif
