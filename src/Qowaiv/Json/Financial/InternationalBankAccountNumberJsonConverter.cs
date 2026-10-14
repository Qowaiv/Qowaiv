#if NET10_0_OR_GREATER

using Qowaiv.Financial;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for an IBAN.</summary>
internal sealed class InternationalBankAccountNumberJsonConverter : JsonConverter<InternationalBankAccountNumber>
{
    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, InternationalBankAccountNumber value, JsonSerializerOptions options)
        => writer.WriteAsciiString(value.ToJson());

    /// <inheritdoc />
    public override InternationalBankAccountNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.Null => default,
            JsonTokenType.String => Read(ref reader),
            _ => reader.UnexpectedToken<InternationalBankAccountNumber>(),
        };

    /// <inheritdoc />
    public override InternationalBankAccountNumber ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Read(ref reader);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static InternationalBankAccountNumber Read(ref Utf8JsonReader reader)
        => InternationalBankAccountNumber.TryParse(reader.GetString(), null, out var iban)
        ? iban
        : throw new JsonException(QowaivMessages.FormatExceptionInternationalBankAccountNumber);
}

#endif
