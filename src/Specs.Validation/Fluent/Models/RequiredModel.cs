using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Specs.Fluent.Models;

public class RequiredModel
{
    public EmailAddress Email { get; set; }
    public Country Country { get; set; }
}
public class RequiredModelValidator : ModelValidator<RequiredModel>
{
    public RequiredModelValidator()
    {
        RuleFor(m => m.Email).Required();
        RuleFor(m => m.Country).Required(true);
    }
}
