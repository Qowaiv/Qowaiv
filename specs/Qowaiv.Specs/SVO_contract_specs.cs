using System.Numerics;

namespace SVO_contract_specs;

public class Implements : SingleValueObjectSpecs
{
    [TestCaseSource(nameof(AllSvos))]
    public void IEquatable(Type type) => type.Should().Implement(typeof(IEquatable<>).MakeGenericType(type));

    [TestCaseSource(nameof(AllSvos))]
    public void IComparable(Type type) => type.Should().Implement<IComparable>();

    [TestCaseSource(nameof(AllSvos))]
    public void IComparable_TSelf(Type type) => type.Should().Implement(typeof(IComparable<>).MakeGenericType(type));

    [TestCaseSource(nameof(AllSvos))]
    public void IFormattable(Type type) => type.Should().Implement<IFormattable>();

    [TestCaseSource(nameof(AllSvos))]
    public void ISerializable(Type type) => type.Should().Implement<ISerializable>();

    [TestCaseSource(nameof(AllSvos))]
    public void IXmlSerializable(Type type) => type.Should().Implement<IXmlSerializable>();

#if NET7_0_OR_GREATER
    [TestCaseSource(nameof(AllSvosExceptGeneric))]
    public void IEqualityOperators(Type type) => type.Should().Implement(typeof(IEqualityOperators<,,>).MakeGenericType(type, type, typeof(bool)));

    [TestCaseSource(nameof(AllSvosExceptGeneric))]
    public void IParsable(Type type) => type.Should().Implement(typeof(IParsable<>).MakeGenericType(type));
#endif
}
