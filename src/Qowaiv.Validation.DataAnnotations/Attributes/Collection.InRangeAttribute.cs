namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the size of the collection.</summary>
public static partial class Collection
{
    /// <summary>Specifies the allowed range of the length of property, field or parameter.</summary>
    [AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
    [Validates(typeof(object))]
    public sealed class InRangeAttribute(long minimum, long maximum)
        : ValidationAttribute(() => QowaivValidationMessages.Collection_InRange_ValidationError)
    {
        /// <summary>The minimum length.</summary>
        public long Minimum { get; } = minimum;

        /// <summary>The maximum length.</summary>
        public long Maximum { get; } = maximum;

        /// <inheritdoc />
        [Pure]
        public override bool IsValid(object? value)
            => GetCount<InRangeAttribute>(value) is not long count
            || count == 0 // should be dealt with by mandatory/required.
            || (count >= Minimum && count <= Maximum);

        /// <inheritdoc />
        [Pure]
        public override string FormatErrorMessage(string name)
                => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum, Maximum);
    }
}
