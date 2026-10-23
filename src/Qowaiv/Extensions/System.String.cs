using Qowaiv;
using System.Buffers;

namespace System;

/// <summary>Extensions on <see cref="string" />.</summary>
public static class QowaivString
{
    extension(string str)
    {
        /// <summary>Converts the specified string to an uppercase string.</summary>
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
        public string ToUpper(IFormatProvider provider)
        {
            Guard.NotNull(provider);
            var textInfo = provider.GetFormat<TextInfo>() ?? CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToUpper(str);
        }

        /// <summary>Converts the specified string to title case (except for words that are entirely
        /// in uppercase, which are considered to be acronyms).
        /// </summary>
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
        public string ToTitleCase(IFormatProvider provider)
        {
            Guard.NotNull(provider);
            var textInfo = provider.GetFormat<TextInfo>() ?? CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(str);
        }

        [Pure]
        internal bool Matches(Regex pattern) => pattern.IsMatch(str);
    }

    extension(string? str)
    {
        /// <summary>Returns the provided default if <see cref="string.IsNullOrEmpty(string)" />,
        /// otherwise the string value.
        /// </summary>
        [Pure]
        internal string WithDefault(string @default = "")
            => str is { Length: > 0 } ? str : @default;

        [Pure]
        internal string Unify()
        {
            if (str is { Length: > 0 })
            {
                ReadOnlySpan<char> span = str;
                var size = span.BufferSize();

                if (size <= 256)
                {
                    Span<char> buffer = stackalloc char[span.BufferSize()];
                    var length = span.Unify(buffer);
                    return buffer[..length].ToString();
                }
                else
                {
                    char[] buffer = ArrayPool<char>.Shared.Rent(size);
                    var length = span.Unify(buffer);
                    var unified = buffer.AsSpan()[..length].ToString();
                    ArrayPool<char>.Shared.Return(buffer);
                    return unified;
                }
            }
            else return str ?? string.Empty;
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsEmpty() => str is not { Length: > 0 };

        [Pure]
        internal bool IsUnknown(IFormatProvider? formatProvider)
            => Unknown.IsUnknown(str, formatProvider as CultureInfo);

        [Pure]
        internal CharSpan CharSpan() => new(str ?? string.Empty);
    }
}
