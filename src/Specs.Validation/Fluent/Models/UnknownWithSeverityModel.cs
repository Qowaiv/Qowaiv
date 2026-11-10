using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Specs.Fluent.Models;

public class UnknownWithSeverityModel
{
    public EmailAddress Email { get; set; }
}
public class UnknownWithSeverityModelValidator : ModelValidator<UnknownWithSeverityModel>
{
    public UnknownWithSeverityModelValidator()
        => RuleFor(m => m.Email).NotEmptyOrUnknown().WithSeverity(Severity.Warning);
}
