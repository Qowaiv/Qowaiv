using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Specs.Fluent.Models;

public class ZeroModel
{
    public double? NullableNumber { get; set; }
    public double Number { get; set; }
}

public sealed class IsPositiveValidator : ModelValidator<ZeroModel>
{
    public IsPositiveValidator()
    {
        RuleFor(m => m.NullableNumber).IsPositive();
        RuleFor(m => m.Number).IsPositive();
    }
}

public sealed class IsNegativeValidator : ModelValidator<ZeroModel>
{
    public IsNegativeValidator()
    {
        RuleFor(m => m.NullableNumber).IsNegative();
        RuleFor(m => m.Number).IsNegative();
    }
}

public sealed class IsNotPositiveValidator : ModelValidator<ZeroModel>
{
    public IsNotPositiveValidator()
    {
        RuleFor(m => m.NullableNumber).IsNotPositive();
        RuleFor(m => m.Number).IsNotPositive();
    }
}

public sealed class IsNotNegativeValidator : ModelValidator<ZeroModel>
{
    public IsNotNegativeValidator()
    {
        RuleFor(m => m.NullableNumber).IsNotNegative();
        RuleFor(m => m.Number).IsNotNegative();
    }
}
