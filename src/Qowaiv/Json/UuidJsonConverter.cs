#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a UUID.</summary>
public sealed class UuidJsonConverter : SvoJsonConverter<Uuid>
{
    /// <inheritdoc />
    [Pure]
    protected override Uuid FromJson(string? json) => Uuid.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Uuid svo) => svo.ToJson();
}

#endif
