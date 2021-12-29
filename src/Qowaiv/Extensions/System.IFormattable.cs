namespace System;

/// <summary>Extensions on <see cref="IFormattable"/>.</summary>
public static class QowaivFormattableExtensions
{
    /// <summary>Formats the object using the formatting arguments.</summary>
    /// <param name="formattable">
    /// The object to format.
    /// </param>
    /// <param name="arguments">
    /// The formatting arguments
    /// </param>
    /// <returns>
    /// A formatted string representing the object.
    /// </returns>
    [Pure]
    public static string? ToString(this IFormattable? formattable, FormattingArguments arguments)
        => formattable is { }
        ? arguments.ToString(formattable)
        : null;

    /// <summary>Formats the object using the formatting arguments collection.</summary>
    /// <param name="formattable">
    /// The object to format.
    /// </param>
    /// <param name="argumentsCollection">
    /// The formatting arguments collection.
    /// </param>
    /// <returns>
    /// A formatted string representing the object.
    /// </returns>
    [Pure]
    public static string? ToString(this IFormattable? formattable, FormattingArgumentsCollection? argumentsCollection)
      => formattable is { }
        ? (argumentsCollection ?? new FormattingArgumentsCollection()).ToString(formattable)
        : null;
}
