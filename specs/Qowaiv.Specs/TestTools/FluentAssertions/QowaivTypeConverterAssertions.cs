using AwesomeAssertions.Types;

namespace AwesomeAssertions;

/// <summary>Extensions to assert type converter behavior.</summary>
internal static class QowaivTypeConverterAssertions
{
    /// <summary>Asserts that the type converter exists for the specified type.</summary>
    [CustomAssertion]
    public static AndConstraint<TypeAssertions> HaveTypeConverterDefined(this TypeAssertions assertions, string because = "", params object[] becauseArgs)
    {
        var converter = TypeDescriptor.GetConverter(assertions.Subject);

        assertions.CurrentAssertionChain
           .BecauseOf(because, becauseArgs)
           .ForCondition(converter.GetType() != typeof(TypeConverter))
           .FailWith($"There is no type converter defined for '{assertions.Subject}'.");

        return new AndConstraint<TypeAssertions>(assertions);
    }

    /// <summary>Asserts that the type converter exists for the specified type.</summary>
    [CustomAssertion]
    public static AndConstraint<TypeAssertions> HaveNoTypeConverterDefined(this TypeAssertions assertions, string because = "", params object[] becauseArgs)
    {
        var converter = TypeDescriptor.GetConverter(assertions.Subject);

        assertions.CurrentAssertionChain
           .BecauseOf(because, becauseArgs)
           .ForCondition(converter.GetType() == typeof(TypeConverter))
           .FailWith($"There is type converter defined for '{assertions.Subject}' ({converter.GetType().FullName}).");

        return new AndConstraint<TypeAssertions>(assertions);
    }
}
