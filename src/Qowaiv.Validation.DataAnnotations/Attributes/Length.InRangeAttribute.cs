namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the length of a value.</summary>
public static partial class Length
{
    /// <summary>Specifies the allowed range of the length of property, field or parameter.</summary>
    [AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
    [Validates(typeof(object))]
    public sealed class InRangeAttribute(long minimum, long maximum)
        : ValidationAttribute(() => QowaivValidationMessages.Length_InRange_ValidationError)
    {
        /// <summary>The minimum length.</summary>
        public long Minimum { get; } = minimum;

        /// <summary>The maximum length.</summary>
        public long Maximum { get; } = maximum;

        /// <inheritdoc />
        [Pure]
        public override bool IsValid(object? value)
            => GetLength<InRangeAttribute>(value) is not long length
            || length == 0 // should be dealt with by mandatory/required.
            || (length >= Minimum && length <= Maximum);

        /// <inheritdoc />
        [Pure]
        public override string FormatErrorMessage(string name)
            => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum, Maximum);
    }
}
