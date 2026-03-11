public partial struct @TSvo
#if NET8_0_OR_GREATER
    : IParsable<@TSvo>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="@TSvo"/>.</summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <returns>
    /// The parsed @FullName.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static @TSvo Parse(string? s)
        => TryParse(s, null, out var svo)
            ? svo
            : throw Unparsable.ForValue<@TSvo>(s, @FormatExceptionMessage);

    /// <summary>Converts the <see cref="string"/> to <see cref="@TSvo"/>.</summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed @FullName.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static @TSvo Parse(string? s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
            ? svo
            : throw Unparsable.ForValue<@TSvo>(s, @FormatExceptionMessage);

    /// <summary>Converts the <see cref="string"/> to <see cref="@TSvo"/>.</summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <returns>
    /// The @FullName if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static @TSvo? TryParse(string? s)
        => TryParse(s, null, out var val)
            ? val
            : default(@TSvo?);

    /// <summary>Converts the <see cref="string"/> to <see cref="@TSvo"/>.</summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The @FullName if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static @TSvo? TryParse(string? s, IFormatProvider? provider)
        => TryParse(s, provider, out var val)
            ? val
            : default(@TSvo?);

    /// <summary>Converts the <see cref="string"/> to <see cref="@TSvo"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out @TSvo result) => TryParse(s, null, out result);
}
