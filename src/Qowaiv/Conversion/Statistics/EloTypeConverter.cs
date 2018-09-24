using Qowaiv.Statistics;
using System.Globalization;

namespace Qowaiv.Conversion.Statistics
{
    /// <summary>Provides a conversion for an Elo.</summary>
    public class EloTypeConverter : NumericTypeConverter<Elo, double>
    {
        /// <inheritdoc/>
        protected override Elo FromRaw(double raw) => raw;

        /// <inheritdoc/>
        protected override Elo FromString(string str, CultureInfo culture) => Elo.Parse(str, culture);

        /// <inheritdoc/>
        protected override double ToRaw(Elo svo) => (double)svo;
    }
}
