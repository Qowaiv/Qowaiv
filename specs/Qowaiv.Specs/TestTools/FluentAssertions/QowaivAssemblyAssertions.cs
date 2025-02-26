using FluentAssertions.Execution;
using FluentAssertions.Reflection;

namespace FluentAssertions;

/// <summary>Extensions on <see cref="AssemblyAssertions" />.</summary>
internal static class QowaivAssemblyAssertions
{
    /// <summary>Asserts the <see cref="Assembly" /> to have a specific public key.</summary>
    [CustomAssertion]
    public static AndConstraint<AssemblyAssertions> HavePublicKey(this AssemblyAssertions assertions, string publicKey, string because = "", params object[] becauseArgs)
    {
        var bytes = assertions.Subject.GetName().GetPublicKey() ?? [];

#pragma warning disable CA1872 // Prefer 'Convert.ToHexString' and 'Convert.ToHexStringLower' over call chains based on 'BitConverter.ToString'
        // Also targets .NET 5.0 that does not has this implementation.
        var assemblyKey = BitConverter.ToString(bytes).Replace("-", string.Empty);
#pragma warning restore CA1872

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(assemblyKey == publicKey)
            .FailWith(
            $"Expected '{assertions.Subject}' to have public key: {publicKey},{Environment.NewLine}" +
            $"but got: {assemblyKey} instead.");

        return new AndConstraint<AssemblyAssertions>(assertions);
    }
}
