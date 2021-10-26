using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for an email address.</summary>
    public class EmailAddressTypeConverter : SvoTypeConverter<EmailAddress>
    {
        /// <inheritdoc/>
        [Pure]
        protected override EmailAddress FromString(string str, CultureInfo culture) => EmailAddress.Parse(str, culture);
    }
}
