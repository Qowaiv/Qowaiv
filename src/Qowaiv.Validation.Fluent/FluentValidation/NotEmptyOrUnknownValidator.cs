namespace FluentValidation;

/// <summary>
/// To ensure that NotEmpty is validated equally for
/// <see cref="UnknownValidation.NotEmptyOrUnknown{TModel, TProperty}(IRuleBuilder{TModel, TProperty})"/>
/// and
/// <see cref="UnknownValidation.NotUnknown{TModel, TProperty}(IRuleBuilder{TModel, TProperty})"/>
/// the <see cref="NotEmptyValidator{TModel, TProperty}"/> is overridden.
/// </summary>
internal sealed class NotEmptyOrUnknownValidator<TModel, TProperty> : NotEmptyValidator<TModel, TProperty>
{
    /// <inheritdoc />
    [Pure]
    public override bool IsValid(ValidationContext<TModel> context, TProperty value)
        => base.IsValid(context, value)
        && !Equals(SvoValues.Unknown(typeof(TProperty)), value);
}
