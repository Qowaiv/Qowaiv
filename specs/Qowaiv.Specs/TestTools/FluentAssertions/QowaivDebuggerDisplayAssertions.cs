using AwesomeAssertions.Numeric;
using AwesomeAssertions.Primitives;

namespace AwesomeAssertions;

/// <summary>Extensions to assert debugger display behavior.</summary>
internal static class QowaivDebuggerDisplayAssertions
{
    /// <summary>Verifies the outcome of the <see cref="DebuggerDisplayAttribute" /> of a certain <see cref="object" />.</summary>
    [CustomAssertion]
    public static AndConstraint<ObjectAssertions> HaveDebuggerDisplay(
        this ObjectAssertions assertions,
        object display,
        string because = "",
        params object[] becauseArgs)
    {
        var prop = DebuggerDisplay(assertions.Subject.GetType());

        assertions.CurrentAssertionChain
            .ForCondition(prop is { })
            .FailWith($"'{assertions.Subject.GetType()}' has no DebuggerDisplay defined.");


        prop!.GetValue(assertions.Subject).Should().Be(display, because, becauseArgs);

        return new AndConstraint<ObjectAssertions>(assertions);
    }

    /// <summary>Verifies the outcome of the <see cref="DebuggerDisplayAttribute" /> of a certain <see cref="object" />.</summary>
    [CustomAssertion]
    public static AndConstraint<ComparableTypeAssertions<T>> HaveDebuggerDisplay<T>(
        this ComparableTypeAssertions<T> assertions,
        object display,
        string because = "",
        params object[] becauseArgs)
    {
        var prop = DebuggerDisplay(assertions.Subject.GetType());

        assertions.CurrentAssertionChain
            .ForCondition(prop is { })
            .FailWith($"'{assertions.Subject.GetType()}' has no DebuggerDisplay defined");

        prop!.GetValue(assertions.Subject).Should().Be(display, because, becauseArgs);

        return new AndConstraint<ComparableTypeAssertions<T>>(assertions);
    }

    [Pure]
    private static PropertyInfo? DebuggerDisplay(Type type)
    {
        var prop = type.GetProperty(nameof(DebuggerDisplay), NonPublicInstance);
        return prop is null && type.BaseType is { }
            ? DebuggerDisplay(type.BaseType)
            : prop;
    }

    private const BindingFlags NonPublicInstance = (BindingFlags)36;
}
