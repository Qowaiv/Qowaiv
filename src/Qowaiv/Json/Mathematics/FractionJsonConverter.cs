#if NET6_0_OR_GREATER

using Qowaiv.Mathematics;

namespace Qowaiv.Json.Mathematics;

/// <summary>Provides a JSON conversion for a fraction.</summary>
[Inheritable]
public class FractionJsonConverter : SvoJsonConverter<Fraction>
{
    /// <inheritdoc />
    [Pure]
    protected override Fraction FromJson(string? json) => Fraction.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Fraction FromJson(long json) => Fraction.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Fraction FromJson(decimal json) => Fraction.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Fraction FromJson(double json) => Fraction.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Fraction svo) => svo.ToJson();
}

#endif
