namespace Qowaiv.Validation.Fluent;

/// <summary>Base class for an <see cref="Abstractions.IValidator{TModel}"/> using FluentValidation.NET.</summary>
/// <typeparam name="TModel">
/// The model to validate for.
/// </typeparam>
public class ModelValidator<TModel> : AbstractValidator<TModel>, Abstractions.IValidator<TModel>
    where TModel : notnull
{
    /// <summary>Initializes a new instance of the <see cref="ModelValidator{TModel}"/> class.</summary>
    protected ModelValidator() { }

    /// <inheritdoc />
    [Pure]
    Result<TModel> Abstractions.IValidator<TModel>.Validate(TModel model)
    {
        var result = Validate(new ValidationContext<TModel>(model));
        return Result.For(model, ValidationMessage.For(result.Errors));
    }
}
