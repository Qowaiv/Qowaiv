namespace Qowaiv.Validation.TestTools;

/// <summary>Contains a number of methods to assert the (in)validness of a <see cref="Result{TModel}"/>.</summary>
public sealed class ResultAssertions<TModel> : ResultAssertionsBase<Result<TModel>>
{
    /// <summary>Initializes a new instance of the <see cref="ResultAssertions{TModel}"/> class.</summary>
    internal ResultAssertions(Result<TModel> subject, string? expression) : base(subject, expression) { }

    /// <summary>Asserts thats <see cref="Result"/> is invalid.</summary>
    [Assertion]
    public ResultValidnessMessageAssertions<TModel> BeValid(string because = "", params object[] becauseArgs)
    {
        ExecuteBeValid(because, becauseArgs);
        return new ResultValidnessMessageAssertions<TModel>(Subject, Expression);
    }
}
