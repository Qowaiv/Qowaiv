#if NET8_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a sex.</summary>
[Inheritable]
public class SexJsonConverter : SvoJsonConverter<Sex>
{
    /// <inheritdoc />
    [Pure]
    protected override Sex FromJson(string? json) => Sex.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Sex FromJson(long json) => Sex.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Sex FromJson(double json) => Sex.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Sex svo) => svo.ToJson();
}

#endif
