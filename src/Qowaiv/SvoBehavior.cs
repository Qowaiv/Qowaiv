#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
// Justification: SvoBehavior instances can be created via reflection, this one should then be excluded.

namespace Qowaiv;

/// <summary>Handles the behavior of a custom Single Value Object.</summary>
public abstract class SvoBehavior : TypeConverter, IComparer<string>
{
    internal static readonly string unknown = $"{char.MaxValue}";

    /// <summary>Defines the minimum length the string representation of the Single Value Object may be.</summary>
    /// <remarks>
    /// Default is 0.
    /// </remarks>
    public virtual int MinLength => 0;

    /// <summary>Defines the maximum length the string representation of the Single Value Object may be.</summary>
    /// <remarks>
    /// Default is <see cref="int.MaxValue"/>.
    /// </remarks>
    public virtual int MaxLength => int.MaxValue;

    /// <summary>If specified, it defines the maximum length the string representation of the Single Value Object may be.</summary>
    public virtual Regex? Pattern => null;

    /// <summary>Returns true when the input string is considered the unknown state of the Single Value Object.</summary>
    /// <param name="str">
    /// The input string to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The optional format provider.
    /// </param>
    [Pure]
    public virtual bool IsUnknown(string str, IFormatProvider? formatProvider) => Unknown.IsUnknown(str, formatProvider as CultureInfo);

    /// <summary>Validates if the input <see cref="string"/> is valid given its format provider.</summary>
    /// <param name="str">
    /// The input string to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The optional format provider.
    /// </param>
    /// <param name="validated">
    /// The validated (and potentially transformed) result.
    /// </param>
    [Pure]
    public virtual bool IsValid(string str, IFormatProvider? formatProvider, out string validated)
    {
        validated = str;
        return true;
    }

    /// <inheritdoc />
    [Pure]
    public virtual int Compare(string? x, string? y) => Comparer<string>.Default.Compare(x!, y!);

    /// <summary>Gets the length of string representation of Single Value Object.</summary>
    [Pure]
    public virtual int Length(string str) => str.Length;

    /// <summary>Normalizes the string input before being validated.</summary>
    /// <param name="str">
    /// The input string to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The optional format provider.
    /// </param>
    [Pure]
    public virtual string NormalizeInput(string? str, IFormatProvider? formatProvider)
        => str?.Trim() ?? string.Empty;

    /// <summary>Returns a formatted <see cref="string" /> that represents the 
    /// unknown state of the Single Value Object.
    /// </summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public virtual string FormatUnknown(string? format, IFormatProvider? formatProvider) => "?";

    /// <summary>Returns a formatted <see cref="string" /> that represents the Single Value Object.</summary>
    /// <param name="str">
    /// The string representing the Single Value Object.
    /// </param>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// Not called for empty and unknown.
    /// </remarks>
    [Pure]
    public virtual string Format(string str, string? format, IFormatProvider? formatProvider) => str;

    /// <summary>Serializes the Single Value Object to a JSON string.</summary>
    /// <param name="str">
    /// The string representing the Single Value Object.
    /// </param>
    [Pure]
    public virtual string? ToJson(string? str)
    {
        if (str is null) return null;
        else if (str == unknown) return FormatUnknown(default, CultureInfo.InvariantCulture);
        else return str;
    }

    /// <summary>Serializes the Single Value Object to an XML string.</summary>
    /// <param name="str">
    /// The string representing the Single Value Object.
    /// </param>
    [Pure]
    public virtual string? ToXml(string? str)
    {
        if (str is null) return null;
        else if (str == unknown) return FormatUnknown(default, CultureInfo.InvariantCulture);
        else return str;
    }

    /// <summary>Creates a <see cref="FormatException"/> using the <see cref="InvalidFormatMessage(string?, IFormatProvider?)"/>.</summary>
    [Pure]
    public virtual FormatException InvalidFormat(string? str, IFormatProvider? formatProvider)
        => new(InvalidFormatMessage(str, formatProvider));

    /// <summary>Composes an invalid format message.</summary>
    [Pure]
    public virtual string InvalidFormatMessage(string? str, IFormatProvider? formatProvider)
        => GetType().Name.StartsWith("For")
        ? $"Not a valid {GetType().Name.Substring(3)}"
        : $"Not a valid {GetType().Name}";

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(string);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => TryParse(value?.ToString(), culture, out var parsed)
        ? parsed
        : throw InvalidFormat(value?.ToString(), culture);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => ToString(value?.ToString(), default, culture);

    /// <summary>Converts the <see cref="string"/> to a 
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="str">
    /// A string containing the Single Value Object to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <param name="validated">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    internal bool TryParse(string? str, IFormatProvider? formatProvider, out string? validated)
    {
        var normalized = NormalizeInput(str, formatProvider);
        if (string.IsNullOrWhiteSpace(normalized))
        {
            validated = null;
            return true;
        }
        else if (IsUnknown(normalized, formatProvider))
        {
            validated = unknown;
            return true;
        }
        else if (WithinSize(normalized)
            && IsMatch(normalized)
            && IsValid(normalized, formatProvider, out validated))
        {
            return true;
        }
        else
        {
            validated = null;
            return false;
        }

        bool WithinSize(string str)
            => str.Length >= MinLength 
            && str.Length <= MaxLength;

        bool IsMatch(string str)
            => Pattern is null
            || Pattern.IsMatch(str);
    }

    /// <summary>Returns a formatted <see cref="string" /> that represents the Single Value Object.</summary>
    /// <param name="str">
    /// The string representing the Single Value Object.
    /// </param>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    internal string ToString(string? str, string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, str!, formatProvider, out string formatted))
        {
            return formatted;
        }
        else if (str is null)
        {
            return string.Empty;
        }
        else if (str == unknown)
        {
            return FormatUnknown(format, formatProvider);
        }
        else return Format(str, format, formatProvider);
    }
}
