namespace Diagnostics.Contracts.Conditional_specs;

public class Is_decorated_with
{
    [Test]
    [Arguments(typeof(CollectionMutationAttribute))]
    [Arguments(typeof(FluentSyntaxAttribute))]
    [Arguments(typeof(ImpureAttribute))]
    [Arguments(typeof(InheritableAttribute))]
    [Arguments(typeof(MutableAttribute))]
    [Arguments(typeof(EmptyTypeAttribute))]
    [Arguments(typeof(EmptyClassAttribute))]
    [Arguments(typeof(EmptyEnumAttribute))]
    [Arguments(typeof(EmptyInterfaceAttribute))]
    [Arguments(typeof(EmptyStructAttribute))]
    [Arguments(typeof(EmptyTestClassAttribute))]
    [Arguments(typeof(EmptyTestEnumAttribute))]
    [Arguments(typeof(EmptyTestInterfaceAttribute))]
    [Arguments(typeof(EmptyTestStructAttribute))]
    public void condtional_CONTRACTS_FULL(Type attribute)
        => attribute.Should().BeDecoratedWith<ConditionalAttribute>()
            .Which.ConditionString.Should().Be("CONTRACTS_FULL");
}
