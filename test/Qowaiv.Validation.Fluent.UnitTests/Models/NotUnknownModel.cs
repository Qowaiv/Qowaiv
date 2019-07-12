using FluentValidation;

namespace Qowaiv.Validation.Fluent.UnitTests.Models
{
    public class NotUnknownModel
    {
        public EmailAddress Email { get; set; }
    }
    public class NotUnknownModelValidator : AbstractValidator<NotUnknownModel>
    {
        public NotUnknownModelValidator()
        {
            RuleFor(m => m.Email).NotEmptyOrUnknown();
        }
    }
}
