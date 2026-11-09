using System.Numerics;

namespace FluentValidation;

/// <summary>Validates <see cref="INumber{TSelf}"/>.</summary>
public static class NumberValidation
{
    /// <typeparam name="T">Type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">Type of the property being validated.</typeparam>
    extension<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
        where TProperty : INumber<TProperty>
    {
        /// <summary>
        /// Defines a 'is finite' validator on the current rule builder.
        /// The validation will succeed if the property value is a finite number.
        /// </summary>
        [FluentSyntax]
        [OverloadResolutionPriority(1)]
        public IRuleBuilderOptions<T, TProperty> IsFinite() => ruleBuilder
            .Must(TProperty.IsFinite)
            .WithMessage(_ => QowaivValidationFluentMessages.IsFinite);

        /// <summary>
        /// Defines a 'is positive' validator on the current rule builder.
        /// The validation will succeed if the property value is greather than zero.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> IsPositive() => ruleBuilder
            .Must(x => x > TProperty.Zero)
            .WithMessage(_ => QowaivValidationFluentMessages.IsPositive);

        /// <summary>
        /// Defines a 'is negative' validator on the current rule builder.
        /// The validation will succeed if the property value is less than zero.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> IsNegative() => ruleBuilder
            .Must(x => x < TProperty.Zero)
            .WithMessage(_ => QowaivValidationFluentMessages.IsNegative);

        /// <summary>
        /// Defines a 'is not positive' validator on the current rule builder.
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> IsNotPositive() => ruleBuilder
            .Must(x => x <= TProperty.Zero)
            .WithMessage(_ => QowaivValidationFluentMessages.IsNotPositive);

        /// <summary>
        /// Defines a 'is not negative' validator on the current rule builder.
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> IsNotNegative() => ruleBuilder
            .Must(x => x >= TProperty.Zero)
            .WithMessage(_ => QowaivValidationFluentMessages.IsNotNegative);
    }

    /// <typeparam name="T">Type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">Type of the property being validated.</typeparam>
    extension<T, TProperty>(IRuleBuilder<T, TProperty?> ruleBuilder)
        where TProperty : struct, INumber<TProperty>
    {
        /// <summary>
        /// Defines a 'is finite' validator on the current rule builder.
        /// The validation will succeed if the property value is a finite number.
        /// </summary>
        [FluentSyntax]
        [OverloadResolutionPriority(1)]
        public IRuleBuilderOptions<T, TProperty?> IsFinite() => ruleBuilder
            .Must(x => x is null || TProperty.IsFinite(x.Value))
            .WithMessage(_ => QowaivValidationFluentMessages.IsFinite);

        /// <summary>
        /// Defines a 'is positive' validator on the current rule builder.
        /// The validation will succeed if the property value is greather than zero.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> IsPositive() => ruleBuilder
            .Must(x => x is null || x > TProperty.Zero)
            .WithMessage(_ => QowaivValidationFluentMessages.IsPositive);

        /// <summary>
        /// Defines a 'is negative' validator on the current rule builder.
        /// The validation will succeed if the property value is less than zero.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> IsNegative() => ruleBuilder
            .Must(x => x is null || x < TProperty.Zero)
            .WithMessage(_ => QowaivValidationFluentMessages.IsNegative);

        /// <summary>
        /// Defines a 'is not positive' validator on the current rule builder.
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> IsNotPositive() => ruleBuilder
            .Must(x => x is null || x <= TProperty.Zero)
            .WithMessage(_ => QowaivValidationFluentMessages.IsNotPositive);

        /// <summary>
        /// Defines a 'is not negative' validator on the current rule builder.
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> IsNotNegative() => ruleBuilder
            .Must(x => x is null || x >= TProperty.Zero)
            .WithMessage(_ => QowaivValidationFluentMessages.IsNotNegative);
    }
}
