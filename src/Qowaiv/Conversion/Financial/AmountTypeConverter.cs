using Qowaiv.Financial;
using System.Globalization;

namespace Qowaiv.Conversion.Financial
{
    /// <summary>Provides a conversion for an Amount.</summary>
    public class AmountTypeConverter : NumericTypeConverter<Amount, decimal>
    {
        /// <inheritdoc/>
        protected override Amount FromRaw(decimal raw) => (Amount)raw;

        /// <inheritdoc/>
        protected override Amount FromString(string str, CultureInfo culture) => Amount.Parse(str, culture);

        /// <inheritdoc/>
        protected override decimal ToRaw(Amount svo) => (decimal)svo;
    }
}
