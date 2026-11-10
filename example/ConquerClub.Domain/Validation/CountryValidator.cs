using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace ConquerClub.Domain.Validation;

internal sealed class CountryValidator : ModelValidator<Country>
{
    public CountryValidator()
    {
        RuleFor(c => c.Army).NotEmpty();
        RuleFor(c => c.Owner).NotUnknown();
    }
}
