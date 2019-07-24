using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Qowaiv.Genealogy
{
    public class PersonValidator : FluentModelValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(m => m.Gender)
                .Must(g => g.IsMaleOrFemale() || g.IsUnknown())
                .WithMessage("Not allowed value for 'Gender'.");

            RuleFor(m => m.DateOfDeath)
                .Must((person, d) => !d.HasValue || d.Value >= person.DateOfBirth)
                .WithMessage("'DateOfDeath' should not be before 'DateOfBirth'.");
        }
    }
}
