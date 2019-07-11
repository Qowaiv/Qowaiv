using Qowaiv;
using Qowaiv.Validation.Fluent.Validators;

namespace FluentValidation
{
    public static class RuleBuilderOptionsExtensions
    {
        /// <summary>Defines a 'not unkown' validator on the current rule builder.
        /// Validation will fail if the property has the 'unkown' value for the type.
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
    }
}
