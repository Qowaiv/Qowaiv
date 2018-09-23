using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a year.</summary>
    public class YearTypeConverter : NumericTypeConverter<Year, int>
    {
        /// <inheritdoc/>
        protected override Year FromRaw(int raw) => Year.Create(raw == 0 ? null : (int?)raw);

        /// <inheritdoc/>
        protected override Year FromString(string str, CultureInfo culture) => Year.Parse(str, culture);

        /// <inheritdoc/>
        protected override int ToRaw(Year svo) => (int)svo;
    }
}
