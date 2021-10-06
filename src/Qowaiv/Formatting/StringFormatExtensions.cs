using System;
using System.Globalization;

namespace Qowaiv.Formatting
{
    /// <summary>Formatting extensions on <see cref="string"/> and <see cref="IFormatProvider"/>.</summary>
    public static class StringFormatExtensions
    {
        /// <summary>Converts the specified string to an uppercase string.</summary> 
        /// <param name="str">
        /// The string to convert to uppercase.
        /// </param>
        /// <param name="provider"></param>
        /// <returns>
        /// The uppercase equivalent of the current string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// str is null or the provider is null.
        /// </exception>
        public static string ToUpper(this string str, IFormatProvider provider)
        {
            Guard.NotNull(provider, nameof(provider));
            var textInfo = provider.GetFormat<TextInfo>() ?? CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToUpper(str);
        }

        /// <summary>Converts the specified string to title case (except for words that are entirely
        /// in uppercase, which are considered to be acronyms).
        /// 
        /// </summary>
        /// <param name="str">The string to convert to title case.</param>
        /// <param name="provider"></param>
        /// <returns>
        /// The specified string converted to title case.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// str is null or the provider is null.
        /// </exception>
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
        public static TFormat GetFormat<TFormat>(this IFormatProvider provider)
        {
            Guard.NotNull(provider, nameof(provider));
            return (TFormat)provider.GetFormat(typeof(TFormat));
        }

        /// <summary>Returns the provided default if <see cref="string.IsNullOrEmpty(string)"/>,
        /// otherwise the string value.
        /// </summary>
        internal static string WithDefault(this string str, string @default)
            => string.IsNullOrEmpty(str) ? @default : str;
    }
}
