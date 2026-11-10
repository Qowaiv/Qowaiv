using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Specs.Fluent.Models;

public class NoIPBasedEmailAddressModel
{
    public EmailAddress Email { get; set; }
}
public class NoIPBasedEmailAddressModelValidator : ModelValidator<NoIPBasedEmailAddressModel>
{
    public NoIPBasedEmailAddressModelValidator() => RuleFor(m => m.Email).NotIPBased();
}
