#if !NETSTANDARD2_0

using Qowaiv.Json;

namespace Qowaiv.Locale.NL.Json;

/// <summary>Provides a JSON conversion for a BAG-identificatienummer.</summary>
public sealed class BagIdJsonConverter : SvoJsonConverter<BagId>
{
    /// <inheritdoc />
    [Pure]
    protected override BagId FromJson(string? json) => BagId.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override BagId FromJson(long json) => BagId.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override BagId FromJson(double json) => BagId.Create((long)json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(BagId svo) => svo.ToJson();
}

#endif
