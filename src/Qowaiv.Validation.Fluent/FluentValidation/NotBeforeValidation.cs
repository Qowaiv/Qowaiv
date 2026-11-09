using System.Linq.Expressions;

namespace FluentValidation;

/// <summary>Validates if a <see cref="IComparable"/> is not before a specific value.</summary>
/// <remarks>
/// This is syntactic sugar for:
/// <code>
/// ruleBuilder.GreaterThanOrEqualTo(valueToCompare).WithMessage().
/// </code>
/// </remarks>
public static class NotBeforeValidation
{
    /// <typeparam name="T">Type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">Type of the property being validated.</typeparam>
    extension<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
        where TProperty : IComparable<TProperty>, IComparable
    {
        /// <summary>
        /// Defines a 'not before' validator on the current rule builder.
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <param name="valueToCompare">The value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> NotBefore(TProperty valueToCompare)
            => ruleBuilder.GreaterThanOrEqualTo(valueToCompare).WithMessage(_ => QowaivValidationFluentMessages.NotBefore);

        /// <summary>
        /// Defines a 'not before' validator on the current rule builder using a lambda expression.
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <param name="expression">A lambda that should return the value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> NotBefore(Expression<Func<T, TProperty>> expression)
            => ruleBuilder.GreaterThanOrEqualTo(expression).WithMessage(_ => QowaivValidationFluentMessages.NotBefore);
    }

    /// <typeparam name="T">Type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">Type of the property being validated.</typeparam>
    extension<T, TProperty>(IRuleBuilder<T, TProperty?> ruleBuilder)
        where TProperty : struct, IComparable<TProperty>, IComparable
    {
        /// <summary>
        /// Defines a 'not before' validator on the current rule builder.
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <param name="valueToCompare">The value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> NotBefore(TProperty valueToCompare)
            => ruleBuilder.GreaterThanOrEqualTo(valueToCompare).WithMessage(_ => QowaivValidationFluentMessages.NotBefore);

        /// <summary>
        /// Defines a 'not before' validator on the current rule builder using a lambda expression.
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <param name="expression">A lambda that should return the value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> NotBefore(Expression<Func<T, TProperty?>> expression)
            => ruleBuilder.GreaterThanOrEqualTo(expression).WithMessage(_ => QowaivValidationFluentMessages.NotBefore);

        /// <summary>
        /// Defines a 'not before' validator on the current rule builder using a lambda expression.
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <param name="expression">A lambda that should return the value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty?> NotBefore(Expression<Func<T, TProperty>> expression)
            => ruleBuilder.GreaterThanOrEqualTo(expression).WithMessage(_ => QowaivValidationFluentMessages.NotBefore);
    }

    /// <typeparam name="T">Type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">Type of the property being validated.</typeparam>
    extension<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
        where TProperty : struct, IComparable<TProperty>, IComparable
    {
        /// <summary>
        /// Defines a 'not before' validator on the current rule builder using a lambda expression.
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <param name="expression">A lambda that should return the value being compared.</param>
        [FluentSyntax]
        public IRuleBuilderOptions<T, TProperty> NotBefore(Expression<Func<T, TProperty?>> expression)
            => ruleBuilder.GreaterThanOrEqualTo(expression).WithMessage(_ => QowaivValidationFluentMessages.NotBefore);
    }
}
