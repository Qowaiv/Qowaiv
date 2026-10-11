#if NET8_0_OR_GREATER

using Qowaiv.Financial;
using System.Text.Json;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for a currency.</summary>
[Inheritable]
public class CurrencyJsonConverter : SvoJsonConverter<Currency>
{
    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Currency value, JsonSerializerOptions options)
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
    protected override Currency FromJson(string? json) => Currency.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Currency FromJson(long json) => Currency.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Currency svo) => svo.ToJson();
}

#endif
