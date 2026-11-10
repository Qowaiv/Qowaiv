namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Base <see cref="ValidationAttribute"/> for allowing or forbidding a set of values.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
[CLSCompliant(false)]
public abstract class SetOfValuesAttribute(params string[] values) : ValidationAttribute(() => QowaivValidationMessages.AllowedValuesAttribute_ValidationError)
{
    /// <summary>The result to return when the value of <see cref="IsValid(object)"/>
    /// equals one of the values of the <see cref="SetOfValuesAttribute"/>.
    /// </summary>
    protected abstract bool OnEqual { get; }

    /// <summary>Gets the values.</summary>
    public string[] Values { get; } = Guard.NotNull(values);

    /// <summary>Returns true if the value is forbidden, otherwise false.</summary>
    [Pure]
    public sealed override bool IsValid(object? value)
    {
        if (value == null) return true;
        else
        {
            var converter = TypeDescriptor.GetConverter(value.GetType());
            return OnEqual == Values.Any(val => value.Equals(converter.ConvertFromInvariantString(val)));
        }
    }
}
