namespace Qowaiv.Validation.TestTools;

/// <summary>Contains a number of methods to assert the state of a <see cref="Result"/>.</summary>
public class ResultValidnessAssertionsBase<TSubject>(TSubject subject, string? expression)
    where TSubject : Result
{
    /// <summary>Gets the object which value is being asserted.</summary>
    protected TSubject Subject { get; } = Guard.NotNull(subject);

    /// <summary>Gets the expression asserted on.</summary>
    internal string Expression { get; } = expression ?? "Result";

    /// <summary>Gets the <see cref="Result.Messages"/>.</summary>
    protected IEnumerable<IValidationMessage> Messages => Subject?.Messages ?? [];

    internal void ExecuteWithoutMessages() => Assertion
        .For(Expression)
        .WithMessage(WithoutMessages(Messages))
        .Ensure(!Messages.Any());

    internal void ExecuteWithMessage(IValidationMessage message) => Assertion
        .For(Expression)
        .WithMessage(WithMessage(message, [.. Messages]))
        .Ensure(Messages.Count() == 1 && Comparer().Equals(message, Messages.Single()));

    internal void ExecuteWithMessages(IValidationMessage[] messages)
    {
        var missing = messages.Except(Messages, Comparer()).ToArray();
        var extra = Messages.Except(messages, Comparer()).ToArray();
        var error = Messages.Any()
                ? WithMessages(missing, extra)
                : "Expected messages, but found none.";

        Assertion
            .For(Expression)
            .WithMessage(error)
            .Ensure(missing.Length == 0 && extra.Length == 0);
    }

    [Pure]
    internal static string WithoutMessages(IEnumerable<IValidationMessage> messages)
        => new StringBuilder()
            .Append("Expected no messages, but found:")
            .AppendMessages(messages)
            .ToString();

    [Pure]
    private static string WithMessage(IValidationMessage expected, IValidationMessage[] actuals)
    {
        if (actuals.Length == 0) return "Expected a message, but found none.";
        else if (actuals.Length == 1)
        {
            return new StringBuilder()
                .AppendLine("Expected:")
                .AppendMessage(expected)
                .AppendLine()
                .AppendLine("Actual:")
                .AppendMessage(actuals[0])
                .ToString();
        }
        else
        {
            var all = new[] { expected };
            return WithMessages([.. all.Except(actuals, Comparer())], [.. actuals.Except(all, Comparer())]);
        }
    }

    [Pure]
    private static IEqualityComparer<IValidationMessage> Comparer() => ValidationMessageCompare.ByInterface;

    [Pure]
    private static string WithMessages(IValidationMessage[] missing, IValidationMessage[] extra)
    {
        var sb = new StringBuilder();
        if (missing.Length > 0)
        {
            sb.Append($"Missing message{(missing.Length == 1 ? string.Empty : "s")}:")
                .AppendMessages(missing);
        }
        if (extra.Length > 0)
        {
            sb.Append($"Extra message{(extra.Length == 1 ? string.Empty : "s")}:")
                .AppendMessages(extra);
        }
        return sb.ToString();
    }
}
