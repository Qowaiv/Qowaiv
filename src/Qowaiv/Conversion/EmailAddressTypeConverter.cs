namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for an email address.</summary>
[Inheritable]
public class EmailAddressTypeConverter : SvoTypeConverter<EmailAddress>
{
    /// <inheritdoc/>
    [Pure]
    protected override EmailAddress FromString(string? str, CultureInfo? culture) => EmailAddress.Parse(str, culture);
}

