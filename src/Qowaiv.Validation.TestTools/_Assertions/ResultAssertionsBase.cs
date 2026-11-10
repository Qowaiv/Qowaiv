namespace Qowaiv.Validation.TestTools;

/// <summary>Contains a number of methods to assert the (in)validness of a <see cref="Result"/>.</summary>
[StackTraceHidden]
public abstract class ResultAssertionsBase<TSubject>(TSubject? subject, string? expression)
    where TSubject : Result
{
    /// <summary>Gets the object which validness is being asserted.</summary>
    public TSubject Subject { get; } = Guard.NotNull(subject);

    /// <summary>Gets the expression asserted oon.</summary>
    internal string Expression { get; } = expression ?? "Result";

    /// <summary>Asserts thats <typeparamref name="TSubject"/> is invalid.</summary>
    [Assertion]
    public ResultInvalidnessAssertions BeInvalid(string because = "", params object[] becauseArgs)
    {
        var error = Subject is { Messages.Count : > 0 }
            ? Text("Actual {context} is not invalid{reason}:").AppendMessages(Subject.Messages).ToString()
            : "Actual {context} is not invalid{reason}.";

        Assertion.For(Expression)
            .BecauseOf(because, becauseArgs)
            .WithMessage(error)
            .Ensure(!Subject.IsValid);

        return new ResultInvalidnessAssertions(Subject, Expression);
    }

    internal void ExecuteBeValid(string because, object[] becauseArgs)
    {
        var error = Subject is { Messages: { Count: > 0 } messages }
            ? Text("Actual {context} is not valid{reason}:").AppendMessages(messages).ToString()
            : "Actual {context} is not invalid{reason}.";

        Assertion
            .For(Expression)
            .BecauseOf(because, becauseArgs)
            .WithMessage(error)
            .Ensure(Subject.IsValid);
    }

    [Pure]
    private static StringBuilder Text(string text) => new(text);
}
