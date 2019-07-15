using Qowaiv.Validation.Abstractions;

namespace Qowaiv.Validation.Fluent
{
    public class FluentModelValidator<TModel> : FluentValidation.AbstractValidator<TModel>, IValidator<TModel>
    {
        /// <inheritdoc />
        Result<TModel> IValidator<TModel>.Validate(TModel model)
        {
            var context = new FluentValidation.ValidationContext<TModel>(model);
            var result = Validate(context);
            return Result.For(model, ValidationMessage.For(result.Errors));
        }
    }
}
