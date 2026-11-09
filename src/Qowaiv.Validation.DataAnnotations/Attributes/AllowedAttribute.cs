namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validates if the decorated item has a value that is specified in the allowed values.</summary>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
[CLSCompliant(false)]
[Validates(GenericArgument = true)]
public sealed class AllowedAttribute<TValue>(params object[] values) : SetOfAttribute<TValue>(values)
{
    /// <summary>Return true the value of <see cref="SetOfAttribute{TValue}.IsValid(object)"/>
    /// equals one of the values of the <see cref="SetOfAttribute{TValue}" />.
    /// </summary>
    protected override bool OnEqual => true;
}
