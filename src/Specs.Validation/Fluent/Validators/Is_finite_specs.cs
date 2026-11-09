using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.Is_finite_specs;

public class Valid_for
{
    [Test]
    [Arguments(double.MinValue)]
    [Arguments(0.33)]
    [Arguments(0)]
    [Arguments(42)]
    [Arguments(double.MaxValue)]
    public void finite_doubles(double value)
        => new FloatingPointsModel { Double = value }
        .ValidateWith(new FloatingPointsValidator())
        .Should().BeValid();

    [Test]
    [Arguments(double.MinValue)]
    [Arguments(0.33f)]
    [Arguments(0)]
    [Arguments(42)]
    [Arguments(double.MaxValue)]
    public void finite_nullable_doubles(double value)
        => new FloatingPointsModel { NullableDouble = value }
        .ValidateWith(new FloatingPointsValidator())
        .Should().BeValid();

    [Test]
    [Arguments(float.MinValue)]
    [Arguments(0.33f)]
    [Arguments(0)]
    [Arguments(42)]
    [Arguments(float.MaxValue)]
    public void finite_floats(float value)
        => new FloatingPointsModel { Float = value }
        .ValidateWith(new FloatingPointsValidator())
        .Should().BeValid();

    [Test]
    [Arguments(float.MinValue)]
    [Arguments(0.33f)]
    [Arguments(0)]
    [Arguments(42)]
    [Arguments(float.MaxValue)]
    public void finite_nullable_floats(float value)
        => new FloatingPointsModel { NullableFloat = value }
        .ValidateWith(new FloatingPointsValidator())
        .Should().BeValid();

    [Test]
    public void not_set_nullable_double()
        => new FloatingPointsModel { NullableFloat = null }
        .ValidateWith(new FloatingPointsValidator())
        .Should().BeValid();

    [Test]
    public void not_set_nullable_float()
       => new FloatingPointsModel { NullableFloat = null }
       .ValidateWith(new FloatingPointsValidator())
       .Should().BeValid();
}

public class Invalid_for
{
    [Test]
    [Arguments(double.NaN)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(double.PositiveInfinity)]
    public void not_finite_doubles(double value)
    {
        using var _ = CultureInfo.InvariantCulture.Scoped();

        new FloatingPointsModel { Double = value, NullableDouble = value }
            .ValidateWith(new FloatingPointsValidator())
            .Should().BeInvalid()
            .WithMessages(
                ValidationMessage.Error("'Double' must be a finite number.", "Double"),
                ValidationMessage.Error("'Nullable Double' must be a finite number.", "NullableDouble"));
    }

    [Test]
    [Arguments(float.NaN)]
    [Arguments(float.NegativeInfinity)]
    [Arguments(float.PositiveInfinity)]
    public void not_finite_floats(float value)
    {
        using var _ = CultureInfo.InvariantCulture.Scoped();

        new FloatingPointsModel { Float = value, NullableFloat = value }
            .ValidateWith(new FloatingPointsValidator())
            .Should().BeInvalid()
            .WithMessages(
                ValidationMessage.Error("'Float' must be a finite number.", "Float"),
                ValidationMessage.Error("'Nullable Float' must be a finite number.", "NullableFloat"));
    }

    [Test]
    [Arguments("nl", "'Double' moet een eindig getal zijn.")]
    [Arguments("en", "'Double' must be a finite number.")]
    public void with_localized_message(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();

        new FloatingPointsModel { Double = double.NaN }
            .ValidateWith(new FloatingPointsValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "Double"));
    }
}
