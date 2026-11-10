namespace Qowaiv.DomainModel.TestTools;

/// <summary>Assert exception.</summary>
/// <remarks>
/// Exists to be independent to external test frameworks.
/// </remarks>
public class AssertionFailed : Exception
{
    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    [ExcludeFromCodeCoverage/* Justification = "Required for inheritance only." */]
    public AssertionFailed() : this("Assertion failed.") { }

    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    /// <param name="message">
    /// The error message that explains the reason for this exception.
    /// </param>
    public AssertionFailed(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    /// <param name="message">
    /// The error message that explains the reason for this exception.
    /// </param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference
    ///  if no inner exception is specified.
    /// </param>
    [ExcludeFromCodeCoverage/* Justification = "Required for inheritance only." */]
    public AssertionFailed(string message, Exception innerException) : base(message, innerException) { }
}
