namespace Qowaiv.Validation.TestTools;

/// <summary>Contains a number of methods to assert the messages of an invalid <see cref="Result"/>.</summary>
public sealed class ResultInvalidnessAssertions : ResultValidnessAssertionsBase<Result>
{
    /// <summary>Initializes a new instance of the <see cref="ResultInvalidnessAssertions"/> class.</summary>
    internal ResultInvalidnessAssertions(Result subject, string? expression) : base(subject, expression) { }

    /// <summary>Asserts that the <see cref="Result"/> contains the specified message.</summary>
    [Assertion]
    public void WithMessage(IValidationMessage message) => ExecuteWithMessage(message);

    /// <summary>Asserts that the <see cref="Result"/> contains the specified messages.</summary>
    [Assertion]
    public void WithMessages(params IValidationMessage[] messages)
        => ExecuteWithMessages(messages);
}
