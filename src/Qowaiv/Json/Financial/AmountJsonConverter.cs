#if NET6_0_OR_GREATER

using Qowaiv.Financial;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for an amount.</summary>
[Inheritable]
public class AmountJsonConverter : SvoJsonConverter<Amount>
{
    /// <inheritdoc />
    [Pure]
    protected override Amount FromJson(string? json) => Amount.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Amount FromJson(long json) => Amount.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Amount FromJson(double json) => Amount.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Amount svo) => svo.ToJson();
}

#endif
