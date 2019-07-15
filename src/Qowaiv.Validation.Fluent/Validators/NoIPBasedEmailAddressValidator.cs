using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Qowaiv.Validation.Fluent.Validators
{
    /// <summary>A validator that disallows IP-based email addresses.</summary>
    internal class NoIPBasedEmailAddressValidator : PropertyValidator
    {
        /// <summary>Creates a new instance of a <see cref="NoIPBasedEmailAddressValidator"/>.</summary>
        public NoIPBasedEmailAddressValidator()
            : base(new LazyStringSource(cxt => QowaivValidationFluentMessages.NoIPBasedEmailAddress)) { }

        /// <inheritdoc />
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var email = (EmailAddress)context.PropertyValue;
            return !email.IsIPBased;
        }
    }
}
