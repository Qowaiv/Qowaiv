using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qowaiv.Financial;
using System.Linq.Expressions;

namespace Qowaiv.EntityFrameworkCore;

/// <summary>Extensions to bind Propereties as SVO's.</summary>
public static class SvoPropertyBuilder
{
    extension<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        /// <summary>
        /// Returns a builder that can be used to configure an <see cref="EmailAddress"/>
        /// SVO property of the entity type. If the specified property is not
        /// already part of the model, it will be added.
        /// </summary>
        /// <remarks>
        /// Configured are:
        /// * Conversion
        /// * MaxLength
        /// * Unicode.
        /// </remarks>
        /// <param name="propertyExpression">
        /// A lambda expression representing the property to be configured (
        /// <c>model => model.Email</c>).
        /// </param>
        [FluentSyntax]
        [CLSCompliant(false)]
        public PropertyBuilder<EmailAddress> SvoProperty(Expression<Func<TEntity, EmailAddress>> propertyExpression) => builder
            .Property(propertyExpression)
            .HasConversion(svo => svo.ToString(), str => EmailAddress.Parse(str))
            .HasMaxLength(EmailAddress.MaxLength)
            .IsUnicode();

        /// <summary>
        /// Returns a builder that can be used to configure an <see cref="InternationalBankAccountNumber"/>
        /// SVO property of the entity type. If the specified property is not
        /// already part of the model, it will be added.
        /// </summary>
        /// <remarks>
        /// Configured are:
        /// * Conversion
        /// * MaxLength
        /// * ASCII.
        /// </remarks>
        /// <param name="propertyExpression">
        /// A lambda expression representing the property to be configured (
        /// <c>model => model.Iban</c>).
        /// </param>
        [FluentSyntax]
        [CLSCompliant(false)]
        public PropertyBuilder<InternationalBankAccountNumber> SvoProperty(Expression<Func<TEntity, InternationalBankAccountNumber>> propertyExpression) => builder
            .Property(propertyExpression)
            .HasConversion(svo => svo.ToString(), str => InternationalBankAccountNumber.Parse(str))
            .IsUnicode(false)
            .HasMaxLength(InternationalBankAccountNumber.MaxLength);
    }
}
