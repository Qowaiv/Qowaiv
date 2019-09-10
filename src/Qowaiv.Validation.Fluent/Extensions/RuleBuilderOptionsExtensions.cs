using Qowaiv;
using Qowaiv.Globalization;
using Qowaiv.Validation.Fluent.Validators;
using System;

namespace FluentValidation
{
    /// <summary>Extensions on <see cref="IRuleBuilder{T, TProperty}"/>.</summary>
    public static class RuleBuilderOptionsExtensions
    {
        /// <summary>Defines a 'not unknown' validator on the current rule builder.
        /// Validation will fail if the property has the 'unknown' value for the type.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        public static IRuleBuilderOptions<T, TProperty> NotUnknown<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            Guard.NotNull(ruleBuilder, nameof(ruleBuilder));
            return ruleBuilder.SetValidator(new NotUnknownValidator(Unknown.Value(typeof(TProperty))));
        }

        /// <summary>Defines a 'not empty' and a 'not unkown' validator on the current rule builder.</summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        public static IRuleBuilderOptions<T, TProperty> NotEmptyOrUnknown<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().NotUnknown();
        }

        /// <summary>Defines a postal code validator on the current rule builder.</summary>
        /// <typeparam name="T">
        /// Type of object being validated.
        /// </typeparam>
        /// <param name="ruleBuilder">
        /// The rule builder on which the validator should be defined.
        /// </param>
        /// <param name="country">
        /// The county the postal code should be valid for.
        /// </param>
        public static IRuleBuilderOptions<T, PostalCode> ValidFor<T>(this IRuleBuilder<T, PostalCode> ruleBuilder, Country country)
        {
            Guard.NotNull(ruleBuilder, nameof(ruleBuilder));
            return ruleBuilder.SetValidator(new PostalCodeValidator(country));
        }

        /// <summary>Defines a postal code validator on the current rule builder.</summary>
        /// <typeparam name="T">
        /// Type of object being validated.
        /// </typeparam>
        /// <param name="ruleBuilder">
        /// The rule builder on which the validator should be defined.
        /// </param>
        /// <param name="country">
        /// The county the postal code should be valid for.
        /// </param>
        public static IRuleBuilderOptions<T, PostalCode> ValidFor<T>(this IRuleBuilder<T, PostalCode> ruleBuilder, Func<T, Country> country)
        {
            Guard.NotNull(ruleBuilder, nameof(ruleBuilder));
            return ruleBuilder.SetValidator(new PostalCodeValidator(country.ToNongenericInput()));
        }


        /// <summary>Defines validator that disallows IP-based email addresses on the current rule builder.</summary>
        /// <typeparam name="TModel">
        /// Type of object being validated.
        /// </typeparam>
        /// <param name="ruleBuilder">
        /// The rule builder on which the validator should be defined.
        /// </param>
        public static IRuleBuilderOptions<TModel, EmailAddress> NotIPBased<TModel>(this IRuleBuilder<TModel, EmailAddress> ruleBuilder)
        {
            Guard.NotNull(ruleBuilder, nameof(ruleBuilder));
            return ruleBuilder.SetValidator(new NoIPBasedEmailAddressValidator());
        }
    }
}
