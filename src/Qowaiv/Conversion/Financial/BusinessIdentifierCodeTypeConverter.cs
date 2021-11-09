using Qowaiv.Financial;

namespace Qowaiv.Conversion.Financial;

/// <summary>Provides a conversion for a BIC.</summary>
public class BusinessIdentifierCodeTypeConverter : SvoTypeConverter<BusinessIdentifierCode>
{
    /// <inheritdoc/>
    [Pure]
    protected override BusinessIdentifierCode FromString(string str, CultureInfo culture) => BusinessIdentifierCode.Parse(str, culture);
}
