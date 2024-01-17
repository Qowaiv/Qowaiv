#if NET6_0_OR_GREATER

namespace Qowaiv.Sql.Json;

/// <summary>Provides a JSON conversion for a timestamp.</summary>
public sealed class TimestampJsonConverter : Qowaiv.Json.SvoJsonConverter<Timestamp>
{
    /// <inheritdoc />
    [Pure]
    protected override Timestamp FromJson(string? json) => Timestamp.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Timestamp FromJson(long json) => Timestamp.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Timestamp FromJson(double json) => Timestamp.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Timestamp svo) => svo.ToJson();
}

#endif
