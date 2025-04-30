partial struct @Svo
#if NET8_0_OR_GREATER
    : global::System.IParsable<@Svo>
#endif
{
    /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.</summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <returns>
    /// The parsed @FullName.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s" /> is not in the correct format.
    /// </exception>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.</summary>
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
    /// <paramref name="s" /> is not in the correct format.
    /// </exception>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo Parse(string? s, global::System.IFormatProvider? provider)
        => TryParse(s, provider)
        ?? throw behavior.InvalidFormat(s, provider);

    /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.</summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <returns>
    /// The @FullName if the string was converted successfully, otherwise default.
    /// </returns>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.</summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The @FullName if the string was converted successfully, otherwise default.
    /// </returns>
    [global::System.Diagnostics.Contracts.Pure]
    public static @Svo? TryParse(string? s, global::System.IFormatProvider? formatProvider)
        => TryParse(s, formatProvider, out var val)
        ? val
        : default(@Svo?);

    /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.
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
    [global::System.Diagnostics.Contracts.Pure]
    public static bool TryParse(string? s, out @Svo result) => TryParse(s, null, out result);

    /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [global::System.Diagnostics.Contracts.Pure]
    public static bool TryParse(string? s, global::System.IFormatProvider? provider, out @Svo result)
    {
        if (behavior.TryTransform(s, provider, out var transformed))
        {
            result = new @Svo(transformed);
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }
}
