#if NET8_0_OR_GREATER

using Qowaiv.Financial;
using System.Text.Json;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for an IBAN.</summary>
[Inheritable]
public class InternationalBankAccountNumberJsonConverter : SvoJsonConverter<InternationalBankAccountNumber>
{
    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, InternationalBankAccountNumber value, JsonSerializerOptions options)
    {
        if (value.ToJson() is { } json)
        {
            writer.WriteAsciiString(json);
        }
        else
        {
            writer.WriteNullValue();
        }
    }

    /// <inheritdoc />
    [Pure]
    protected override InternationalBankAccountNumber FromJson(string? json) => InternationalBankAccountNumber.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(InternationalBankAccountNumber svo) => svo.ToJson();
}

#endif
