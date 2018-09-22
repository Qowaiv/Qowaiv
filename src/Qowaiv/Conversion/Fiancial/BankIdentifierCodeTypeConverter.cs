using Qowaiv.Financial;
using System.Globalization;

namespace Qowaiv.Conversion.Financial
{
    /// <summary>Provides a conversion for a BIC.</summary>
    public class BankIdentifierCodeTypeConverter : SvoTypeConverter<BankIdentifierCode>
    {
        /// <inheritdoc/>
        protected override BankIdentifierCode FromString(string str, CultureInfo culture) => BankIdentifierCode.Parse(str, culture);
    }
}
