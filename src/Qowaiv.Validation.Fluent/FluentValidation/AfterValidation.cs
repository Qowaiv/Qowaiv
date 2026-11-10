using System.Linq.Expressions;

namespace FluentValidation;

/// <summary>Validates if a <see cref="IComparable"/> is after a specific value.</summary>
/// <remarks>
/// This is syntactic sugar for:
/// <code>
/// ruleBuilder.GreaterThan(valueToCompare).WithMessage().
/// </code>
/// </remarks>
public static class AfterValidation
{
    /// <typeparam name="T">Type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">Type of the property being validated.</typeparam>
    extension<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
        where TProperty : IComparable<TProperty>, IComparable
    {
        /// <summary>
        /// Defines an 'after' validator on the current rule builder.
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal to the specified value.
        /// </summary>
        /// <param name="valueToCompare">The value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> After(TProperty valueToCompare)
            => ruleBuilder.GreaterThan(valueToCompare).WithMessage(_ => QowaivValidationFluentMessages.After);

        /// <summary>
        /// Defines an 'after' validator on the current rule builder using a lambda expression.
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal to the specified value.
        /// </summary>
        /// <param name="expression">A lambda that should return the value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> After(Expression<Func<T, TProperty>> expression)
            => ruleBuilder.GreaterThan(expression).WithMessage(_ => QowaivValidationFluentMessages.After);
    }

    /// <typeparam name="T">Type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">Type of the property being validated.</typeparam>
    extension<T, TProperty>(IRuleBuilder<T, TProperty?> ruleBuilder)
        where TProperty : struct, IComparable<TProperty>, IComparable
    {
        /// <summary>
        /// Defines an 'after' validator on the current rule builder.
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal to the specified value.
        /// </summary>
        /// <param name="valueToCompare">The value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> After(TProperty valueToCompare)
            => ruleBuilder.GreaterThan(valueToCompare).WithMessage(_ => QowaivValidationFluentMessages.After);

        /// <summary>
        /// Defines an 'after' validator on the current rule builder using a lambda expression.
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal to the specified value.
        /// </summary>
        /// <param name="expression">A lambda that should return the value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> After(Expression<Func<T, TProperty>> expression)
            => ruleBuilder.GreaterThan(expression).WithMessage(_ => QowaivValidationFluentMessages.After);

        /// <summary>
        /// Defines an 'after' validator on the current rule builder using a lambda expression.
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal to the specified value.
        /// </summary>
        /// <param name="expression">A lambda that should return the value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> After(Expression<Func<T, TProperty?>> expression)
            => ruleBuilder.GreaterThan(expression).WithMessage(_ => QowaivValidationFluentMessages.After);
    }

    /// <typeparam name="T">Type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">Type of the property being validated.</typeparam>
    extension<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
        where TProperty : struct, IComparable<TProperty>, IComparable
    {
        /// <summary>
        /// Defines an 'after' validator on the current rule builder using a lambda expression.
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal to the specified value.
        /// </summary>
        /// <param name="expression">A lambda that should return the value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> After(Expression<Func<T, TProperty?>> expression)
            => ruleBuilder.GreaterThan(expression).WithMessage(_ => QowaivValidationFluentMessages.After);
    }
}
