using Microsoft.EntityFrameworkCore;

namespace Qowaiv.EntityFrameworkCore;

/// <summary>Extensions to bind Propereties as SVO's.</summary>
public static class TypedModelConfigurationBuilder
{
    extension(ModelConfigurationBuilder builder)
    {
        [FluentSyntax]
        public ModelConfigurationBuilder WithSvoProperties()
        {
            builder
               .Properties<Amount>()
               .HaveConversion<AmountDecimalConverter>()
               .HavePrecision(35, 3);

            builder
                .Properties<EmailAddress>()
                .HaveConversion<EmailStringConverter>()
                .HaveMaxLength(EmailAddress.MaxLength)
                .AreUnicode();

            builder
               .Properties<InternationalBankAccountNumber>()
               .HaveConversion<IbanStringConverter>()
               .HaveMaxLength(InternationalBankAccountNumber.MaxLength)
               .AreUnicode(false);

            return builder;
        }
    }
}
