namespace FluentValidation;

/// <summary>Fluent validation for Single Value Objects being required.</summary>
public static class RequiredValidation
{
    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    /// <typeparam name="TProperty">
    /// Type of the property being validated.
    /// </typeparam>
    extension<TModel, TProperty>(IRuleBuilder<TModel, TProperty> ruleBuilder)
    {
        /// <summary>The property is required.</summary>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, TProperty> Required()
            => ruleBuilder.Required(allowUnknown: false);

        /// <summary>The property is required.</summary>
        /// <param name="allowUnknown">
        /// If true, unknown values are seen.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, TProperty> Required(bool allowUnknown)
        {
            Guard.NotNull(ruleBuilder);

            return allowUnknown
                ? ruleBuilder
                    .NotEmpty().WithMessage(_ => QowaivValidationFluentMessages.Required)
                : ruleBuilder
                    .NotEmptyOrUnknown().WithMessage(_ => QowaivValidationFluentMessages.Required);
        }
    }
}
