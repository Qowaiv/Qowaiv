﻿#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
// Justification: SvoBehavior instances can be created via reflection, this one should then be excluded.

namespace Qowaiv;

/// <summary>Handles the behavior of a custom Single Value Object.</summary>
public partial class SvoBehavior : TypeConverter, IComparer<string>
{
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
    public virtual int Compare(string? x, string? y) => Comparer<string>.Default.Compare(x, y);

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

    [Pure]
    public virtual FormatException InvalidFormat(string? str)
        => new(InvalidFormatMessage(str));

    [Pure]
    public virtual string InvalidFormatMessage(string? str)
        => GetType().Name.StartsWith("For")
        ? $"Not a valid {GetType().Name.Substring(3)}"
        : $"Not a valid {GetType().Name}";
}
