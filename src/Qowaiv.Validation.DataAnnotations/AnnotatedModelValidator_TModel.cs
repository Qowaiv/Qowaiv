using Qowaiv.Validation.Abstractions;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>A validator to validate models based on their data annotations.</summary>
/// <typeparam name="TModel">
/// The type of the annotated model to validate.
/// </typeparam>
/// <param name="serviceProvider">
/// The object that implements the System.IServiceProvider interface. This parameter is optional.
/// </param>
/// <param name="items">
/// A dictionary of key/value pairs to make available to the service consumers. This parameter is optional.
/// </param>
[Mutable]
public class AnnotatedModelValidator<TModel>(IServiceProvider? serviceProvider, IDictionary<object, object?>? items) : IValidator<TModel>
    where TModel : notnull
{
    /// <summary>Initializes a new instance of the <see cref="AnnotatedModelValidator{TModel}"/> class.</summary>
    public AnnotatedModelValidator() : this(null, null) { }

    /// <summary>Initializes a new instance of the <see cref="AnnotatedModelValidator{TModel}"/> class.</summary>
    /// <param name="serviceProvider">
    /// The object that implements the System.IServiceProvider interface. This parameter is optional.
    /// </param>
    public AnnotatedModelValidator(IServiceProvider? serviceProvider)
        : this(serviceProvider, null) { }

    /// <summary>Initializes a new instance of the <see cref="AnnotatedModelValidator{TModel}"/> class.</summary>
    /// <param name="items">
    /// A dictionary of key/value pairs to make available to the service consumers. This parameter is optional.
    /// </param>
    public AnnotatedModelValidator(IDictionary<object, object?>? items)
        : this(null, items) { }

    /// <summary>Gets the <see cref="IServiceProvider"/>.</summary>
    protected IServiceProvider? ServiceProvider { get; } = serviceProvider;

    /// <summary>Gets the <see cref="IServiceProvider"/>.</summary>
    protected IDictionary<object, object?> Items { get; } = items ?? new Dictionary<object, object?>(0);

    /// <summary>Validates the model.</summary>
    /// <param name="model">
    /// The model to validate.
    /// </param>
    /// <returns>
    /// A result including the model and the <see cref="ValidationResult"/>s.
    /// </returns>
    [Pure]
    public Result<TModel> Validate(TModel model)
        => AnnotatedModelValidator.Validate(model, ServiceProvider, Items);
}
