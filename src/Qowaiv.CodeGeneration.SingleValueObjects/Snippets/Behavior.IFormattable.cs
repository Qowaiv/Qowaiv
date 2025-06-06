[global::System.Diagnostics.DebuggerDisplay("{DebuggerDisplay}")]
partial struct @Svo : global::System.IFormattable
{
    /// <summary>Returns a <see cref="string" /> that represents the @FullName.</summary>
    [global::System.Diagnostics.Contracts.Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string" /> that represents the @FullName.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [global::System.Diagnostics.Contracts.Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string" /> that represents the @FullName.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [global::System.Diagnostics.Contracts.Pure]
    public string ToString(global::System.IFormatProvider? provider) => ToString(format: null, provider);

    /// <summary>Returns a formatted <see cref="string" /> that represents the @FullName.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [global::System.Diagnostics.Contracts.Pure]
    public string ToString(string? format, global::System.IFormatProvider? formatProvider)
    {
        if (global::Qowaiv.Formatting.StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else if (HasValue)
        {
            return behavior.ToString(m_Value, format, formatProvider);
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>Returns a <see cref="string" /> that represents the @FullName for DEBUG purposes.</summary>
    [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private string DebuggerDisplay => HasValue ? ToString() : "{empty}";
}
