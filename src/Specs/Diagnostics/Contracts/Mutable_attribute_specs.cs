namespace Diagnostics.Contracts.Mutable_attribute_specs;

internal class Can_decorate
{
    [Test]
    public void Classes()
        => typeof(SomeClass).Should().BeDecoratedWith<MutableAttribute>();
}

[Mutable("For test purposes")]
internal class SomeClass
{
}
