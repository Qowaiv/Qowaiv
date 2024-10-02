using Qowaiv.Globalization;

namespace Qowaiv.Conversion.Globalization;

/// <summary>Provides a conversion for a Country.</summary>
[Inheritable]
public class CountryTypeConverter : SvoTypeConverter<Country>
{
    /// <inheritdoc/>
    [Pure]
    protected override Country FromString(string? str, CultureInfo? culture) => Country.Parse(str, culture);
}
