using Qowaiv.Financial;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion.Financial
{
    /// <summary>Provides a conversion for Money.</summary>
    public class MoneyTypeConverter : SvoTypeConverter<Money>
    {
        /// <inheritdoc/>
        [Pure]
        protected override Money FromString(string str, CultureInfo culture) => Money.Parse(str, culture);
    }
}
