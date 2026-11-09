namespace FluentValidation;

/// <summary>Fluent validation for <see cref="EmailAddress"/>.</summary>
public static class EmailAddressValidation
{
    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, EmailAddress> ruleBuilder)
    {
        /// <summary>Disallow IP-based email addresses.</summary>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, EmailAddress> NotIPBased()
            => Guard.NotNull(ruleBuilder)
            .Must(emailAddress => !emailAddress.IsIPBased)
            .WithMessage(_ => QowaivValidationFluentMessages.NoIPBasedEmailAddress);
    }
}
