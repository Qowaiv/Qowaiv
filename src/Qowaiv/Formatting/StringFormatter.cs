using System;
using System.Collections.Generic;
using System.Text;

namespace Qowaiv.Formatting
{
    /// <summary>A string formatter class.</summary>
    public class StringFormatter
    {
        /// <summary>Apply a fformat string instruction on an object.</summary>
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
        /// <param name="escape">
        /// The escape character, the default escape character '\'.
        /// </param>
        /// <returns></returns>
        public static string Apply<T>(T obj, string format, IFormatProvider formatProvider, Dictionary<char, Func<T, IFormatProvider, string>> tokens, char escape = '\\')
        {
            #region Guarding

            if (obj == null) { throw new ArgumentNullException("obj"); }
            if (String.IsNullOrEmpty(format)) { throw new ArgumentNullException("format"); }
            if (formatProvider == null) { throw new ArgumentNullException("formatProvider"); }
            if (tokens == null) { throw new ArgumentNullException("tokens"); }

            #endregion

            var sb = new StringBuilder();
            var isEscape = false;
            Func<T, IFormatProvider, string> action;

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
                else if (tokens.TryGetValue(ch, out action))
                {
                    sb.Append(action.Invoke(obj, formatProvider));
                }
                // Append char.
                else
                {
                    sb.Append(ch);
                }
            }
            if (isEscape)
            {
                throw new FormatException(QowaivMessages.FormatExceptionInvalidFormat);
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
        public static bool TryApplyCustomFormatter(string format, object obj, IFormatProvider formatProvider, out string formatted)
        {
            formatted = null;
            if (formatProvider != null)
            {
                var customFormatter = formatProvider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
                if (customFormatter != null)
                {
                    formatted = customFormatter.Format(format, obj, formatProvider);
                    return true;
                }
            }
            return false;
        }
    }
}
