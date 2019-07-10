using Qowaiv.Validation.Abstractions;

namespace Qowaiv.Validation.Fluent
{
    public class FluentValidator<TModel> : IValidator<TModel>
    {
        private readonly FluentValidation.IValidator<TModel> _validator;

        public FluentValidator(FluentValidation.IValidator<TModel> validator)
        {
            _validator = Guard.NotNull(validator, nameof(validator));
        }
        
        /// <inheritdoc />
        public Result<TModel> Validate(TModel model)
        {
            var context = new FluentValidation.ValidationContext<TModel>(model);
            var result = _validator.Validate(context);
            return Result.For(model, ValidationMessage.For(result.Errors));
        }
    }
}
