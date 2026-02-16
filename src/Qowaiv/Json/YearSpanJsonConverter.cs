#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a year span.</summary>
public sealed class YearSpanJsonConverter : SvoJsonConverter<YearSpan>
{
    /// <inheritdoc />
    [Pure]
    protected override YearSpan FromJson(string? json) => YearSpan.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override YearSpan FromJson(long json) => YearSpan.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override YearSpan FromJson(double json) => YearSpan.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(YearSpan svo) => svo.ToJson();
}

#endif
