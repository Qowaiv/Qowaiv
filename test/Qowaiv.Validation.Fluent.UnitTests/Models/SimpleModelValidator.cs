using FluentValidation;

namespace Qowaiv.Validation.Fluent.UnitTests.Models
{
    public class SimpleModelValidator : AbstractValidator<SimpleModel>
    {
        public SimpleModelValidator()
        {
            RuleFor(m => m.Email).NotEmptyOrUnknown();
        }
    }
}
