namespace Qowaiv.Validation.Abstractions
{
    /// <summary>Static validator helper class.</summary>
    public static class Validator
    {
        /// <summary>Gets an empty <see cref="IValidator{TModel}"/>.</summary>
        public static IValidator<TModel> Empty<TModel>() => new EmptyValidator<TModel>();

        /// <summary>Implementation of an empty validator.</summary>
        private sealed class EmptyValidator<TModel> : IValidator<TModel>
        {
            /// <inheritdoc />
            public Result<TModel> Validate(TModel model) => Result.For(model);
        }
    }
}
