namespace Qowaiv.Validation.TestTools;

/// <summary>Thrown when an assertion fails.</summary>
public class AssertionFailed : Exception
{
    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    public AssertionFailed(string? message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    [ExcludeFromCodeCoverage(Justification = "Just for completeness.")]
    public AssertionFailed() { }

    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    [ExcludeFromCodeCoverage(Justification = "Just for completeness.")]
    public AssertionFailed(string? message, Exception? innerException) : base(message, innerException) { }
}
