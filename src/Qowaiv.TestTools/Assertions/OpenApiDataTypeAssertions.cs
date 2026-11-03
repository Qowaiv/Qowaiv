using Qowaiv.OpenApi;

namespace Qowaiv.TestTools.Assertions;

/// <summary>Specialized assertions for <see cref="OpenApiDataType"/>.</summary>
public sealed class OpenApiDataTypeAssertions(OpenApiDataType? subject)
{
    /// <summary>The subject under test.</summary>
    public OpenApiDataType? Subject { get; } = subject;

    /// <summary>Asserts that the subject is equal to the expected <see cref="OpenApiDataType"/>.</summary>
    /// <exception cref="AssertionFailed">
    /// When the subject is not equal to the expected <see cref="OpenApiDataType"/>.
    /// </exception>
    public void Be(OpenApiDataType expected)
    {
        Guard.NotNull(expected);

        if (Subject is null) Assertion.New()
            .Line("Expected Open API data type to be")
            .Value(expected)
            .Line(',')
            .Append("but found null.")
            .Fail();

        if (!Subject.Equals(expected)) Assertion.New()
            .Line("Expected Open API data type to be")
            .Value(expected)
            .Line(',')
            .Line("but found")
            .Value(Subject)
            .Append('.')
            .Fail();
    }
}
