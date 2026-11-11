namespace Qowaiv.EntityFrameworkCore.Converters;

internal sealed class IbanStringConverter() : ValueConverter<InternationalBankAccountNumber, string>(
    svo => svo.ToString(),
    str => InternationalBankAccountNumber.Parse(str));
