namespace FluentValidation;

/// <summary>Fluent validation for Single Value Objects being (not) unknown.</summary>
public static class UnknownValidation
{
    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    /// <typeparam name="TProperty">
    /// Type of the property being validated.
    /// </typeparam>
    extension<TModel, TProperty>(IRuleBuilder<TModel, TProperty> ruleBuilder)
    {
        /// <summary>Defines a 'not unknown' validator on the current rule builder.
        /// Validation will fail if the property has the 'unknown' value for the type.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, TProperty> NotUnknown()
            => Guard.NotNull(ruleBuilder)
            .Must(prop => prop is null || !Equals(SvoValues.Unknown(typeof(TProperty)), prop))
            .WithMessage(_ => QowaivValidationFluentMessages.NotUnknown);

        /// <summary>Defines a 'not empty' and a 'not unkmown' validator on the current rule builder.</summary>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, TProperty> NotEmptyOrUnknown()
            => Guard.NotNull(ruleBuilder)
            .SetValidator(new NotEmptyOrUnknownValidator<TModel, TProperty>())
            .WithMessage(_ => QowaivValidationFluentMessages.NotEmptyOrUnknown);
    }
}
