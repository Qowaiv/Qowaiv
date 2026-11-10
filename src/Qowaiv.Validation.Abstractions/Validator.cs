namespace Qowaiv.Validation.Abstractions;

/// <summary>Static validator helper class.</summary>
public static class Validator
{
    /// <summary>Creates a composite <see cref="IValidator{TModel}"/>.</summary>
    [Pure]
    public static IValidator<TModel> Composite<TModel>(params IEnumerable<IValidator<TModel>> validators) where TModel : notnull
        => new CompositeValidator<TModel>([.. Guard.NotNull(validators)]);

    /// <summary>Gets an empty <see cref="IValidator{TModel}"/>.</summary>
    [Pure]
    public static IValidator<TModel> Empty<TModel>()
        where TModel : notnull
        => new EmptyValidator<TModel>();

    private sealed class CompositeValidator<TModel>(IValidator<TModel>[] validators) : IValidator<TModel>
        where TModel : notnull
    {
        private readonly IValidator<TModel>[] Validators = Guard.HasAny(validators);

        /// <inheritdoc />
        [Pure]
        public Result<TModel> Validate(TModel model)
        {
            return Result.For(model, Validators.SelectMany(ValidateModel));

            IEnumerable<IValidationMessage> ValidateModel(IValidator<TModel> validator)
                => validator.Validate(model).Messages;
        }
    }

    /// <summary>Implementation of an empty validator.</summary>
    private sealed class EmptyValidator<TModel> : IValidator<TModel>
        where TModel : notnull
    {
        /// <inheritdoc />
        [Pure]
        public Result<TModel> Validate(TModel model) => Result.For(model);
    }
}
