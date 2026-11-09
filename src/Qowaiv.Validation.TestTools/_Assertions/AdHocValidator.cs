namespace Qowaiv.Validation.TestTools;

/// <summary>Allows a model to be validated with a validator of choice.</summary>
[DebuggerNonUserCode]
public static class AdHocValidator
{
    extension<TModel>(TModel model) where TModel : class
    {
        /// <summary>Validates the model is valid for the specified validator.</summary>
        [Pure]
        public Result<TModel> ValidateWith(IValidator<TModel> validator)
        {
            Guard.NotNull(model, nameof(model));
            Guard.NotNull(validator, nameof(validator));
            return validator.Validate(model);
        }
    }
}
