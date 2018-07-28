using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a house number.</summary>
    public class HouseNumberTypeConverter : NumericTypeConverter<HouseNumber, int>
    {
        /// <inheritdoc />
        protected override HouseNumber FromRaw(int raw) => HouseNumber.Create(raw);

        /// <inheritdoc />
        protected override HouseNumber FromString(string str, CultureInfo culture) => HouseNumber.Parse(str, culture);

        /// <inheritdoc />
        protected override int ToRaw(HouseNumber svo) => (int)svo;
    }
}
