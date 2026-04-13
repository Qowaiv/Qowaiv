namespace Diagnostics.Contracts.Conditional_specs;

public class Is_decorated_with
{
    [TestCase(typeof(CollectionMutationAttribute))]
    [TestCase(typeof(FluentSyntaxAttribute))]
    [TestCase(typeof(ImpureAttribute))]
    [TestCase(typeof(InheritableAttribute))]
    [TestCase(typeof(MutableAttribute))]
    [TestCase(typeof(EmptyTypeAttribute))]
    [TestCase(typeof(EmptyClassAttribute))]
    [TestCase(typeof(EmptyEnumAttribute))]
    [TestCase(typeof(EmptyInterfaceAttribute))]
    [TestCase(typeof(EmptyStructAttribute))]
    [TestCase(typeof(EmptyTestClassAttribute))]
    [TestCase(typeof(EmptyTestEnumAttribute))]
    [TestCase(typeof(EmptyTestInterfaceAttribute))]
    [TestCase(typeof(EmptyTestStructAttribute))]
    public void condtional_CONTRACTS_FULL(Type attribute)
        => attribute.Should().BeDecoratedWith<ConditionalAttribute>()
            .Which.ConditionString.Should().Be("CONTRACTS_FULL");
}
