using Qowaiv.Financial;

namespace Qowaiv.Conversion.Financial;

/// <summary>Provides a conversion for an IBAN.</summary>
public class InternationalBankAccountNumberTypeConverter : SvoTypeConverter<InternationalBankAccountNumber>
{
    /// <inheritdoc/>
    [Pure]
    protected override InternationalBankAccountNumber FromString(string? str, CultureInfo? culture) => InternationalBankAccountNumber.Parse(str, culture);
}

