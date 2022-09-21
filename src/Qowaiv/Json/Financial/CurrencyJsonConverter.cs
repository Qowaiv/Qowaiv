#if NET5_0_OR_GREATER

using Qowaiv.Financial;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for a postal code.</summary>
public sealed class CurrencyJsonConverter : SvoJsonConverter<Currency>
{
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
