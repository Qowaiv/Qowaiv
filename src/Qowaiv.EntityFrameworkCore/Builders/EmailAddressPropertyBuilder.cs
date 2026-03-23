using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qowaiv.EntityFrameworkCore.Builders;

public sealed class EmailAddressPropertyBuilder : PropertyBuilder<EmailAddress>
{
    internal EmailAddressPropertyBuilder(IMutableProperty property) : base(property)
    {
        HasConversion(email => email.ToString(), str => EmailAddress.Parse(str));
        HasMaxLength(EmailAddress.MaxLength);
    }
}
