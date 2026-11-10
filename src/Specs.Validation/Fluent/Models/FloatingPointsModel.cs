using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Specs.Fluent.Models;

public sealed class FloatingPointsModel
{
    public float Float { get; set; }
    public double Double { get; set; }

    public float? NullableFloat { get; set; }
    public double? NullableDouble { get; set; }
}

public sealed class FloatingPointsValidator : ModelValidator<FloatingPointsModel>
{
    public FloatingPointsValidator()
    {
        RuleFor(m => m.Double).IsFinite();
        RuleFor(m => m.Float).IsFinite();
        RuleFor(m => m.NullableFloat).IsFinite();
        RuleFor(m => m.NullableDouble).IsFinite();
    }
}
