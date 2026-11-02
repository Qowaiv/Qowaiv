#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members: Naming safes the day here.
#pragma warning disable IDE0230 // Use UTF-8 string literal: this is not about UTF-8 literals.

namespace Qowaiv.Text;

public readonly partial struct Ansi
{
    /// <summary>Set foreground color to black (30).</summary>
    public static readonly Ansi Black = Single(30);

    /// <summary>Set foreground color to red (31).</summary>
    public static readonly Ansi Red = Single(31);

    /// <summary>Set foreground color to green (32).</summary>
    public static readonly Ansi Green = Single(32);

    /// <summary>Set foreground color to yellow (33).</summary>
    public static readonly Ansi Yellow = Single(33);

    /// <summary>Set foreground color to blue (34).</summary>
    public static readonly Ansi Blue = Single(34);

    /// <summary>Set foreground color to purple (35).</summary>
    public static readonly Ansi Purple = Single(35);

    /// <summary>Set foreground color to cyan (36).</summary>
    public static readonly Ansi Cyan = Single(36);

    /// <summary>Set foreground color to white (37).</summary>
    public static readonly Ansi White = Single(37);

    /// <summary>Set foreground color to default (39).</summary>
    public static readonly Ansi Default = Single(39);

    /// <summary>Sets foreground color to a gray level (38).</summary>
    /// <param name="g">
    /// Gray level, 0 is black, 255 is white.
    /// </param>
    /// <remarks>
    /// If the 256-color palette contains exactly this gray, the (shorter) palette
    /// index is used, otherwise RGB.
    /// </remarks>
    [Pure]
    public static Ansi Gray(byte g) => GrayIndex(g) is var i and >= 0 ? Foreground((byte)i) : Foreground(g, g, g);

    /// <summary>Sets foreground color using the 256-color palette (38).</summary>
    /// <param name="index">
    /// Palette index. Indices 0-15 are the simple colors, 16-231 a 6x6x6 color cube,
    /// and 232-255 a gray scale.
    /// </param>
    [Pure]
    public static Ansi Pal(byte index) => Foreground(index);

    /// <summary>Sets forground color using RGB (38).</summary>
    /// <param name="r">
    /// Red gradiant.
    /// </param>
    /// <param name="g">
    /// Green gradiant.
    /// </param>
    /// <param name="b">
    /// Blue gradiant.
    /// </param>
    [Pure]
    public static Ansi Rgb(byte r, byte g, byte b)
        => r == g && r == b
        ? Gray(r)
        : Foreground(r, g, b);

    /// <summary>Sets forground color using RGB (38).</summary>
    /// <param name="rgb">
    /// 24-bit RGB code.
    /// </param>
    [Pure]
    public static Ansi Rgb(uint rgb) => rgb >> 24 is 0
        ? Rgb((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb)
        : throw new ArgumentOutOfRangeException(nameof(rgb), "RGB color contains more than 24 bit.");

    /// <summary>Group bright foreground colors.</summary>
    public static class Bright
    {
        /// <summary>Set foreground color to bright black (90).</summary>
        public static readonly Ansi Black = Single(90);

        /// <summary>Set foreground color to bright red (91).</summary>
        public static readonly Ansi Red = Single(91);

        /// <summary>Set foreground color to bright green (92).</summary>
        public static readonly Ansi Green = Single(92);

        /// <summary>Set foreground color to bright yellow (93).</summary>
        public static readonly Ansi Yellow = Single(93);

        /// <summary>Set foreground color to bright blue (94).</summary>
        public static readonly Ansi Blue = Single(94);

        /// <summary>Set foreground color to bright purple (95).</summary>
        public static readonly Ansi Purple = Single(95);

        /// <summary>Set foreground color to bright cyan (96).</summary>
        public static readonly Ansi Cyan = Single(96);

        /// <summary>Set foreground color to bright white (97).</summary>
        public static readonly Ansi White = Single(97);

        /// <summary>Group bright background colors.</summary>
        public static class BG
        {
            /// <summary>Set background color to bright black (100).</summary>
            public static readonly Ansi Black = Single(100);

            /// <summary>Set background color to bright red (101).</summary>
            public static readonly Ansi Red = Single(101);

            /// <summary>Set background color to bright green (102).</summary>
            public static readonly Ansi Green = Single(102);

            /// <summary>Set background color to bright yellow (103).</summary>
            public static readonly Ansi Yellow = Single(103);

            /// <summary>Set background color to bright blue (104).</summary>
            public static readonly Ansi Blue = Single(104);

            /// <summary>Set background color to bright purple (105).</summary>
            public static readonly Ansi Purple = Single(105);

            /// <summary>Set background color to bright cyan (106).</summary>
            public static readonly Ansi Cyan = Single(106);

            /// <summary>Set background color to bright white (107).</summary>
            public static readonly Ansi White = Single(107);
        }
    }

    /// <summary>Group background colors.</summary>
    public static class BG
    {
        /// <summary>Set background color to black (40).</summary>
        public static readonly Ansi Black = Single(40);

        /// <summary>Set background color to red (41).</summary>
        public static readonly Ansi Red = Single(41);

        /// <summary>Set background color to green (42).</summary>
        public static readonly Ansi Green = Single(42);

        /// <summary>Set background color to yellow (43).</summary>
        public static readonly Ansi Yellow = Single(43);

        /// <summary>Set background color to blue (44).</summary>
        public static readonly Ansi Blue = Single(44);

        /// <summary>Set background color to purple (45).</summary>
        public static readonly Ansi Purple = Single(45);

        /// <summary>Set background color to cyan (46).</summary>
        public static readonly Ansi Cyan = Single(46);

        /// <summary>Set background color to white (47).</summary>
        public static readonly Ansi White = Single(47);

        /// <summary>Set background color to default (49).</summary>
        public static readonly Ansi Default = Single(49);

        /// <summary>Sets background color to a gray level (48).</summary>
        /// <param name="g">
        /// Gray level, 0 is black, 255 is white.
        /// </param>
        /// <remarks>
        /// If the 256-color palette contains exactly this gray, the (shorter) palette
        /// index is used, otherwise RGB.
        /// </remarks>
        [Pure]
        public static Ansi Gray(byte g) => GrayIndex(g) is var i and >= 0 ? Background((byte)i) : Background(g, g, g);

        /// <summary>Sets background color using the 256-color palette (48).</summary>
        /// <param name="index">
        /// Palette index. Indices 0-15 are the simple colors, 16-231 a 6x6x6 color cube,
        /// and 232-255 a gray scale.
        /// </param>
        [Pure]
        public static Ansi Pal(byte index) => Background(index);

        /// <summary>Sets background color using RGB (48).</summary>
        /// <param name="r">
        /// Red gradiant.
        /// </param>
        /// <param name="g">
        /// Green gradiant.
        /// </param>
        /// <param name="b">
        /// Blue gradiant.
        /// </param>
        [Pure]
        public static Ansi Rgb(byte r, byte g, byte b)
            => r == g && r == b
            ? Gray(r)
            : Background(r, g, b);

        /// <summary>Sets background color using RGB (48).</summary>
        /// <param name="rgb">
        /// 24-bit RGB code.
        /// </param>
        [Pure]
        public static Ansi Rgb(uint rgb) => rgb >> 24 is 0
            ? Rgb((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb)
            : throw new ArgumentOutOfRangeException(nameof(rgb), "RGB color contains more than 24 bit.");
    }

    /// <summary>Gets the palette index of a gray, if the 256-color palette contains that exact gray.</summary>
    [Pure]
    private static int GrayIndex(byte gray) => gray switch
    {
        000 => 016,
        255 => 231,
        095 => 059,
        135 => 102,
        175 => 145,
        215 => 188,
        >= 8 and <= 238 when (gray - 8) % 10 is 0 => 232 + ((gray - 8) / 10),
        _ => -1,
    };

    [Pure]
    private static Ansi Foreground(byte r, byte g, byte b) => new(Bits.Fore.Set | Bits.Fore.Rgb, Pack(r, g, b), 0);

    [Pure]
    private static Ansi Background(byte r, byte g, byte b) => new(Bits.Back.Set | Bits.Back.Rgb, 0, Pack(r, g, b));

    /// <summary>Selects a color of the 256-color palette, simple colors (0-15) are SGR color codes.</summary>
    [Pure]
    private static Ansi Foreground(byte index) => index switch
    {
        <= 07 => Single((byte)(30 + index)),
        <= 15 => Single((byte)(90 + index - 8)),
        _ => new(Bits.Fore.Set | Bits.Fore.Pal, index, 0),
    };

    [Pure]
    private static Ansi Background(byte index) => index switch
    {
        <= 07 => Single((byte)(040 + index)),
        <= 15 => Single((byte)(100 + index - 8)),
        _ => new(Bits.Back.Set | Bits.Back.Pal, 0, index),
    };

    [Pure]
    private static uint Pack(byte r, byte g, byte b) => (uint)r << 16 | (uint)g << 8 | b;
}
