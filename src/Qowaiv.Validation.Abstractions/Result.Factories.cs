namespace Qowaiv.Validation.Abstractions;

public partial class Result
{
    /// <summary>Represents an OK <see cref="Result"/>.</summary>
    public static readonly Result OK = new([]);

    /// <summary>Creates a <see cref="Task"/> for the value.</summary>
    [Pure]
    public static Task<Result<TModel>> Async<TModel>(TModel value, params IEnumerable<IValidationMessage> messages)
        where TModel : notnull
            => For(value, messages).Async();

    /// <summary>Creates a valid null <see cref="Result{TModel}"/>.</summary>
    [Pure]
    public static Result<TModel> Null<TModel>(params IEnumerable<IValidationMessage> messages)
        => new(ValidationMessages.New(messages));

    /// <summary>Creates a <see cref="Result{TModel}"/> for the value.</summary>
    [Pure]
    public static Result<TModel> For<TModel>(TModel value, params IEnumerable<IValidationMessage> messages)
        where TModel : notnull
        => new Result<TModel>(value, ValidationMessages.New(messages)).NotNull();

    /// <summary>Creates a result with messages.</summary>
    [Pure]
    public static Result WithMessages(params IEnumerable<IValidationMessage> messages)
        => new(ValidationMessages.New(messages));

    /// <summary>Creates a result with messages.</summary>
    [Pure]
    public static Result<TModel> WithMessages<TModel>(params IEnumerable<IValidationMessage> messages)
        => new Result<TModel>(ValidationMessages.New(messages)) is { IsValid: false } model
        ? model
        : throw NoValue.For<TModel>();
}
