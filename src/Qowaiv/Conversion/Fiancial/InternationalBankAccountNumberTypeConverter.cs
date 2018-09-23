using Qowaiv.Financial;
using System.Globalization;

namespace Qowaiv.Conversion.Financial
{
    /// <summary>Provides a conversion for an IBAN.</summary>
    public class InternationalBankAccountNumberTypeConverter : SvoTypeConverter<InternationalBankAccountNumber>
    {
        /// <inheritdoc/>
        protected override InternationalBankAccountNumber FromString(string str, CultureInfo culture) => InternationalBankAccountNumber.Parse(str, culture);
    }
}
