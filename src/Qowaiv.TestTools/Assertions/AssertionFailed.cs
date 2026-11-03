namespace Qowaiv.TestTools.Assertions;

/// <summary>Represents an assertion failure.</summary>
public class AssertionFailed : Exception
{
    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    public AssertionFailed() { }

    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    public AssertionFailed(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="AssertionFailed"/> class.</summary>
    public AssertionFailed(string message, Exception innerException) : base(message, innerException) { }
}
