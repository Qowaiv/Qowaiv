namespace Qowaiv.EntityFrameworkCore.Converters;

internal sealed class EmailStringConverter() : ValueConverter<EmailAddress, string>(
    svo => svo.ToString(),
    str => EmailAddress.Parse(str));
