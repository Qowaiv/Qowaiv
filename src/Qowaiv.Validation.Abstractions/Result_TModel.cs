namespace Qowaiv.Validation.Abstractions;

/// <summary>Represents a result of a validation, executed command, etcetera.</summary>
/// <typeparam name="TModel">
/// The type of <see cref="Value"/>.
/// </typeparam>
public sealed partial class Result<TModel> : Result
{
    /// <summary>Initializes a new instance of the <see cref="Result{T}"/> class.</summary>
    internal Result(ValidationMessages messages) : base(messages) { }

    /// <summary>Initializes a new instance of the <see cref="Result{T}"/> class.</summary>
    /// <param name="value">
    /// The value related to the result.
    /// </param>
    /// <param name="messages">
    /// The messages related to the result.
    /// </param>
    internal Result(TModel? value, ValidationMessages messages) : base(messages)
        => _value = IsValid ? value : default;

    /// <summary>Gets the value related to result.</summary>
    /// <remarks>
    /// Although the value can be null (<see cref="Result.Null{T}(IEnumerable{IValidationMessage})"/>
    /// in normal flows however, the value is not null when valid.
    /// </remarks>
    [Pure]
    public TModel Value => IsValid
        ? _value!
        : throw InvalidModel.For<TModel>(Errors);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly TModel? _value;

    /// <summary>Implicitly casts a model to the <see cref="Result"/>.</summary>
    [Pure]
    public static implicit operator Result<TModel>(TModel model) => NotNull(model, []);

    /// <inheritdoc />
    [Pure]
    protected internal override InvalidModel InvalidOperation()
        => InvalidModel.For<TModel>(Errors);

    /// <summary>Casts the result of <typeparamref name="TModel"/> to a result of <typeparamref name="TOut"/>.</summary>
    /// <typeparam name="TOut">
    /// The new type of the value.
    /// </typeparam>
    /// <exception cref="InvalidCastException">
    /// if <typeparamref name="TOut"/> is not a subtype of <typeparamref name="TModel"/>.
    /// </exception>
    [Pure]
    public Result<TOut> Cast<TOut>()
    {
        return new(_value is not null && IsValid ? Cast() : default, Messages);

        TOut Cast()
            => _value is TOut cast
            ? cast
            : throw new InvalidCastException($"Unable to cast object of type 'Result<{typeof(TModel)}>' to type 'Result<{typeof(TOut)}>'.");
    }

    /// <summary>Wraps the <see cref="Result{TModel}"/> in an <see cref="Task"/>.</summary>
    [Pure]
    [Obsolete("Use Async() instead.")]
    public Task<Result<TModel>> AsTask() => Async();

    /// <summary>Wraps the <see cref="Result{TModel}"/> in an <see cref="Task"/>.</summary>
    [Pure]
    public new Task<Result<TModel>> Async() => Task.FromResult(this);

    /// <summary>Explicitly casts the <see cref="Result"/> to the type of the related model.</summary>
    public static explicit operator TModel?(Result<TModel>? result) => result == null ? default : result.Value;

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="result">
    /// The result to act on.
    /// </param>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <returns>
    /// A result with the merged messages.
    /// </returns>
    public static Result<TModel> operator |(Result<TModel> result, Func<TModel, TModel> action)
        => Guard.NotNull(result).Act(action);

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="result">
    /// The result to act on.
    /// </param>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <returns>
    /// A result with the merged messages.
    /// </returns>
    public static Result<TModel> operator |(Result<TModel> result, Func<TModel, Result<TModel>> action)
        => Guard.NotNull(result).Act(action);

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="result">
    /// The result to act on.
    /// </param>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <returns>
    /// A result with the merged messages.
    /// </returns>
    public static Task<Result<TModel>> operator |(Result<TModel> result, Func<TModel, Task<Result<TModel>>> action)
        => Guard.NotNull(result).ActAsync(action);

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="result">
    /// The result to act on.
    /// </param>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <returns>
    /// A result with the merged messages.
    /// </returns>
    public static Result<TModel> operator |(Result<TModel> result, Func<TModel, Result> action)
        => Guard.NotNull(result).Act(action);
}
