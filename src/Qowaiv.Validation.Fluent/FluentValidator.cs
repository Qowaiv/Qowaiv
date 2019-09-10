using Qowaiv.Validation.Abstractions;

namespace Qowaiv.Validation.Fluent
{
    /// <summary>Implements <see cref="IValidator{TModel}"/> using <see cref="FluentValidation.IValidator{T}"/>.</summary>
    /// <typeparam name="TModel"></typeparam>
    public class FluentValidator<TModel> : IValidator<TModel>
    {
        private readonly FluentValidation.IValidator<TModel> _validator;

        /// <summary>Creates a new instance of a <see cref="FluentValidator{TModel}"/>.</summary>
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
