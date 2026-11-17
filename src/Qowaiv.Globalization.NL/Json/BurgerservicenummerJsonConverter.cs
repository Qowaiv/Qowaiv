#if !NETSTANDARD2_0

using Qowaiv.Json;
using System.Text.Json;

namespace Qowaiv.Globalization.NL.Json;

/// <summary>Provides a JSON conversion for a Burgerservicenummer.</summary>
public sealed class BurgerservicenummerJsonConverter : SvoJsonConverter<Burgerservicenummer>
{
    /// <inheritdoc />
    [Pure]
    protected override Burgerservicenummer FromJson(string? json) => Burgerservicenummer.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Burgerservicenummer FromJson(long json) => Burgerservicenummer.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Burgerservicenummer svo) => svo.ToJson();
}

#endif
