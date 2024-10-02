namespace Diagnostics.Contracts.Conditional_specs;

public class Is_decorated_with
{
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.CollectionMutationAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.FluentSyntaxAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.ImpureAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.InheritableAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.MutableAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyTypeAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyClassAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyEnumAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyInterfaceAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyStructAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyTestClassAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyTestEnumAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyTestInterfaceAttribute))]
    [TestCase(typeof(Qowaiv.Diagnostics.Contracts.EmptyTestStructAttribute))]
    public void condtional_CONTRACTS_FULL(Type attribute)
        => attribute.Should().BeDecoratedWith<ConditionalAttribute>()
            .Which.ConditionString.Should().Be("CONTRACTS_FULL");
}
