using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a month.</summary>
    public class MonthTypeConverter : NumericTypeConverter<Month, byte>
    {
        /// <inheritdoc />
        protected override Month FromRaw(byte raw) => Month.Create((raw == 0) ? null : (int?)raw);

        /// <inheritdoc />
        protected override Month FromString(string str, CultureInfo culture) => Month.Parse(str, culture);

        /// <inheritdoc />
        protected override byte ToRaw(Month svo) => (byte)svo;
    }
}
