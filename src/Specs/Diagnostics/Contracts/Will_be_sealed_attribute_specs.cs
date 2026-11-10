namespace Diagnostics.Contracts.Will_be_sealed_attribute_specs;

internal class Can_decorate
{
    [Test]
    public void Classes()
        => typeof(SomeClass).Should().BeDecoratedWith<WillBeSealedAttribute>();

    [Test]
    public void Properties()
       => typeof(SomeClass).GetProperty(nameof(SomeClass.Property)).Should().BeDecoratedWith<WillBeSealedAttribute>();

    [Test]
    public void Methods()
        => typeof(SomeClass).GetMethod(nameof(SomeClass.Method)).Should().BeDecoratedWith<WillBeSealedAttribute>();
}

[WillBeSealed("For test purposes")]
internal class SomeClass
{
    [WillBeSealed("No reason to change this property.")]
    public virtual int Property { get; } = 42;

    [WillBeSealed("No reason to change this method.")]
    public virtual int Method() => 42;
}
