namespace Qowaiv.Financial;


public partial struct Amount
#if NET6_0_OR_GREATER
    : ISpanFormattable
#endif
{

#if NET6_0_OR_GREATER

    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var span = new Span<char>(new char[128]);
        ((ISpanFormattable)this).TryFormat(span, out var size, format, formatProvider);
        return span[..size].ToString();
    }

    /// <inheritdoc />
    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format.ToString(), this, provider, out string formatted))
        {
            formatted.CopyTo(destination);
            charsWritten = formatted.Length;
            return true;
        }
        else return m_Value.TryFormat(destination, out charsWritten, format, Money.GetNumberFormatInfo(provider));
    }
#else

    /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : m_Value.ToString(format, Money.GetNumberFormatInfo(formatProvider));

#endif
}
