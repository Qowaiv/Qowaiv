using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Specs.Fluent.Models;

public class WarningModel
{
    public string Message { get; set; } = "Some message";
}

public class WarningModelValidator : ModelValidator<WarningModel>
{
    public WarningModelValidator()
    {
        RuleFor(m => m.Message).Must(v => false).WithMessage("Test warning.").WithSeverity(Severity.Warning);
        RuleFor(m => m.Message).Must(v => false).WithMessage("Nice that you validated this.").WithSeverity(Severity.Info);
    }
}
