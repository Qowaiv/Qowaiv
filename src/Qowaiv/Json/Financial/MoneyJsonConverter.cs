#if NET10_0_OR_GREATER

using Qowaiv.Financial;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for money.</summary>
internal sealed class MoneyJsonConverter : SvoJsonConverter<Money>
{
    /// <inheritdoc />
    [Pure]
    protected override Money FromJson(string? json) => Money.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Money FromJson(long json) => Money.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Money FromJson(double json) => Money.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Money svo) => svo.ToJson();
}

#endif
