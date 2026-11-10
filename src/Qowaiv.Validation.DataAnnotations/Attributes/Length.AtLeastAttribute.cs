namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the length of a value.</summary>
public static partial class Length
{
    /// <summary>Specifies the minimum the length of property, field or parameter.</summary>
    [AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
    [Validates(typeof(object))]
    public sealed class AtLeastAttribute(long minimum)
        : ValidationAttribute(() => QowaivValidationMessages.Length_AtLeast_ValidationError)
    {
        /// <summary>The minimum length.</summary>
        public long Minimum { get; } = minimum;

        /// <inheritdoc />
        [Pure]
        public override bool IsValid(object? value)
            => GetLength<AtLeastAttribute>(value) is not long length
            || length == 0 // should be dealt with by mandatory/required.
            || length >= Minimum;

        /// <inheritdoc />
        [Pure]
        public override string FormatErrorMessage(string name)
            => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum);
    }
}
