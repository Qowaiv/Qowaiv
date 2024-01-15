using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Qowaiv.TestTools.Generation;

public static class FluentAssertionExtensions
{
    public static AndConstraint<StringAssertions> BeTrimmed(this StringAssertions assertions)
    {
        var display = assertions.Subject;
        Execute.Assertion
            .ForCondition(display.Trim() == display)
            .FailWith($"DisplayName '{display}' is not trimmed.");

        return new(assertions);
    }
}
