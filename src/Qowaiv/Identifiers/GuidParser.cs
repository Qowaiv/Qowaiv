using System.Runtime.InteropServices;

namespace Qowaiv.Identifiers;

/// <summary><see cref="Guid" /> parser for Base64 and Base32 strings.</summary>
internal static class GuidParser
{
    private const byte None = 255;

    /// <summary>Tries to parse a <see cref="Guid" /> from a Base64 string.</summary>
    [Pure]
    public static bool TryBase64(string s, out Guid guid)
    {
        guid = Guid.Empty;

        // Wrong length.
        if (s.Length < 22 || s.Length > 24) return false;

        // Invalid suffix.
        for (var i = 22; i < s.Length; i++) { if (s[i] != '=') return false; }

        var success = true;

#if NET8_0_OR_GREATER
        GuidLayout layout = default;
        Span<byte> bytes = MemoryMarshal.AsBytes(new Span<GuidLayout>(ref layout));
#else
        var bytes = new byte[16];
#endif
        success &= Write0(s[00], bytes, 00);
        success &= Write1(s[01], bytes, 00);
        success &= Write2(s[02], bytes, 01);
        success &= Write3(s[03], bytes, 02);
        success &= Write0(s[04], bytes, 03);
        success &= Write1(s[05], bytes, 03);
        success &= Write2(s[06], bytes, 04);
        success &= Write3(s[07], bytes, 05);
        success &= Write0(s[08], bytes, 06);
        success &= Write1(s[09], bytes, 06);
        success &= Write2(s[10], bytes, 07);
        success &= Write3(s[11], bytes, 08);
        success &= Write0(s[12], bytes, 09);
        success &= Write1(s[13], bytes, 09);
        success &= Write2(s[14], bytes, 10);
        success &= Write3(s[15], bytes, 11);
        success &= Write0(s[16], bytes, 12);
        success &= Write1(s[17], bytes, 12);
        success &= Write2(s[18], bytes, 13);
        success &= Write3(s[19], bytes, 14);
        success &= Write0(s[20], bytes, 15);
        success &= WriteLast(s[21], bytes);

#if NET8_0_OR_GREATER
        guid = layout.ToGuid();
#else
        guid = new Guid(bytes);
#endif
        return success;

        // 01234567 01234567 01234567
        // ..012345 ........ .......
        static bool Write0(char ch, Span<byte> bytes, int index)
        {
            var val = Base64Lookup[(byte)ch];
            bytes[index] |= (byte)(val << 2);
            return val != None;
        }

        // 01234567 01234567 01234567
        // 45****** ....0123 ........
        static bool Write1(char ch, Span<byte> bytes, int index)
        {
            var val = Base64Lookup[(byte)ch];
            bytes[index] |= (byte)(val >> 4);
            bytes[index + 1] |= (byte)(val << 4);
            return val != None;
        }

        // 01234567 01234567 01234567
        // ******** 2345**** ......01
        static bool Write2(char ch, Span<byte> bytes, int index)
        {
            var val = Base64Lookup[(byte)ch];
            bytes[index] |= (byte)(val >> 2);
            bytes[index + 1] |= (byte)(val << 6);
            return val != None;
        }

        // 01234567 01234567 01234567
        // ******** ******** 012345**
        static bool Write3(char ch, Span<byte> bytes, int index)
        {
            var val = Base64Lookup[(byte)ch];
            bytes[index] |= val;
            return val != None;
        }

        static bool WriteLast(char ch, Span<byte> bytes)
        {
            var val = Base64Lookup[(byte)ch];
            bytes[15] |= (byte)(val >> 4);
            return val != None;
        }
    }

    /// <summary>Tries to parse a <see cref="Guid" /> from a Base64 string.</summary>
    [Pure]
    public static bool TryBase32(string s, out Guid guid)
    {
        guid = Guid.Empty;
        if (s.Length == 26 && Base32.TryGetBytes(s, out var base32))
        {
            guid = new Guid(base32);
            return true;
        }
        else return false;
    }

    private static readonly byte[] Base64Lookup =
    [
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, 0x3e, None, 0x3e, None, 0x3f,
        0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, None, None, None, None, None, None,
        None, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e,
        0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, None, None, None, None, 0x3f,
        None, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
        0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 0x30, 0x31, 0x32, 0x33, None, None, None, None, None,

        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
        None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None,
    ];
}
