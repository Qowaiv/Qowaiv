namespace Qowaiv.Validation.TestTools;

/// <summary>Contains a number of methods to assert the messages of a valid <see cref="Result"/>.</summary>
public sealed class ResultValidnessAssertions : ResultValidnessAssertionsBase<Result>
{
    internal ResultValidnessAssertions(Result subject, string? expression) : base(subject, expression) { }

    /// <summary>Asserts that the <see cref="Result"/> contains no messages.</summary>
    [Assertion]
    public void WithoutMessages() => ExecuteWithoutMessages();

    /// <summary>Asserts that the <see cref="Result"/> contains the specified message.</summary>
    [Assertion]
    public void WithMessage(IValidationMessage message) => ExecuteWithMessage(message);

    /// <summary>Asserts that the <see cref="Result"/> contains the specified messages.</summary>
    [Assertion]
    public void WithMessages(params IValidationMessage[] messages)
        => ExecuteWithMessages(messages);
}
