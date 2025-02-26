using FluentAssertions.Execution;
using FluentAssertions.Types;

namespace FluentAssertions;

/// <summary>Extensions to assert type converter behavior.</summary>
internal static class QowaivTypeConverterAssertions
{
    /// <summary>Asserts that the type converter exists for the specified type.</summary>
    [CustomAssertion]
    public static AndConstraint<TypeAssertions> HaveTypeConverterDefined(this TypeAssertions assertions, string because = "", params object[] becauseArgs)
    {
        var converter = TypeDescriptor.GetConverter(assertions.Subject);

        Execute.Assertion
           .BecauseOf(because, becauseArgs)
           .ForCondition(converter.GetType() != typeof(TypeConverter))
           .FailWith($"There is no type converter defined for '{assertions.Subject}'.");

        return new AndConstraint<TypeAssertions>(assertions);
    }
}
