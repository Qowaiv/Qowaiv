#if NET10_0_OR_GREATER

using System.Text.Json;

namespace Qowaiv.Json;

internal static class JsonWriter
{
    private const byte Quote = (byte)'"';

    extension(Utf8JsonWriter writer)
    {
        /// <summary>Writes UTF16 chars assuming all to be ASCII that not require JSON escaping.</summary>
        /// <remarks>
        /// All validation is skipped. If the prequisits are not met, the JSON will be corrupt.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAsciiString(string? value)
        {
            if (value is { })
            {
                ReadOnlySpan<char> ascii = value;
                Span<byte> utf8 = stackalloc byte[ascii.Length + 2];
                utf8[0] = Quote;
                Ascii.FromUtf16(ascii, utf8[1..], out _);
                utf8[^1] = Quote;
                writer!.WriteRawValue(utf8, skipInputValidation: true);
            }
            else
            {
                writer!.WriteNullValue();
            }
        }
    }
}
#endif
