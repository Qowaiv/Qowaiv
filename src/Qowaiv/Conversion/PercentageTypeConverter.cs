using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a Percentage.</summary>
    public class PercentageTypeConverter : NumericTypeConverter<Percentage, decimal>
    {
        /// <inheritdoc/>
        protected override Percentage FromRaw(decimal raw) => raw;

        /// <inheritdoc/>
        protected override Percentage FromString(string str, CultureInfo culture) => Percentage.Parse(str, culture);

        /// <inheritdoc/>
        protected override decimal ToRaw(Percentage svo) => (decimal)svo;
    }
}
