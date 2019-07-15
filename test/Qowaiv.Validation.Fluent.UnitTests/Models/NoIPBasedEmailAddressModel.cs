using FluentValidation;

namespace Qowaiv.Validation.Fluent.UnitTests.Models
{
    public class NoIPBasedEmailAddressModel
    {
        public EmailAddress Email { get; set; }
    }
    public class NoIPBasedEmailAddressModelValidator : AbstractValidator<NoIPBasedEmailAddressModel>
    {
        public NoIPBasedEmailAddressModelValidator()
        {
            RuleFor(m => m.Email).NotIPBased();
        }
    }
}
