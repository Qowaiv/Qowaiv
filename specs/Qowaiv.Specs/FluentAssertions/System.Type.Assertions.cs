using FluentAssertions.Types;

namespace FluentAssertions;

internal static class QowaivSpecsTypeAssertions
{
    public static AndWhichConstraint<TypeAssertions, ConditionalAttribute> DecoratedWithConditionalAttribute(
        this TypeAssertions assertions,
        string condition,
        string because = "", params object[] becauseArgs)
    {
        var andWhich = assertions.Subject.Should().BeDecoratedWith<ConditionalAttribute>();
        andWhich.Which.ConditionString.Should().Be(condition, because, becauseArgs);
        return andWhich;
    }
}
