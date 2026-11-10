namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the length of a value.</summary>
public static partial class Length
{
    /// <summary>Specifies the maximum the length of property, field or parameter.</summary>
    [AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
    [Validates(typeof(object))]
    public sealed class AtMostAttribute(long maximum)
        : ValidationAttribute(() => QowaivValidationMessages.Length_AtMost_ValidationError)
    {
        /// <summary>The maximum length.</summary>
        public long Maximum { get; } = maximum;

        /// <inheritdoc />
        [Pure]
        public override bool IsValid(object? value)
            => GetLength<AtMostAttribute>(value) is not long length
            || length <= Maximum;

        /// <inheritdoc />
        [Pure]
        public override string FormatErrorMessage(string name)
            => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Maximum);
    }
}
