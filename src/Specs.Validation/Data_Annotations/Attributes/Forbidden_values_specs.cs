using Qowaiv.Financial;
using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.Forbidden_values_specs;

public class Is_valid_for
{
    public class Generic
    {
        [Test]
        public void Null()
            => new ForbiddenAttribute<Country>("DE", "FR", "GB").IsValid(null).Should().BeTrue();

        [Test]
        public void value_not_in_allowed_values()
            => new ForbiddenAttribute<Country>("DE", "FR", "GB").IsValid(Country.TR).Should().BeTrue();

        [Test]
        public void based_on_other_than_string()
            => new ForbiddenAttribute<Amount>(12.00, 17.23).IsValid(64.28m.Amount()).Should().BeTrue();
    }

    public class Non_generic
    {
        [Test]
        public void Null()
            => new ForbiddenValuesAttribute("DE", "FR", "GB").IsValid(null).Should().BeTrue();

        [Test]
        public void value_not_in_allowed_values()
            => new ForbiddenValuesAttribute("DE", "FR", "GB").IsValid(Country.TR).Should().BeTrue();
    }
}

public class Is_not_valid_for
{
    public class Generic
    {
        [Test]
        public void value_in_allowed_values()
       => new ForbiddenAttribute<Country>("DE", "FR", "GB").IsValid(Country.GB).Should().BeFalse();
    }

    public class Non_generic
    {
        [Test]
        public void value_in_allowed_values()
       => new ForbiddenValuesAttribute("DE", "FR", "GB").IsValid(Country.GB).Should().BeFalse();
    }
}

public class With_message
{
    [Test]
    [Arguments("nl", "De waarde van het veld Country is niet toegestaan.")]
    [Arguments("en", "The value of the Country field is not allowed.")]
    public void culture_dependent(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new Model().ValidateWith(new AnnotatedModelValidator<Model>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "Country"));
    }
    internal class Model
    {
        [Forbidden<Country>("DE", "FR", "GB")]
        public Country Country { get; set; } = Country.DE;
    }
}
