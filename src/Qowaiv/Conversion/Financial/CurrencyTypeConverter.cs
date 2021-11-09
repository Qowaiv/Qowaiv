using Qowaiv.Financial;

namespace Qowaiv.Conversion.Financial;

/// <summary>Provides a conversion for a currency.</summary>
public class CurrencyTypeConverter : SvoTypeConverter<Currency>
{
    /// <inheritdoc/>
    [Pure]
    protected override Currency FromString(string str, CultureInfo culture) => Currency.Parse(str, culture);
}

