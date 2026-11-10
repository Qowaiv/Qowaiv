namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the size of the collection.</summary>
public static partial class Collection
{
    /// <summary>Specifies the maximum the length of property, field or parameter.</summary>
    [AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
    [Validates(typeof(object))]
    public sealed class AtMostAttribute(long maximum)
        : ValidationAttribute(() => QowaivValidationMessages.Collection_AtMost_ValidationError)
    {
        /// <summary>The maximum length.</summary>
        public long Maximum { get; } = maximum;

        /// <inheritdoc />
        [Pure]
        public override bool IsValid(object? value)
            => GetCount<AtMostAttribute>(value) is not long count
            || count <= Maximum;

        /// <inheritdoc />
        [Pure]
        public override string FormatErrorMessage(string name)
            => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Maximum);
    }
}
