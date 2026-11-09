using ValidationSeverity = Qowaiv.Validation.Abstractions.ValidationSeverity;
using Specs.DataAnnotations.Stubs;
using System.ComponentModel.DataAnnotations;
using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Validator_specs;

public class Validates_children_when_child_type
{
    [Test]
    public void is_IValidatableObject()
        => new WithIValidatableChild()
        .ValidateWith(new AnnotatedModelValidator<WithIValidatableChild>())
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("Answer to the Ultimate Question of Life, the Universe, and Everything.", "Child.Answer"));
  
    [Test]
    public void Has_any_property_with_validation_attribute()
        => new WithChildWithAttributes()
        .ValidateWith(new AnnotatedModelValidator<WithChildWithAttributes>())
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The Answer field is required.", "Child.Answer"));
  
    internal class WithIValidatableChild
    {
        public IValidatableChild Child { get; } = new();
    }
   
    internal class WithChildWithAttributes
    {
        public ChildWithAttributes Child { get; } = new();
    }

    internal class IValidatableChild : IValidatableObject
    {
        public int Answer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Answer != 42) yield return ValidationMessage.Error("Answer to the Ultimate Question of Life, the Universe, and Everything.", nameof(Answer));
        }
    }

    internal class ChildWithAttributes
    {
        [Mandatory]
        public int? Answer { get; set; }
    }
}

public class Supports
{
    [Test]
    public void dependency_injection()
    {
        var model = new WithDI { Answer = 42 };
        var provider = new ServiceProviderStub
        {
            { typeof(AnswerService), new AnswerService(42) }
        };
        model.ValidateWith(new AnnotatedModelValidator<WithDI>(provider))
            .Should().BeValid();
    }

    [Test]
    public void empty_service_provider_by_default()
        => new WithDI().ValidateWith(new AnnotatedModelValidator<WithDI>()).Should().BeValid();

    [Test]
    public void fields()
    => new Model.With.Fields()
        .ValidateAnnotations()
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The value of the Answer field is not allowed.", "Answer"));

    [Test]
    public void models_with_circularity()
    {
        var model = new WithLoop();
        model.Child.Parent = model;
        model.ValidateWith(new AnnotatedModelValidator<WithLoop>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("The Answer field is required.", "Child.Answer"));
    }

    [Test]
    public void models_with_collections_of_children_to_validate()
    {
        var model = new WithChildren
        {
            Records = [new ChildRecord(null), new ChildRecord(null)],
        };
        model.ValidateWith(new AnnotatedModelValidator<WithChildren>())
            .Should().BeInvalid()
            .WithMessages(
                ValidationMessage.Error("The Value field is required.", "Records[0].Value"),
                ValidationMessage.Error("The Value field is required.", "Records[1].Value"));
    }

    [Arguments(ValidationSeverity.Info)]
    [Arguments(ValidationSeverity.Warning)]
    [Arguments(ValidationSeverity.Error)]
    public void all_severity_levels(ValidationSeverity severity)
    {
        var model = new WithSeverity(severity);
        var validator = new AnnotatedModelValidator<WithSeverity>();
        var result = validator.Validate(model).Messages.Single();
        result.Severity.Should().Be(severity);
        result.Message.Should().Be("Has custom severity.");
    }

    [Test]
    public void severity_none_as_no_validation_result()
        => new WithSeverity(ValidationSeverity.None)
        .ValidateWith(new AnnotatedModelValidator<WithSeverity>())
        .Should().BeValid()
        .WithoutMessages();

    internal class WithDI : IValidatableObject
    {
        public int? Answer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var service = validationContext.GetService<AnswerService>();
            if (Answer != service?.Answer) yield return ValidationMessage.Error("Wrong answer!", nameof(Answer));
        }
    }

    internal class WithLoop
    {
        public ChildWithLoop Child { get; } = new();
    }
    internal class ChildWithLoop
    {
        public WithLoop? Parent { get; set; }
        
        [Mandatory]
        public int? Answer { get; set; }
    }

    internal class WithChildren
    {
        public ChildRecord[]? Records { get; set; }
    }
    internal sealed record ChildRecord([property: Mandatory]int? Value);

    internal sealed record AnswerService(int Answer);

    internal record WithSeverity(ValidationSeverity Severity) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return (ValidationMessage)ValidationMessage.For(Severity, "Has custom severity.")!;
        }
    }
}
