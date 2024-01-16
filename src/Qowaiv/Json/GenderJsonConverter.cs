#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a gender.</summary>
[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public sealed class GenderJsonConverter : SvoJsonConverter<Gender>
{
    /// <inheritdoc />
    [Pure]
    protected override Gender FromJson(string? json) => Gender.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Gender FromJson(long json) => Gender.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Gender FromJson(double json) => Gender.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Gender svo) => svo.ToJson();
}

#endif
