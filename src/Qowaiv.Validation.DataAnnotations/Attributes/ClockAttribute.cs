namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Base attribute for validating members compared to the clock.</summary>
[CLSCompliant(false)]
public abstract class ClockAttribute(Func<string> errorMessageAccessor) : ValidationAttribute(errorMessageAccessor)
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsValid(object? value) => value switch
    {
        null => true,
        Year y when y.IsEmptyOrUnknown() => true,

        DateTime d /*.*/ => ValidateCompare(d.CompareTo(Clock.UtcNow())),
        DateTimeOffset d => ValidateCompare(d.CompareTo(Clock.Now())),

        Date d /*.....*/ => ValidateCompare(d.CompareTo(Clock.Today())),
#if NET8_0_OR_GREATER
        DateOnly d /*.*/ => ValidateCompare(d.CompareTo(Clock.Today())),
#endif
        Year y /*.....*/ => ValidateCompare(y.CompareTo(Clock.Today().Year.CE())),
        _ => false,
    };

    /// <summary>Validates the outcome of the compare.</summary>
    [Pure]
    protected abstract bool ValidateCompare(int compare);
}
