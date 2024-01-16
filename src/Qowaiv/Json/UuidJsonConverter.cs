#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a UUID.</summary>
[Inheritable]
public class UuidJsonConverter : SvoJsonConverter<Uuid>
{
    /// <inheritdoc />
    [Pure]
    protected override Uuid FromJson(string? json) => Uuid.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Uuid svo) => svo.ToJson();
}

#endif
