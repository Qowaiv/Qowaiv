namespace Qowaiv.Validation.TestTools;

/// <summary>Contains a number of methods to assert the messages or value of a valid <see cref="Result{TModel}"/>.</summary>
public sealed class ResultValidnessMessageAssertions<TModel> : ResultValidnessAssertions<TModel>
{
    internal ResultValidnessMessageAssertions(Result<TModel> subject, string? expression) : base(subject, expression) { }

    /// <summary>Asserts that the <see cref="Result"/> contains no messages.</summary>
    [Assertion]
    public ResultValidnessAssertions<TModel> WithoutMessages()
    {
        ExecuteWithoutMessages();
        return new ResultValidnessAssertions<TModel>(Subject, Expression);
    }

    /// <summary>Asserts that the <see cref="Result"/> contains the specified message.</summary>
    [Assertion]
    public ResultValidnessAssertions<TModel> WithMessage(IValidationMessage message)
    {
        ExecuteWithMessage(message);
        return new ResultValidnessAssertions<TModel>(Subject, Expression);
    }

    /// <summary>Asserts that the <see cref="Result"/> contains the specified messages.</summary>
    [Assertion]
    public ResultValidnessAssertions<TModel> WithMessages(params IValidationMessage[] messages)
    {
        ExecuteWithMessages(messages);
        return new ResultValidnessAssertions<TModel>(Subject, Expression);
    }
}
