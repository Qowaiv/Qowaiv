using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Models;

public sealed class SimpleEventSourcedAggregateValidator : ModelValidator<SimpleEventSourcedAggregate>
{
    public SimpleEventSourcedAggregateValidator()
        => RuleFor(m => m.IsWrong).Must(prop => !prop).WithMessage("Should not be wrong.");
}
