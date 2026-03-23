using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qowaiv.EntityFrameworkCore.Builders;
using System.Linq.Expressions;

namespace Qowaiv.EntityFrameworkCore;

public static class QowaivPropertyBuilder
{
    extension<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        [FluentSyntax]
        public EmailAddressPropertyBuilder SvoProperty(Expression<Func<TEntity, EmailAddress>> propertyExpression)
            => new(builder.Property(propertyExpression).Metadata);
    }

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
}
