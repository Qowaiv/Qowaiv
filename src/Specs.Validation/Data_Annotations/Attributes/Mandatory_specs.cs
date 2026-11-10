using Qowaiv.Financial;
using Qowaiv.Validation.DataAnnotations;
using Specs;
using System.ComponentModel.DataAnnotations;

namespace Data_annotations.Attributes.Mandatory_specs;

public class Requires
{
    [Test]
    public void ValidationContext()
        => new MandatoryAttribute().RequiresValidationContext.Should().BeTrue();
}
public class Is_valid_for
{
    [Test]
    public void NewGuid()
        => new MandatoryAttribute().IsValid(Guid.NewGuid()).Should().BeTrue();

    [Test]
    public void Known_EmailAddress()
        => new MandatoryAttribute().IsValid(EmailAddress.Parse("test@exact.com")).Should().BeTrue();

    [Test]
    public void EmailAddress_Unknown_when_unknown_values_are_allowed()
        => new MandatoryAttribute { AllowUnknownValue = true }.IsValid(EmailAddress.Unknown).Should().BeTrue();

    [Test]
    public void ReferenceType_without_parameterless_ctor()
        => new MandatoryAttribute().IsValid(new Uri("https://qowaiv.org"))
        .Should().BeTrue();

    [Test]
    public void ReferenceType_without_Unknown_value()
        => new MandatoryAttribute{ AllowUnknownValue = false }.IsValid(new object())
        .Should().BeTrue();

    [Test]
    public void Record_in_default_state()
        => new MandatoryAttribute().IsValid(new AllDefault()).Should().BeTrue();
}
public class Is_not_valid_for
{
    [Test]
    public void Guid_Empty()
        => new MandatoryAttribute().IsValid(Guid.Empty).Should().BeFalse();

    [Test]
    public void EmailAddress_Empty()
        => new MandatoryAttribute().IsValid(EmailAddress.Empty).Should().BeFalse();

    [Test]
    public void EmailAddress_Unknown()
      => new MandatoryAttribute().IsValid(EmailAddress.Unknown).Should().BeFalse();
}
public class Nullable_value_types
{
    [Test]
    [Culture("en-GB")]
    public void that_is_null_is_not_valid()
        => new Model().ValidateWith(new AnnotatedModelValidator<Model>())
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The Value field is required.", "Value"));

    [Test]
    public void with_a_value_is_valid()
        => new Model { Value = 0 }.ValidateWith(new AnnotatedModelValidator<Model>())
        .Should().BeValid();
}

file sealed class Model
{
    [Mandatory]
    public int? Value { get; init; }
}

public class With_message
{
    [Test]
    [Arguments("nl-NL", "Het veld TestField is verplicht.")]
    [Arguments("en-GB", "The TestField field is required.")]
    public void culture_dependent(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new AnnotatedModelValidator<Model>().Validate(new Model { ReferenceField = "ignore" })
            .Should().BeInvalid().WithMessage(ValidationMessage.Error(message, "TestField"));
    }

    [Test]
    [Culture("en-GB")]
    public void equal_to_required_message()
        => new AnnotatedModelValidator<Model>().Validate(new Model())
            .Should().BeInvalid().WithMessages(
                ValidationMessage.Error("The TestField field is required.", "TestField"),
                ValidationMessage.Error("The ReferenceField field is required.", "ReferenceField"));

    [Test]
    [Culture("en-GB")]
    public void supporting_custom_error_resource_type()
        => new WithCustomErrorMessageType().ValidateWith(new AnnotatedModelValidator<WithCustomErrorMessageType>())
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("This Iban is wrong.", "Iban"));

    [Test]
    [Culture("en-GB")]
    public void supporting_custom_display()
        => new WithCustomDisplay().ValidateWith(new AnnotatedModelValidator<WithCustomDisplay>())
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The IBAN field is required.", "Iban"));

    [Test]
    public void supports_custom_error_message()
    {
        var attr = new MandatoryAttribute
        {
            ErrorMessage = "Custom error message" 
        };

        attr.GetValidationMessage(Guid.Empty, new(new object()))!
            .Message.Should().Be("Custom error message");
    }

    internal class Model
    {
        [Mandatory]
        public string? TestField { get; }

        [Required]
        public string? ReferenceField { get; set; }

        [Mandatory] // Is valid as it is a nullable.
        public decimal? Nullable { get; init; } = 0;
    }

    public class WithCustomErrorMessageType
    {
        [Mandatory(ErrorMessageResourceType = typeof(TestMessages), ErrorMessageResourceName = "TestError")]
        public InternationalBankAccountNumber Iban { get; set; }
    }

    public class WithCustomDisplay
    {
        [Mandatory]
        [Display(Name = "IBAN")]
        public InternationalBankAccountNumber Iban { get; set; }
    }
}

file sealed record AllDefault;
