namespace FluentValidation;

/// <summary>FLuent validation for <see cref="double"/> and <see cref="float"/>.</summary>
public static class FloatingPointValidation
{
    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, double> ruleBuilder)
    {
        /// <summary>The floating point should be a finite number.</summary>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, double> IsFinite()
            => Guard.NotNull(ruleBuilder)
            .Must(number => number.IsFinite())
            .WithMessage(_ => QowaivValidationFluentMessages.IsFinite);
    }

    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, double?> ruleBuilder)
    {
        /// <summary>The floating point should be a finite number.</summary>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, double?> IsFinite()
            => Guard.NotNull(ruleBuilder)
            .Must(number => !number.HasValue || number.Value.IsFinite())
            .WithMessage(_ => QowaivValidationFluentMessages.IsFinite);
    }

    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, float> ruleBuilder)
    {
        /// <summary>The floating point should be a finite number.</summary>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, float> IsFinite()
            => Guard.NotNull(ruleBuilder)
            .Must(number => number.IsFinite())
            .WithMessage(_ => QowaivValidationFluentMessages.IsFinite);
    }

    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, float?> ruleBuilder)
    {
        /// <summary>The floating point should be a finite number.</summary>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, float?> IsFinite()
            => Guard.NotNull(ruleBuilder)
            .Must(number => !number.HasValue || number.Value.IsFinite())
            .WithMessage(_ => QowaivValidationFluentMessages.IsFinite);
    }
}
