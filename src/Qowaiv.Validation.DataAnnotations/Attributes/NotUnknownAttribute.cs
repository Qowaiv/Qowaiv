namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Specifies that the unknown value is not allowed.</summary>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
[Validates(typeof(object))]
public sealed class NotUnknownAttribute() : ValidationAttribute(() => QowaivValidationMessages.NotUnknown)
{
    /// <inheritdoc />
    [Pure]
    public override bool IsValid(object? value)
        => value is null
        || !Equals(SvoValues.Unknown(value.GetType()), value);
}
