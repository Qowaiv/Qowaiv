namespace Qowaiv.Validation.TestTools;

/// <summary>Implements <see cref="IValidator{TModel}"/> using <see cref="FluentValidation.IValidator{T}"/>.</summary>
internal sealed class WrapperValidator<TModel>(FluentValidation.IValidator<TModel> validator) : IValidator<TModel>
    where TModel : notnull
{
    private readonly FluentValidation.IValidator<TModel> _validator = validator;

    /// <inheritdoc />
    [Pure]
    public Result<TModel> Validate(TModel model)
    {
        var context = new FluentValidation.ValidationContext<TModel>(model);
        var result = _validator.Validate(context);
        return Result.For(model, Fluent.ValidationMessage.For(result.Errors));
    }
}
