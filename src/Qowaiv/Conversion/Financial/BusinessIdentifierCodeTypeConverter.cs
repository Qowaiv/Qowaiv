using Qowaiv.Financial;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion.Financial
{
    /// <summary>Provides a conversion for a BIC.</summary>
    public class BusinessIdentifierCodeTypeConverter : SvoTypeConverter<BusinessIdentifierCode>
    {
        /// <inheritdoc/>
        [Pure]
        protected override BusinessIdentifierCode FromString(string str, CultureInfo culture) => BusinessIdentifierCode.Parse(str, culture);
    }
}
