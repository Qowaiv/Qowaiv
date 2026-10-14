#if NET10_0_OR_GREATER

using Qowaiv.Financial;
using Qowaiv.Globalization;
using System.Text.Json;

namespace Qowaiv.Json.Globalization;

/// <summary>Provides a JSON conversion for a country.</summary>
[Inheritable]
public class CountryJsonConverter : SvoJsonConverter<Country>
{
    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Country value, JsonSerializerOptions options)
        => writer.WriteAsciiString(value.ToJson());

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

