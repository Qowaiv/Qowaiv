using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Qowaiv.EntityFrameworkCore;

/// <summary>Extensions to bind Propereties as SVO's.</summary>
public static class SvoPropertyBuilder
{
    extension<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        /// <summary>
        /// Returns a builder that can be used to configure an SVO property of the entity type.
        /// If the specified property is not already part of the model, it will be added.
        /// </summary>
        /// <param name="propertyExpression">
        /// A lambda expression representing the property to be configured (
        /// <c>model => model.Email</c>).
        /// </param>
        [FluentSyntax]
        [CLSCompliant(false)]
        public PropertyBuilder<EmailAddress> SvoProperty(Expression<Func<TEntity, EmailAddress>> propertyExpression) => builder
            .Property(propertyExpression)
            .HasConversion(svo => svo.ToString(), str => EmailAddress.Parse(str))
            .IsUnicode()
            .HasMaxLength(EmailAddress.MaxLength);
    }
}
