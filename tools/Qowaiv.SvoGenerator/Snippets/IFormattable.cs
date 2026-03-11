#if !NotIFormattable // exec
public partial struct @TSvo : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the @FullName.</summary>
    [Pure]
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the @FullName.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the @FullName.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}
#endif // exec
