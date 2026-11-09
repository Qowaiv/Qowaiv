using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.Guarding;

namespace Guarding.Must_specs;

public class Requires
{
    [Test]
    public void Subject_not_to_be_null()
    {
        Func<object> create = () => ((TestModel)null!).Must();
        create.Should().Throw<ArgumentNullException>();
    }
}

public class Continues_when
{
    [Test]
    public void Must_Be_condition_is_met()
    {
        var model = new TestModel();
        var result = model.Must().Be(condition: true, ValidationMessage.Error("This is wrong"));

        result.Should().BeValid().WithoutMessages()
            .And.Value.Should().BeSameAs(model);
    }

    [Test]
    public void Must_NotBe_condition_is_not_met()
    {
        var model = new TestModel();
        var result = model.Must().NotBe(condition: false, "This is wrong");

        result.Should().BeValid().WithoutMessages()
            .And.Value.Should().BeSameAs(model);
    }

    [Test]
    public void Must_Exist_resolves_entity()
    {
        var model = new TestModel();
        var result = model.Must().Exist(8, (_, _) => new object());

        result.Should().BeValid().WithoutMessages()
            .And.Value.Should().BeSameAs(model);
    }
}

public class Blocks_when
{
    [Test]
    public void Must_Be_condition_is_not_met()
    {
        var result = new TestModel().Must().Be(condition: false, ValidationMessage.Error("This is wrong"));
        result.Should().BeInvalid().WithMessage(ValidationMessage.Error("This is wrong"));
    }

    [Test]
    public void Must_NotBe_condition_is_met()
    {
        var result = new TestModel().Must().NotBe(condition: true, "This is wrong");
        result.Should().BeInvalid().WithMessage(ValidationMessage.Error("This is wrong"));
    }

    [Test]
    public void Must_Exist_does_not_resolve_entity()
    {
        var result = new TestModel().Must().Exist(666, (_, _) => (object)null!);
        result.Should().BeInvalid().WithMessage(ValidationMessage.Error("Entity with ID 666 could not be found."));
    }
}

public class ToString_as_type
{
    [Test]
    public void Displays_type_based_on_TSubject()
    {
        var result = new TestModel().Must();
        result.ToString().Should().Be("Must<Guarding.Must_specs.TestModel>");
    }
}

[Serializable]
internal class TestModel { }
