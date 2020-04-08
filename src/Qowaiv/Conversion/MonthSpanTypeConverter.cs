using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a month span.</summary>
    public class MonthSpanTypeConverter : NumericTypeConverter<MonthSpan, int>
    {
        /// <inheritdoc/>
        protected override MonthSpan FromRaw(int raw) => MonthSpan.FromMonths(raw);

        /// <inheritdoc/>
        protected override MonthSpan FromString(string str, CultureInfo culture) => MonthSpan.Parse(str, culture);

        /// <inheritdoc/>
        protected override int ToRaw(MonthSpan svo) => (int)svo;
    }
}
