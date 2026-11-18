using Qowaiv.Validation.Microsoft.Extensions;

namespace Qowaiv.Validation.Microsoft;

/// <summary>Implements <see cref="IValidator{TModel}"/> using Microsoft.Extensions.Validation metadata.</summary>
/// <typeparam name="TModel">
/// The type of the model to validate.
/// </typeparam>
/// <param name="options">
/// The validation options that provide the resolvers for validatable type information.
/// </param>
/// <param name="provider">
/// The service provider to use while validating. This parameter is optional.
/// </param>
[CLSCompliant(false)]
public sealed class ModelValidator<TModel>(ValidationOptions options, IServiceProvider? provider = null) : IValidator<TModel>
    where TModel : notnull
{
    private readonly ValidationOptions Options = Guard.NotNull(options);
    private readonly IServiceProvider? Provider = provider;

    /// <summary>Validates the model.</summary>
    /// <param name="model">
    /// The model to validate.
    /// </param>
    /// <returns>
    /// A result containing the model and the validation messages reported by the
    /// resolved <see cref="ValidationOptions"/>.
    /// </returns>
    [Pure]
    public Result<TModel> Validate(TModel model)
    {
        if (Options.TryGetValidatableTypeInfo(typeof(TModel), out var info))
        {
            var context = Context();
            info.Validate(model, context);
            return Result.For(model, context.ValidationMessages());
        }
        else return model;
    }

    [Pure]
    private ValidateContext Context() => new()
    {
        ValidationOptions = Options,
        ServiceProvider = Provider,
    };
}
