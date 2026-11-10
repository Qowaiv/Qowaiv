namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the size of the collection.</summary>
public static partial class Collection
{
    /// <summary>Specifies the minimum the length of property, field or parameter.</summary>
    [AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
    [Validates(typeof(object))]
    public sealed class AtLeastAttribute(long minimum)
        : ValidationAttribute(() => QowaivValidationMessages.Collection_AtLeast_ValidationError)
    {
        /// <summary>The minimum length.</summary>
        public long Minimum { get; } = minimum;

        /// <inheritdoc />
        [Pure]
        public override bool IsValid(object? value)
            => GetCount<AtLeastAttribute>(value) is not long count
            || count == 0 // should be dealt with by mandatory/required.
            || count >= Minimum;

        /// <inheritdoc />
        [Pure]
        public override string FormatErrorMessage(string name)
                => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum);
    }
}
