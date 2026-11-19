namespace Qowaiv.Globalization.NL.Conversion;

/// <summary>Provides a conversion for a Burgerservicenummer.</summary>
[Inheritable]
public class KadastraleGemeenteTypeConverter : SvoTypeConverter<KadastraleGemeente>
{
    /// <inheritdoc />
    protected override KadastraleGemeente FromString(string? str, CultureInfo? culture)
        => KadastraleGemeente.Parse(str);
}
