#if !NotUtf8 // exec

#if NET8_0_OR_GREATER
public partial struct @TSvo
    : IUtf8SpanParsable<@TSvo>
{
    /// <summary>Converts the <see cref="string"/> to <see cref="@TSvo"/>.</summary>
    /// <param name="utf8Text">
    /// An UTF-8 string containing the @FullName to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed @FullName.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="utf8Text"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static @TSvo Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
        => TryParse(utf8Text, provider)
        ?? throw Unparsable.ForValue<@TSvo>(utf8Text.ToString(), @FormatExceptionMessage);

    /// <summary>Converts the <see cref="string"/> to <see cref="@TSvo"/>.</summary>
    /// <param name="utf8Text">
    /// An UTF-8 string containing the @FullName to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The @FullName if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static @TSvo? TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
        => TryParse(utf8Text, provider, out var val) ? val : default(@TSvo?);
}
#endif
#endif // exec
