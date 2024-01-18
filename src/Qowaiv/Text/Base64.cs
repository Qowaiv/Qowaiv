using System.Runtime.InteropServices;

namespace Qowaiv.Text;

/// <summary> is a group of similar binary-to-text encoding schemes that
/// represent binary data in an ASCII string format by translating it into
/// a radix-64 representation. The term Base64 originates from MIME.
/// (RFC 1341, since made obsolete by RFC 2045).
/// </summary>
public static class Base64
{
    /// <summary>Represents a byte array as a <see cref="string"/>.</summary>
    [Pure]
    public static string ToString(byte[]? bytes)
        => bytes == null || bytes.Length == 0
        ? string.Empty
        : Convert.ToBase64String(bytes);

    /// <summary>Tries to get the corresponding bytes of the Base64 string.</summary>
    /// <param name="s">
    /// The string to convert.
    /// </param>
    /// <param name="bytes">
    /// The bytes represented by the Base64 string.
    /// </param>
    /// <returns>
    /// True if the string is a Base64 string, otherwise false.
    /// </returns>
    /// <remarks>
    /// If the conversion fails,  bytes is an empty byte array, not null.
    /// </remarks>
    public static bool TryGetBytes(string? s, out byte[] bytes)
    {
        if (string.IsNullOrEmpty(s))
        {
            bytes = [];
            return true;
        }
        try
        {
            bytes = Convert.FromBase64String(s);
            return true;
        }
        catch
        {
            bytes = [];
            return false;
        }
    }

    [Pure]
    internal static string ToString(Guid guid)
    {
        Span<char> chars = stackalloc char[22];

#if NET8_0_OR_GREATER
        ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes(new Span<Guid>(ref guid));
#else
        ReadOnlySpan<byte> bytes = guid.ToByteArray();
#endif
        chars[00] = Read0(bytes, 00);
        chars[01] = Read1(bytes, 00);
        chars[02] = Read2(bytes, 01);
        chars[03] = Read3(bytes, 02);
        chars[04] = Read0(bytes, 03);
        chars[05] = Read1(bytes, 03);
        chars[06] = Read2(bytes, 04);
        chars[07] = Read3(bytes, 05);
        chars[08] = Read0(bytes, 06);
        chars[09] = Read1(bytes, 06);
        chars[10] = Read2(bytes, 07);
        chars[11] = Read3(bytes, 08);
        chars[12] = Read0(bytes, 09);
        chars[13] = Read1(bytes, 09);
        chars[14] = Read2(bytes, 10);
        chars[15] = Read3(bytes, 11);
        chars[16] = Read0(bytes, 12);
        chars[17] = Read1(bytes, 12);
        chars[18] = Read2(bytes, 13);
        chars[19] = Read3(bytes, 14);
        chars[20] = Read0(bytes, 15);
        chars[21] = Base64Chars[(bytes[15] << 4) & 0b11_0000];

#if NET6_0_OR_GREATER
        return new(chars);
#else
        return new(chars.ToArray());
#endif
        // 01234567 01234567 0123456
        // ..012345 ........ .......
        static char Read0(ReadOnlySpan<byte> bytes, int index)
            => Base64Chars[bytes[index] >> 2];

        // 01234567 01234567 01234567
        // 45****** ....0123 ........
        static char Read1(ReadOnlySpan<byte> bytes, int index)
        {
            var hi = (bytes[index + 0] << 4) & 0b11_0000;
            var lo = (bytes[index + 1] >> 4) & 0b00_1111;
            return Base64Chars[lo | hi];
        }

        // 01234567 01234567 01234567
        // ******** 2345**** ......01
        static char Read2(ReadOnlySpan<byte> bytes, int index)
        {
            var hi = (bytes[index + 0] << 2) & 0b11_1100;
            var lo = (bytes[index + 1] >> 6) & 0b00_0011;
            return Base64Chars[lo | hi];
        }

        // 01234567 01234567 01234567
        // ******** ******** 012345**
        static char Read3(ReadOnlySpan<byte> bytes, int index)
            => Base64Chars[bytes[index] & 0b11_1111];
    }

    private const string Base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
}
