#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
// Justification: SvoBehavior instances can be created via reflection, this one should then be excluded.

namespace Qowaiv;

/// <summary>Handles the behavior of a custom Single Value Object.</summary>
public abstract partial class SvoBehavior : IComparer<string>
{
    internal static readonly string unknown = $"{char.MaxValue}";

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
