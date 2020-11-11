using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for telephone number.</summary>
    public class TelephoneNumberTypeConverter : SvoTypeConverter<TelephoneNumber>
    {
        /// <inheritdoc/>
        protected override TelephoneNumber FromString(string str, CultureInfo culture) => TelephoneNumber.Parse(str, culture);
    }
}
