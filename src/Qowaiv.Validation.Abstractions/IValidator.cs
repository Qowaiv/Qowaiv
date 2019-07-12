namespace Qowaiv.Validation.Abstractions
{
    /// <summary>A validator for <typeparamref name="TModel"/>.</summary>
    /// <typeparam name="TModel">
    /// The type of the model supported by the validator.
    /// </typeparam>
    public interface IValidator<TModel>
    {
        /// <summary>Validates the model.</summary>
        /// <param name="model">
        /// The model to validate.
        /// </param>
        /// <returns>
        /// The <see cref="Result{T}"/> containing the validated model or the error messages.
        /// </returns>
        Result<TModel> Validate(TModel model);
    }
}
