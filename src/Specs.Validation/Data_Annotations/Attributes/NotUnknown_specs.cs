using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.NotUnknown_specs;

public class Is_valid_for
{
    [Test]
    public void Null()
        => new NotUnknownAttribute().IsValid(null).Should().BeTrue();

    [Test]
    public void Known_value()
        => new NotUnknownAttribute().IsValid(EmailAddress.Parse("test@exact.com")).Should().BeTrue();
}
public class Is_not_valid_for
{
    [Test]
    public void Unknown_value()
        => new NotUnknownAttribute().IsValid(EmailAddress.Unknown).Should().BeFalse();
}
public class With_message
{
    [Arguments("nl-NL", "Het veld Property mag niet onbekend zijn.")]
    [Arguments("en-GB", "The field Property must not be unknown.")]
    public void culture_dependent(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new AnnotatedModelValidator<Model>().Validate(new Model { Property = EmailAddress.Unknown })
            .Should().BeInvalid().WithMessage(ValidationMessage.Error(message, "Property"));
    }

    internal class Model
    {
        [NotUnknown]
        public EmailAddress Property { get; init; }
    }
}
