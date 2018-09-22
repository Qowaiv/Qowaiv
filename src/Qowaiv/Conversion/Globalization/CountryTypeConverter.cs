using Qowaiv.Globalization;
using System.Globalization;

namespace Qowaiv.Conversion.Globalization
{
    /// <summary>Provides a conversion for a Country.</summary>
    public class CountryTypeConverter : SvoTypeConverter<Country>
    {
        /// <inheritdoc/>
        protected override Country FromString(string str, CultureInfo culture) => Country.Parse(str, culture);
    }
}
