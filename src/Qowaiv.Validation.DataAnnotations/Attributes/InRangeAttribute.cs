namespace Qowaiv.Validation.DataAnnotations.Attributes;

/// <summary>Specifies the allowed range of values.</summary>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
[Validates(GenericArgument = true)]
public sealed class InRangeAttribute<TValue> : ValidationAttribute where TValue : IComparable
{
    /// <summary>Initializes a new instance of the <see cref="InRangeAttribute{TValue}"/> class.</summary>
    /// <param name="minimum">The minimum value, inclusive.</param>
    /// <param name="maximum">The maximum value, inclusive.</param>
    public InRangeAttribute(object minimum, object maximum) : base(() => QowaivValidationMessages.InRange)
    {
        var converter = TypeDescriptor.GetConverter(typeof(TValue));

        Minimum = minimum is TValue min ? min : converter.ConvertFromCultureInvariant<TValue>(minimum);
        Maximum = maximum is TValue max ? max : converter.ConvertFromCultureInvariant<TValue>(maximum);
    }

    /// <summary>Gets the minimum value for the range.</summary>
    public TValue Minimum { get; }

    /// <summary>Gets the maximum value for the range.</summary>
    public TValue Maximum { get; }

    /// <inheritdoc />
    [Pure]
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        TValue val => val.CompareTo(Minimum) >= 0 && val.CompareTo(Maximum) <= 0,
        _ => false,
    };

    /// <inheritdoc />
    [Pure]
    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum, Maximum);
}
