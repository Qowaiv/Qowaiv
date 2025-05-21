using Microsoft.Testing.Platform.Capabilities.TestFramework;
using Qowaiv.CodeGeneration;

namespace CodeGeneration.Namespace_specs;

public class Parent
{
    [Test]
    public void Empty_for_Empty()
        => Namespace.Empty.Parent.Should().Be(Namespace.Empty);

    [Test]
    public void Empty_for_root()
        => new Namespace("Qowaiv").Parent.Should().Be(Namespace.Empty);

    [Test]
    public void Resolvable_for_others()
        => new Namespace("Qowaiv.CodeGeneration").Parent.Should().Be(new Namespace("Qowaiv"));
}

public class Child
{
    [Test]
    public void Root_for_Empty()
        => Namespace.Empty.Child("Qowaiv").Should().Be(new Namespace("Qowaiv"));

    [Test]
    public void Resolvable_for_others()
        => new Namespace("Qowaiv").Child("CodeGeneration").Should().Be(new Namespace("Qowaiv.CodeGeneration"));
}

public class Formattable
{
    [Test]
    public void Empty_is_string_Empty() => Namespace.Empty.ToString().Should().BeEmpty();

    [Test]
    public void ToString_represents_name() => new Namespace("Qowaiv").ToString().Should().Be("Qowaiv");
}

public class Convertable
{
    [TestCase("")]
    [TestCase(null)]
    public void From_empty_strings_with_casting(string? str)
    {
        Namespace ns = str;
        ns.Should().Be(Namespace.Empty);
    }

    [Test]
    public void From_string_with_casting()
    {
        Namespace ns = "Qowaiv";
        ns.Should().Be(new Namespace("Qowaiv"));
    }
}
