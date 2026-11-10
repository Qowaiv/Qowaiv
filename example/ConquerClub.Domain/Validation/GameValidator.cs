using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace ConquerClub.Domain.Validation;

internal sealed class GameValidator : ModelValidator<Game>
{
    public GameValidator()
    {
        RuleFor(g => g.Settings).Required();
        RuleFor(g => g.Countries).Required();
        RuleFor(g => g.Phase).IsInEnum().NotEmpty();
        RuleFor(g => g.Round).LessThanOrEqualTo(game => game.Settings.RoundLimit);
        RuleFor(g => g.ActivePlayer).NotEmptyOrUnknown();

        RuleForEach(g => g.Countries).SetValidator(new CountryValidator());
    }
}
