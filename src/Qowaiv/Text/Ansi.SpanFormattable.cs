#if !NETSTANDARD2_0

namespace Qowaiv.Text;

public readonly partial struct Ansi : ISpanFormattable
{
    /// <inheritdoc />
    [Pure]
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    /// <summary>Tries to format the value of the current instance into the provided span of characters.</summary>
    /// <param name="destination">
    /// When this method returns, this instance's value formatted as a span of characters.
    /// </param>
    /// <param name="charsWritten">
    /// When this method returns, the number of characters that were written in <paramref name="destination"/>.
    /// </param>
    /// <param name="format">
    /// A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="buffer"/>.
    /// </param>
    /// <param name="provider">
    /// An optional object that supplies culture-specific formatting information for <paramref name="destination"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = 0;
        if (destination.Length < 4) return false;

        // Reset case.
        if (Codes is not { Length: > 0 })
        {
            "\x1b[0m".CopyTo(destination);
            charsWritten = 4;
            return true;
        }

        // Prefix.
        "\x1b[".CopyTo(destination);
        var pos = 2;

        // First code does not fit.
        if (!Codes[0].TryFormat(destination[pos..], out int bytesWritten, format, provider)) return false;

        pos += bytesWritten;

        // Next codes.
        for (var i = 1; i < Codes.Length; i++)
        {
            // Ensure space for ';' and at least one digit, and m.
            if (destination.Length - pos < 3) return false;
            destination[pos++] = ';';

            if (!Codes[i].TryFormat(destination[pos..], out bytesWritten, format, provider)) return false;

            pos += bytesWritten;
        }

        // Final char.
        if (pos >= destination.Length) return false;
        destination[pos++] = 'm';

        charsWritten = pos;
        return true;
    }
}
#endif
