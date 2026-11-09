using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.Is_finite_specs;

public class Is_valid_for
{
    [Test]
    public void Null() => new IsFiniteAttribute().IsValid(null).Should().BeTrue();

    [Arguments(double.MinValue)]
    [Arguments(0.33)]
    [Arguments(0.0)]
    [Arguments(42.0)]
    [Arguments(double.MaxValue)]
    [Arguments(float.MinValue)]
    [Arguments(0.33f)]
    [Arguments(0.0f)]
    [Arguments(42.0f)]
    [Arguments(float.MaxValue)]
    public void finite_floating_points(object value)
        => new IsFiniteAttribute().IsValid(value).Should().BeTrue();
}

public class Is_invalid_for
{
    [Arguments(float.NaN)]
    [Arguments(float.NegativeInfinity)]
    [Arguments(float.PositiveInfinity)]
    [Arguments(double.NaN)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(double.PositiveInfinity)]
    public void not_finite(object value)
        => new IsFiniteAttribute().IsValid(value).Should().BeFalse();

    [Arguments("Hello")]
    [Arguments(42)]
    [Arguments(128L)]
    [Arguments(1024UL)]
    public void non_floating_points(object value)
        => value.Invoking(_ => new IsFiniteAttribute().IsValid(value))
        .Should().Throw<UnsupportedType>();
}
public class With_message
{
    [Arguments("nl", "De waarde van het Total veld is geen eindig getal.")]
    [Arguments("en", "The value of the Total field is not a finite number.")]
    public void culture_dependent(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new Model().ValidateWith(new AnnotatedModelValidator<Model>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "Total"));
    }

    internal class Model
    {
        [IsFinite]
        public double Total { get; set; } = double.NaN;
    }
}

