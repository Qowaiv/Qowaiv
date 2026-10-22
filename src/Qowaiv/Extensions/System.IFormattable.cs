namespace System;

/// <summary>Extensions on <see cref="IFormattable" />.</summary>
public static class QowaivFormattableExtensions
{
    extension(IFormattable? formattable)
    {
        /// <summary>Formats the object using the formatting arguments.</summary>
        /// <param name="arguments">
        /// The formatting arguments.
        /// </param>
        /// <returns>
        /// A formatted string representing the object.
        /// </returns>
        [Pure]
        [return: NotNullIfNotNull(nameof(formattable))]
        public string? ToString(FormattingArguments arguments)
            => formattable is { }
            ? arguments.ToString(formattable)
            : null;

        /// <summary>Formats the object using the formatting arguments collection.</summary>
        /// <param name="argumentsCollection">
        /// The formatting arguments collection.
        /// </param>
        /// <returns>
        /// A formatted string representing the object.
        /// </returns>
        [Pure]
        [return: NotNullIfNotNull(nameof(formattable))]
        public string? ToString(FormattingArgumentsCollection? argumentsCollection)
            => formattable is { }
            ? (argumentsCollection ?? []).ToString(formattable)
            : null;
    }
}
