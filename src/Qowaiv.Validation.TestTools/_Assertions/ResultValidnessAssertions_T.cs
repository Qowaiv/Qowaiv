namespace Qowaiv.Validation.TestTools;

/// <summary>Contains a number of methods to assert the value of a valid <see cref="Result{TModel}"/>.</summary>
[Inheritable]
public class ResultValidnessAssertions<TModel> : ResultValidnessAssertionsBase<Result<TModel>>
{
    internal ResultValidnessAssertions(Result<TModel> subject, string? expression) : base(subject, expression) { }

    /// <summary>Expose the value, so that the chain of assertions can be continued.</summary>
    [Obsolete("Use And.Value instead")]
    public TModel Value => Subject.Value;

    /// <summary>Expose the value, so that the chain of assertions can be continued.</summary>
    public AndConstraint And => new() { Value = Subject.Value };

    /// <summary>Constraint builder.</summary>
    public sealed class AndConstraint 
    {
        /// <summary>Expose the value, so that the chain of assertions can be continued.</summary>
        public required TModel Value { get; init; }
    }
}
