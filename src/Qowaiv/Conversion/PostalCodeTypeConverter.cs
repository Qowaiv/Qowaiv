using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a postal code.</summary>
    public class PostalCodeTypeConverter : SvoTypeConverter<PostalCode>
    {
        /// <inheritdoc/>
        [Pure]
        protected override PostalCode FromString(string str, CultureInfo culture) => PostalCode.Parse(str, culture);
    }
}
