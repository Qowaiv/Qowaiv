﻿using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for an email address.</summary>
    public class EmailAddressTypeConverter : SvoTypeConverter<EmailAddress>
    {
        /// <inheritdoc/>
        protected override EmailAddress FromString(string str, CultureInfo culture) => EmailAddress.Parse(str, culture);
    }
}
