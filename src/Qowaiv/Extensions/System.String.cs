using Qowaiv;

namespace System;

/// <summary>Extensions on <see cref="string"/>.</summary>
public static class QowaivSystemExtensions
{
    /// <summary>Converts the specified string to an uppercase string.</summary>
    /// <param name="str">
    /// The string to convert to uppercase.
    /// </param>
    /// <param name="provider">
    /// The format provider to apply.
    /// </param>
    /// <returns>
    /// The uppercase equivalent of the current string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// str is null or the provider is null.
    /// </exception>
    [Pure]
    public static string ToUpper(this string str, IFormatProvider provider)
    {
        Guard.NotNull(provider, nameof(provider));
        var textInfo = provider.GetFormat<TextInfo>() ?? CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToUpper(str);
    }

    /// <summary>Converts the specified string to title case (except for words that are entirely
    /// in uppercase, which are considered to be acronyms).
    /// </summary>
    /// <param name="str">The string to convert to title case.</param>
    /// <param name="provider">
    /// The format provider to apply.
    /// </param>
    /// <returns>
    /// The specified string converted to title case.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// str is null or the provider is null.
    /// </exception>
    [Pure]
    public static string ToTitleCase(this string str, IFormatProvider provider)
    {
        Guard.NotNull(provider, nameof(provider));
        var textInfo = provider.GetFormat<TextInfo>() ?? CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(str);
    }

    /// <summary>Returns an object that provides formatting services for the specified type.</summary>
    /// <typeparam name="TFormat">
    /// An object that specifies the type of format object to return.
    /// </typeparam>
    /// <returns>
    /// An instance of the object specified by formatType, if the System.IFormatProvider
    /// implementation can supply that type of object; otherwise, null.
    /// </returns>
    [Pure]
    public static TFormat? GetFormat<TFormat>(this IFormatProvider provider)
    {
        Guard.NotNull(provider, nameof(provider));
        return (TFormat?)provider.GetFormat(typeof(TFormat));
    }

    /// <summary>Returns the provided default if <see cref="string.IsNullOrEmpty(string)"/>,
    /// otherwise the string value.
    /// </summary>
    [Pure]
    internal static string WithDefault(this string? str, string @default = "")
        => str is { Length: > 0 } ? str : @default;

    [Pure]
    internal static string Unify(this string? str) => str.Buffer().Unify();

    [Pure]
    internal static bool IsEmpty(this string? str) => string.IsNullOrEmpty(str);

    [Pure]
    internal static bool IsUnknown(this string? str, IFormatProvider? formatProvider)
        => Unknown.IsUnknown(str, formatProvider as CultureInfo);

    [Pure]
    internal static bool Matches(this string str, Regex pattern) => pattern.IsMatch(str);

    [Pure]
    internal static CharSpan CharSpan(this string? str) => str is null ? new(string.Empty) : new(str);
}
