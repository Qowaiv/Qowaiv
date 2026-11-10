namespace Diagnostics.Contracts.Empty_type_attribute_specs;

public class Can_decorate_empty
{
    [Test]
    public void Classes()
       => typeof(SomeEmptyClass).Should().BeDecoratedWith<EmptyTestClassAttribute>();

    [Test]
    public void Enums()
        => typeof(SomeEmptyEnumeration).Should().BeDecoratedWith<EmptyTestEnumAttribute>();

    [Test]
    public void Interfaces()
        => typeof(ISomeEmptyInterface).Should().BeDecoratedWith<EmptyTestInterfaceAttribute>();

    [Test]
    public void Structs()
        => typeof(SomeEmptyStruct).Should().BeDecoratedWith<EmptyTestStructAttribute>();
}

[EmptyTestClass]
internal class SomeEmptyClass { }

[EmptyTestEnum]
internal enum SomeEmptyEnumeration { }

[EmptyTestInterface]
internal interface ISomeEmptyInterface { }

[EmptyTestStruct]
internal struct SomeEmptyStruct { }
