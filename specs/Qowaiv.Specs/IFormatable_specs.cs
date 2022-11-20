namespace IFormatable_specs;

public class All_SVOs : SvoTypeTest
{
    [TestCaseSource(nameof(AllSvos))]
    public void implement_IFormattable(Type type) => type.Should().Implement(typeof(IFormattable));

#if NET6_0_OR_GREATER
    [TestCaseSource(nameof(AllSvos))]
    public void implement_ISpanFormattable(Type type) => type.Should().Implement(typeof(ISpanFormattable));
#endif
}
