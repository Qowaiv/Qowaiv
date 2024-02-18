namespace Diagnostics.Contracts.Conditional_specs;

public class Is_decorated_with_condtional_CONTRACTS_FULL
{
    [Test]
    public void Collection_mutation_attribute()
        => typeof(Qowaiv.Diagnostics.Contracts.CollectionMutationAttribute)
            .Should().DecoratedWithConditionalAttribute("CONTRACTS_FULL");

    [Test]
    public void Fluent_syntax_attribute()
       => typeof(Qowaiv.Diagnostics.Contracts.FluentSyntaxAttribute)
           .Should().DecoratedWithConditionalAttribute("CONTRACTS_FULL");

    [Test]
    public void Impure_attribute()
       => typeof(Qowaiv.Diagnostics.Contracts.ImpureAttribute)
           .Should().DecoratedWithConditionalAttribute("CONTRACTS_FULL");

    [Test]
    public void Inheritable_attribute()
       => typeof(Qowaiv.Diagnostics.Contracts.InheritableAttribute)
           .Should().DecoratedWithConditionalAttribute("CONTRACTS_FULL");

    [Test]
    public void Mutable_attribute()
       => typeof(Qowaiv.Diagnostics.Contracts.MutableAttribute)
           .Should().DecoratedWithConditionalAttribute("CONTRACTS_FULL");
}
