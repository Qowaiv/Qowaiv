using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Annotation_specs;

public class Does_not_crash_on
{
    [Test]
    public void inaccessible_property()
        => new AnnotatedModelValidator<Model.With.InaccessibleProperty>()
        .Validate(new())
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The value is inaccessible.", "ThrowsOnGet"));

    [Test]
    public void indexed_property()
        => new AnnotatedModelValidator<Model.With.IndexedProperty>()
        .Validate(new())
        .Should().BeValid();

    [Test]
    public void set_only_property()
        => new AnnotatedModelValidator<Model.With.SetOnlyProperty>()
        .Validate(new())
        .Should().BeValid();

    [Test]
    public void non_generic_enumerables()
     => new AnnotatedModelValidator<Model.With.NonGenericEnumerable>()
        .Validate(new() { Items = ["one"] })
        .Should().BeValid();

    [Test]
    public void enumerables_without_count()
     => new AnnotatedModelValidator<Model.With.Enumerables>()
        .Validate(new())
        .Should().BeValid();
}

public class Resolves_property
{
    [Test]
    public void validates_attributes()
        => new Model.With.AnnotatedProperty() { Name = "An" }
        .ValidateAnnotations()
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The length of the Name field should be at least 3.", "Name"));
}

public class Resolves
{
    [Test]
    public void Display_attribute()
        => new Model.With.Display() { Prop = "Too long" }
        .ValidateAnnotations()
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The length of the Property field should be at most 2.", "Prop"));

    [Test]
    public void inherritable_member()
        => new Model.With.InheritableMember { Member = new Model.Inherited { Value = 17 } }
        .ValidateAnnotations()
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The value of the Value field is not allowed.", "Member.Value"));
}

public class Validates_recursive_models
{
    [Test]
    public void self_referencing_member()
        => new Model.With.SelfReference { Value = 42, Next = new() { Value = 17 } }
        .ValidateAnnotations()
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The value of the Value field is not allowed.", "Next.Value"));

    [Test]
    public void deeply_nested_self_references()
        => new Model.With.SelfReference { Value = 42, Next = new() { Value = 42, Next = new() { Value = 17 } } }
        .ValidateAnnotations()
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The value of the Value field is not allowed.", "Next.Next.Value"));

    [Test]
    public void terminates_on_instance_cycles()
    {
        var model = new Model.With.SelfReference { Value = 17 };
        model.Next = model;

        model.ValidateAnnotations()
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("The value of the Value field is not allowed.", "Value"));
    }

    [Test]
    public void mutually_referencing_members()
        => new Model.With.MutualA { B = new() { Value = 17 } }
        .ValidateAnnotations()
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The value of the Value field is not allowed.", "B.Value"));

    [Test]
    public void sealed_type_in_a_cycle()
        => new Model.With.UnsealedInner { Value = 42, Cycle = new() { Inner = new() { Value = 17 } } }
        .ValidateAnnotations()
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("The value of the Value field is not allowed.", "Cycle.Inner.Value"));
}

public class Ingores
{
    [Test]
    public void not_annotated_model()
        => new Model.Without.Annotations() { Required = null! }
            .ValidateAnnotations()
            .Should().BeValid()
            .WithoutMessages();

    [Test]
    public void required_attribute_on_value_type()
        => new Model.With.RequiredOnValueType()
            .ValidateAnnotations()
            .Should().BeInvalid()
            .WithMessages(
                ValidationMessage.Error("The Required field is required.", "Required"),
                ValidationMessage.Error("The Mandatory field is required.", "Mandatory"));
}
