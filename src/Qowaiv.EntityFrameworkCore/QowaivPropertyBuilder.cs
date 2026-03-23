using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qowaiv.EntityFrameworkCore;

public static class QowaivPropertyBuilder
{
    extension(PropertyBuilder<EmailAddress> builder)
    {
        [FluentSyntax]
        public PropertyBuilder<EmailAddress> Svo(int? maxLength = EmailAddress.MaxLength, bool required = false)
        {
            Guard.NotNull(builder)
                .HasConversion(email => email.ToString(), str => EmailAddress.Parse(str))
                .IsRequired(required);

            if (maxLength > 0)
            {
                builder.HasMaxLength(maxLength.Value);
            }

            return builder;
        }
    }
}
