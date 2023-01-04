using FluentAssertions.Execution;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using System.Reflection;

namespace FluentAssertions;

/// <summary>Extensions to assert debugger display behavior.</summary>
public static class QowaivDebuggerDisplayAssertions
{
    /// <summary>Verifies the outcome of the <see cref="DebuggerDisplayAttribute"/> of a certain <see cref="object"/>.</summary>
    [CLSCompliant(false)]
    [CustomAssertion]
    public static AndConstraint<ObjectAssertions> HaveDebuggerDisplay(
        this ObjectAssertions assertions,
        object display,
        string because = "",
        params object[] becauseArgs)
    {
        var prop = DebuggerDisplay(Guard.NotNull(assertions, nameof(assertions)).Subject?.GetType());

        if (Execute.Assertion
            .ForCondition(prop is not null)
            .FailWith($"'{assertions.Subject?.GetType()}' has no DebuggerDisplay defined"))
        {
            return prop!.GetValue(assertions.Subject).Should().Be(display, because, becauseArgs);
        }
        else return new AndConstraint<ObjectAssertions>(assertions);
    }

    /// <summary>Verifies the outcome of the <see cref="DebuggerDisplayAttribute"/> of a certain <see cref="object"/>.</summary>
    [CLSCompliant(false)]
    [CustomAssertion]
    public static AndConstraint<ComparableTypeAssertions<T>> HaveDebuggerDisplay<T>(
        this ComparableTypeAssertions<T> assertions,
        object display,
        string because = "",
        params object[] becauseArgs)
    {
        var prop = DebuggerDisplay(Guard.NotNull(assertions, nameof(assertions)).Subject?.GetType());

        if (Execute.Assertion
            .ForCondition(prop is not null)
            .FailWith($"'{assertions.Subject?.GetType()}' has no DebuggerDisplay defined"))
        {
           prop!.GetValue(assertions.Subject).Should().Be(display, because, becauseArgs);
        }
        return new AndConstraint<ComparableTypeAssertions<T>>(assertions);
    }


    [Pure]
    private static PropertyInfo? DebuggerDisplay(Type? type)
    {
        var prop = type?.GetProperty(nameof(DebuggerDisplay), NonPublicInstance);
        return prop is null && type?.BaseType is not null
            ? DebuggerDisplay(type.BaseType)
            : prop;
    }

    private const BindingFlags NonPublicInstance = (BindingFlags)36;
}
