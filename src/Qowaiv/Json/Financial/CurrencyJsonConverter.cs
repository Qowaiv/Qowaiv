#if NET10_0_OR_GREATER

using Qowaiv.Financial;
using System.Text.Json;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for a currency.</summary>
internal sealed class CurrencyJsonConverter : SvoJsonConverter<Currency>
{
    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Currency value, JsonSerializerOptions options)
        => writer.WriteAsciiString(value.ToJson());

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
