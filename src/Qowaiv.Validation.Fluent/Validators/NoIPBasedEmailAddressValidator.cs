using FluentValidation;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Qowaiv.Validation.Fluent.Validators
{
    /// <summary>A validator that disallowes IP-based email addresses.</summary>
    public class NoIPBasedEmailAddressValidator : PropertyValidator
    {
        /// <summary>Creates a new instance of a <see cref="NoIPBasedEmailAddressValidator"/>.</summary>
        public NoIPBasedEmailAddressValidator()
            : base(new LazyStringSource(cxt => QowaivValidationFluentMessages.NoIPBasedEmailAddress)) { }

        /// <inheritdoc />
        protected override bool IsValid(PropertyValidatorContext context)
        {
            Guard.NotNull(context, nameof(context));
            var email = Guard.IsInstanceOf<EmailAddress>(context.PropertyValue, nameof(context.PropertyValue));
            return !email.IsIPBased;
        }
    }
}
