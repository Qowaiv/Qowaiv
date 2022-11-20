namespace SVO_contract_specs;

public class Implements : SingleValueObjectSpecs
{
    [TestCaseSource(nameof(AllSvos))]
    public void IFormattable(Type type) => type.Should().Implement(typeof(IFormattable));

    [TestCaseSource(nameof(AllSvos))]
    public void ISerializable(Type type) => type.Should().Implement(typeof(ISerializable));

    [TestCaseSource(nameof(AllSvos))]
    public void IXmlSerializable(Type type) => type.Should().Implement(typeof(IXmlSerializable));

    [TestCaseSource(nameof(AllSvos))]
    public void IComparable(Type type) => type.Should().Implement(typeof(IComparable));
}
