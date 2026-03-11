#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a @FullName.</summary>
public sealed class @TSvoJsonConverter : SvoJsonConverter<@TSvo>
{
    /// <inheritdoc />
    [Pure]
    protected override @TSvo FromJson(string? json) => @TSvo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override @TSvo FromJson(long json) => @TSvo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override @TSvo FromJson(double json) => @TSvo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override @TSvo FromJson(bool json) => @TSvo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(@TSvo svo) => svo.ToJson();
}

#endif
