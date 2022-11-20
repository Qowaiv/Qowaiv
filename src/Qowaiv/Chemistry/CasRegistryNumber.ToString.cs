#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Chemistry;

/// <summary>
/// A CAS Registry Number, is a unique numerical identifier assigned by the
/// Chemical Abstracts Service (CAS), US to every chemical substance described
/// in the open scientific literature. It includes all substances described
/// from 1957 through the present, plus some substances from as far back as the
/// early 1800's.
/// </summary>
#if NET6_0_OR_GREATER
public readonly partial struct CasRegistryNumber : ISpanFormattable
#else
public readonly partial struct CasRegistryNumber : IFormattable
#endif
{
    /// <summary>Returns a formatted <see cref="string" /> that represents the CAS Registry Number.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// The formats:
    /// f: as formatted.
    /// 
    /// other (not empty) formats are applied on the number (long).
    /// </remarks>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else if (IsEmpty()) return string.Empty;
        else if (IsUnknown()) return "?";
        else return format.WithDefault("f") == "f"
            ? m_Value.ToString(@"#00\-00\-0", formatProvider)
            : m_Value.ToString(format, formatProvider);
    }
#if NET6_0_OR_GREATER
    /// <inheritdoc />
    [Pure]
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, provider, out string formatted))
        {
            return destination.TryWrite(formatted, out charsWritten);
        }
        else if (IsEmpty()) return destination.TryWrite(string.Empty, out charsWritten);
        else if (IsUnknown()) return destination.TryWrite('?', out charsWritten);
        else return format.WithDefault("f") == "f"
            ? m_Value.TryFormat(destination, out charsWritten, @"#00\-00\-0", provider)
            : m_Value.TryFormat(destination, out charsWritten, format, provider);
    }
#endif
    /// <summary>Gets an XML string representation of the CAS Registry Number.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the CAS Registry Number to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

  
}
