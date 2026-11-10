namespace Diagnostics.Contracts.Inheritable_attribute_specs;

internal class Can_decorate
{
    [Test]
    public void Classes()
        => typeof(SomeClass).Should().BeDecoratedWith<InheritableAttribute>();
}

[Inheritable("For test purposes")]
internal class SomeClass
{
}
