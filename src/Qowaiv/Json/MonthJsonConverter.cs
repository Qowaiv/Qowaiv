#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a month.</summary>
[Inheritable]
public class MonthJsonConverter : SvoJsonConverter<Month>
{
    /// <inheritdoc />
    [Pure]
    protected override Month FromJson(string? json) => Month.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Month FromJson(long json) => Month.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Month FromJson(double json) => Month.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Month svo) => svo.ToJson();
}

#endif
