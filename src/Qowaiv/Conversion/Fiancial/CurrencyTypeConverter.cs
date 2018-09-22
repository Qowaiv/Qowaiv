using Qowaiv.Financial;
using System.Globalization;

namespace Qowaiv.Conversion.Financial
{
    /// <summary>Provides a conversion for a currency.</summary>
    public class CurrencyTypeConverter : SvoTypeConverter<Currency>
    {
        /// <inheritdoc/>
        protected override Currency FromString(string str, CultureInfo culture) => Currency.Parse(str, culture);
    }
}
