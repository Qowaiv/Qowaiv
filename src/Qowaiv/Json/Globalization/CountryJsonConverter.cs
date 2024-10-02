#if NET6_0_OR_GREATER

using Qowaiv.Globalization;

namespace Qowaiv.Json.Globalization;

/// <summary>Provides a JSON conversion for a country.</summary>
[Inheritable]
public class CountryJsonConverter : SvoJsonConverter<Country>
{
    /// <inheritdoc />
    [Pure]
    protected override Country FromJson(string? json) => Country.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Country FromJson(long json) => Country.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Country svo) => svo.ToJson();
}

#endif

