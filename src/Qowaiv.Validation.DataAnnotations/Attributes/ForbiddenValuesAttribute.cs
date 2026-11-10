namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validates if the decorated item has a value that is specified in the forbidden values.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
[CLSCompliant(false)]
[Validates(typeof(object))]
public sealed class ForbiddenValuesAttribute(params string[] values) : SetOfValuesAttribute(values)
{
    /// <summary>Return false if the value of <see cref="SetOfValuesAttribute.IsValid(object)"/>
    /// equals one of the values of the <see cref="ForbiddenValuesAttribute"/>.
    /// </summary>
    protected override bool OnEqual => false;
}
