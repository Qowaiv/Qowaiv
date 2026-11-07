namespace AwesomeAssertions;

public static class QowaivTypeAssertions
{
    extension(TypeAssertions assertions)
    {
        /// <summary>Asserts that the type converter exists for the specified type.</summary>
        [CustomAssertion]
        public AndConstraint<TypeAssertions> HaveTypeConverterDefined(string because = "", params object[] becauseArgs)
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
        public AndConstraint<TypeAssertions> HaveNoTypeConverterDefined(string because = "", params object[] becauseArgs)
        {
            var converter = TypeDescriptor.GetConverter(assertions.Subject);

            assertions.CurrentAssertionChain
               .BecauseOf(because, becauseArgs)
               .ForCondition(converter.GetType() == typeof(TypeConverter))
               .FailWith($"There is a type converter defined for '{assertions.Subject}' ({converter.GetType().FullName}).");

            return new AndConstraint<TypeAssertions>(assertions);
        } 
    }
}
