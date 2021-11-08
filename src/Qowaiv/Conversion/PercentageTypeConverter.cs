using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a Percentage.</summary>
    public class PercentageTypeConverter : NumericTypeConverter<Percentage, decimal>
    {
        /// <inheritdoc/>
        [Pure]
        protected override Percentage FromRaw(decimal raw) => Percentage.Create(raw);

        /// <inheritdoc/>
        [Pure]
        protected override Percentage FromString(string str, CultureInfo culture) => Percentage.Parse(str, culture);

        /// <inheritdoc/>
        [Pure]
        protected override decimal ToRaw(Percentage svo) => (decimal)svo;
    }
}
