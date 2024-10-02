namespace Diagnostics.Contracts.Impure_attribute_specs;

public class Can_decorate_methods_with
{
    [Test]
    public void Collection_mutation_attribute()
       => typeof(SomeClass).GetMethod(nameof(SomeClass.CollectionMutation)).Should().BeDecoratedWith<CollectionMutationAttribute>();

    [Test]
    public void Impure_attribute()
        => typeof(SomeClass).GetMethod(nameof(SomeClass.Impure)).Should().BeDecoratedWith<ImpureAttribute>();

    [Test]
    public void Fluent_syntax_attribute()
        => typeof(SomeClass).GetMethod(nameof(SomeClass.FluentSyntax)).Should().BeDecoratedWith<FluentSyntaxAttribute>();
}

internal class SomeClass
{
    [CollectionMutation("It just returns if addition worked.")]
    public bool CollectionMutation(HashSet<SomeClass> set) => set.Add(this);
    
    [Impure("It has side effects.")]
    public static int Impure() => 42;

    [FluentSyntax("We like fluent syntaxes.")]
    public SomeClass FluentSyntax() => this;
}
