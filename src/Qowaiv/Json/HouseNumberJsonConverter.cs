#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a postal code.</summary>
public sealed class HouseNumberJsonConverter : SvoJsonConverter<HouseNumber>
{
    /// <inheritdoc />
    [Pure]
    protected override HouseNumber FromJson(string? json) => HouseNumber.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override HouseNumber FromJson(long json) => HouseNumber.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override HouseNumber FromJson(double json) => HouseNumber.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(HouseNumber svo) => svo.ToJson();
}

#endif
