using Qowaiv.IO;
using System.IO;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the size of a value.</summary>
public static partial class Size
{
    /// <summary>Specifies the maximum the length of property, field or parameter.</summary>
    [AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
    [Validates(typeof(Stream))]
    [Validates(typeof(ICollection<byte>))]
    [Validates(typeof(IReadOnlyCollection<byte>))]
    [Validates("System.BinaryData")]
    public sealed class AtMostAttribute(long maximum)
        : ValidationAttribute(() => QowaivValidationMessages.Size_AtMost_ValidationError)
    {
        /// <summary>Initializes a new instance of the <see cref="AtMostAttribute"/> class.</summary>
        public AtMostAttribute(string minimum)
            : this((long)StreamSize.Parse(minimum, CultureInfo.InvariantCulture)) { }

        /// <summary>The maximum length.</summary>
        public StreamSize Maximum { get; } = maximum;

        /// <inheritdoc />
        [Pure]
        public override bool IsValid(object? value)
            => GetSize<AtMostAttribute>(value) is not StreamSize size
            || size <= Maximum;

        /// <inheritdoc />
        [Pure]
        public override string FormatErrorMessage(string name)
            => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Maximum);
    }
}
