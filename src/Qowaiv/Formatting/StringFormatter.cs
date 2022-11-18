using Microsoft.VisualBasic;
using System.ComponentModel;

namespace Qowaiv.Formatting;

/// <summary>A string formatter class.</summary>
public static class StringFormatter
{
    /// <summary>Apply a format string instruction on an object.</summary>
    /// <typeparam name="T">
    /// The type of the object to format.
    /// </typeparam>
    /// <param name="obj">
    /// The object to format.
    /// </param>
    /// <param name="format">
    /// The format string.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <param name="tokens">
    /// An dictionary with character based tokens.
    /// </param>
    /// <returns>
    /// An formatted string.
    /// </returns>
    /// <remarks>
    /// Uses the escape character '\'.
    /// </remarks>
    [Pure]
    public static string Apply<T>(T obj, string format, IFormatProvider? formatProvider, Dictionary<char, Func<T, IFormatProvider, string>> tokens)
        => Apply(obj, format, formatProvider, tokens, '\\');

    /// <summary>Apply a format string instruction on an object.</summary>
    /// <typeparam name="T">
    /// The type of the object to format.
    /// </typeparam>
    /// <param name="obj">
    /// The object to format.
    /// </param>
    /// <param name="format">
    /// The format string.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider, if null <see cref="CultureInfo.CurrentCulture"/> is used.
    /// </param>
    /// <param name="tokens">
    /// An dictionary with character based tokens.
    /// </param>
    /// <param name="escape">
    /// The escape character.
    /// </param>
    /// <returns>
    /// An formatted string.
    /// </returns>
    [Pure]
    public static string Apply<T>(T obj, string format, IFormatProvider? formatProvider, Dictionary<char, Func<T, IFormatProvider, string>> tokens, char escape)
    {
        Guard.NotNull((object?)obj, nameof(obj));
        Guard.NotNullOrEmpty(format, nameof(format));
        Guard.NotNull(tokens, nameof(tokens));

        var sb = new StringBuilder();
        var isEscape = false;

        foreach (var ch in format)
        {
            // If escape, write the char and unescape.
            if (isEscape)
            {
                // if is not for a token, or the escape characters.
                if (!tokens.ContainsKey(ch) && ch != escape)
                {
                    sb.Append(escape);
                }
                sb.Append(ch);
                isEscape = false;
            }
            // Escape char, enable escape.
            else if (ch == escape)
            {
                isEscape = true;
            }
            // If a token match, apply.
            else if (tokens.TryGetValue(ch, out var action))
            {
                sb.Append(action.Invoke(obj, formatProvider ?? CultureInfo.CurrentCulture));
            }
            // Append char.
            else
            {
                sb.Append(ch);
            }
        }
        if (isEscape)
        {
            throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
        }
        return sb.ToString();
    }

    /// <summary>Tries to apply the custom formatter to format the object.</summary>
    /// <param name="format">
    /// The format to apply
    /// </param>
    /// <param name="obj">
    /// The object to format.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <param name="formatted">
    /// The formatted result.
    /// </param>
    /// <returns>
    /// True, if the format provider supports custom formatting, otherwise false.
    /// </returns>
    public static bool TryApplyCustomFormatter(string? format, object obj, IFormatProvider? formatProvider, out string formatted)
    {
        var customFormatter = formatProvider?.GetFormat<ICustomFormatter>();
        if (customFormatter is null)
        {
            formatted = string.Empty;
            return false;
        }
        else
        {
            formatted = customFormatter.Format(format, obj, formatProvider);
            return true;
        }
    }

#if NET6_0_OR_GREATER
    /// <summary>Tries to apply the custom formatter to format the object.</summary>
    /// <param name="format">
    /// The format to apply
    /// </param>
    /// <param name="obj">
    /// The object to format.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <param name="formatted">
    /// The formatted result.
    /// </param>
    /// <returns>
    /// True, if the format provider supports custom formatting, otherwise false.
    /// </returns>
    public static bool TryApplyCustomFormatter(ReadOnlySpan<char> format, object obj, IFormatProvider? formatProvider, out string formatted)
        => TryApplyCustomFormatter(format.ToString(), obj, formatProvider, out formatted);
#endif

    /// <summary>Replaces diacritic characters by non diacritic ones.</summary>
    /// <param name="str">
    /// The string to remove the diacritics from.
    /// </param>
    [Pure]
    public static string ToNonDiacritic(string str)
        => ToNonDiacritic(str, string.Empty);

    /// <summary>Replaces diacritic characters by non diacritic ones.</summary>
    /// <param name="str">
    /// The string to remove the diacritics from.
    /// </param>
    /// <param name="ignore">
    /// Diacritics at the ignore, will not be changed.
    /// </param>
    [Pure]
    public static string ToNonDiacritic(string str, string ignore)
     => string.IsNullOrEmpty(str)
        ? str
        : str.Buffer().ToNonDiacritic(ignore ?? string.Empty);
}
