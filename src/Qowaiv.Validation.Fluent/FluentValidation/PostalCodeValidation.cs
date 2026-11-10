namespace FluentValidation;

/// <summary>Fluent validation for <see cref="PostalCode"/>.</summary>
public static class PostalCodeValidation
{
    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, PostalCode> ruleBuilder)
    {
        /// <summary>The postal code should be valid for the specified country.</summary>
        /// <param name="country">
        /// The county the postal code should be valid for.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, PostalCode> ValidFor(Country country)
            => Guard.NotNull(ruleBuilder)
            .ValidFor(_ => country);

        /// <summary>The postal code should be valid for the specified country.</summary>
        /// <param name="country">
        /// The county the postal code should be valid for.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, PostalCode> ValidFor(Func<TModel, Country> country)
            => Guard.NotNull(ruleBuilder)
                .Must((model, postalCode, context) => IsValidFor(postalCode, Guard.NotNull(country)(model), context))
                .WithMessage(_ => QowaivValidationFluentMessages.PostalCodeValidForCountry);
    }

    [Impure]
    private static bool IsValidFor<TModel>(PostalCode postalCode, Country country, ValidationContext<TModel> context)
    {
        if (country.IsKnown
            && postalCode.IsKnown
            && !postalCode.IsValid(country))
        {
            context.MessageFormatter
                .AppendArgument(nameof(Country), country.DisplayName)
                .AppendArgument("ComparisonValue", postalCode);

            return false;
        }
        else return true;
    }
}
