namespace Qowaiv.Locale.NL.Conversion;

/// <summary>Provides a conversion for a kadastrale gemeente.</summary>
[Inheritable]
public class KadastraleGemeenteTypeConverter : SvoTypeConverter<KadastraleGemeente>
{
    /// <inheritdoc />
    [Pure]
    protected override KadastraleGemeente FromString(string? str, CultureInfo? culture)
        => KadastraleGemeente.Parse(str);
}
