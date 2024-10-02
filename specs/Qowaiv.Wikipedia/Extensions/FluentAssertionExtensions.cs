using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Qowaiv.TestTools.Generation;

public static class FluentAssertionExtensions
{
    public static AndConstraint<StringAssertions> MatchWikipedia(this StringAssertions assertions, string? wikipedia)
    {
        var display = assertions.Subject;
        Execute.Assertion
            .ForCondition(display == wikipedia)
            .FailWith($"DisplayName: '{display}', Wiki: {wikipedia ?? "<Not found>"}.");

        return new(assertions);
    }

    public static AndConstraint<StringAssertions> BeTrimmed(this StringAssertions assertions)
    {
        var display = assertions.Subject;
        Execute.Assertion
            .ForCondition(display.Trim() == display)
            .FailWith($"DisplayName '{display}' is not trimmed.");

        return new(assertions);
    }

    public static AndConstraint<StringAssertions> BeArabic(this StringAssertions assertions)
    {
        var display = assertions.Subject;
        var nonArabic = Regex.Match(display, @"[^\p{IsArabic} _]+");

        Execute.Assertion
            .ForCondition(!nonArabic.Success)
            .FailWith($"DisplayName '{display}' contains non-arabic characters: {nonArabic.Value}.");

        return new(assertions);
    }
}
