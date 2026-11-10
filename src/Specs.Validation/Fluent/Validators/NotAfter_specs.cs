using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Fluent_validation.NotAfter_specs;

public class Is_valid
{
    [Test]
    [MethodDataSource(nameof(Validators))]
    public void When_not_after(ModelValidator<Model> validator)
        => new Model { DateOfBirth = new(1942, 11, 27), DateOfDeath = new Date(1970, 09, 18) }
        .ValidateWith(validator)
        .Should().BeValid();

    [Test]
    [MethodDataSource(nameof(Validators))]
    public void When_nullable_not_set(ModelValidator<Model> validator)
        => new Model { DateOfBirth = new(1942, 11, 27), DateOfDeath = null }
        .ValidateWith(validator)
        .Should().BeValid();

    private static readonly IEnumerable<ModelValidator<Model>> Validators = [new FixedValidator(), new ExpressionValidator(), new ExpressionNullableValidator()];
}

public class Is_not_valid
{
    [Test]
    [Arguments("2000-01-02", "'Date Of Birth' is after 01/01/2000.", "en-GB")]
    [Arguments("2024-12-31", "'Date Of Birth' is na 01-01-2000.", "nl-NL")]
    public void When_after_fixed(Date dt, string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new Model { DateOfBirth = dt }
            .ValidateWith(new FixedValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "DateOfBirth"));
    }

    [Test]
    [Arguments("2000-01-02", "'Date Of Death' is after 01/01/2000.", "en-GB")]
    [Arguments("2024-12-31", "'Date Of Death' is na 01-01-2000.", "nl-NL")]
    public void When_nullable_after_fixed(Date dt, string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new Model { DateOfDeath = dt }
            .ValidateWith(new FixedValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "DateOfDeath"));
    }

    [Test]
    [Arguments("2000-01-02", "'Date Of Birth' is after 01/01/2000.", "en-GB")]
    [Arguments("2024-12-31", "'Date Of Birth' is na 01-01-2000.", "nl-NL")]
    public void When_after_expression(Date dt, string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new Model { DateOfBirth = dt }
            .ValidateWith(new ExpressionValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "DateOfBirth"));
    }

    [Test]
    [Arguments("2000-01-02", "'Date Of Death' is after 01/01/2000.", "en-GB")]
    [Arguments("2024-12-31", "'Date Of Death' is na 01-01-2000.", "nl-NL")]
    public void When_nullable_after_expression(Date dt, string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new Model { DateOfDeath = dt }
            .ValidateWith(new ExpressionValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "DateOfDeath"));
    }

    [Test]
    [Arguments("2000-01-02", "'Date Of Birth' is after 01/01/2000.", "en-GB")]
    [Arguments("2024-12-31", "'Date Of Birth' is na 01-01-2000.", "nl-NL")]
    public void When_after_nullable_expression(Date dt, string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new Model { DateOfBirth = dt }
            .ValidateWith(new ExpressionNullableValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "DateOfBirth"));
    }

    [Test]
    [Arguments("2000-01-02", "'Date Of Death' is after 01/01/2000.", "en-GB")]
    [Arguments("2024-12-31", "'Date Of Death' is na 01-01-2000.", "nl-NL")]
    public void When_nullable_after_nullable_expression(Date dt, string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new Model { DateOfDeath = dt }
            .ValidateWith(new ExpressionNullableValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "DateOfDeath"));
    }
}

public sealed class Model
{
    public Date DateOfBirth { get; init; }

    public Date? DateOfDeath { get; init; }
}

internal sealed class FixedValidator : ModelValidator<Model>
{
    public FixedValidator()
    {
        RuleFor(m => m.DateOfBirth).NotAfter(new Date(2000, 01, 01));
        RuleFor(m => m.DateOfDeath).NotAfter(new Date(2000, 01, 01));
    }
}

internal sealed class ExpressionValidator : ModelValidator<Model>
{
    private static readonly Date Date = new(2000, 01, 01);

    public ExpressionValidator()
    {
        RuleFor(m => m.DateOfBirth).NotAfter(_ => Date);
        RuleFor(m => m.DateOfDeath).NotAfter(_ => Date);
    }
}

internal sealed class ExpressionNullableValidator : ModelValidator<Model>
{
    private static readonly Date? Date = new Date(2000, 01, 01);
    
    public ExpressionNullableValidator()
    {
        RuleFor(m => m.DateOfBirth).NotAfter(_ => Date);
        RuleFor(m => m.DateOfDeath).NotAfter(_ => Date);
    }
}
