using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qowaiv.EntityFrameworkCore.Builders;

namespace Qowaiv.EntityFrameworkCore;

public static class QowaivPropertyBuilder
{
    extension(PropertyBuilder<Percentage> builder)
    {
        [FluentSyntax]
        public PropertyBuilder<Percentage> Svo(bool required = true)
        {
            Guard.NotNull(builder)
                .HasConversion(email => (decimal)email, dec => Percentage.Create(dec))
                .IsRequired(required);

            return builder;
        }
    }

    extension(PropertyBuilder<EmailAddress> builder)
    {
        [FluentSyntax]
        public EmailAddressPropertyBuilder Svo() => new(Guard.NotNull(builder).Metadata);
    }
}
