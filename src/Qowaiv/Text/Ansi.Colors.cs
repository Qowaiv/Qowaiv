#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members: Naming safes the day here.
#pragma warning disable IDE0230 // Use UTF-8 string literal: this is not about UTF-8 literals.

namespace Qowaiv.Text;

public readonly partial struct Ansi
{
    /// <summary>Set foreground color to black (30).</summary>
    public static readonly Ansi Black = new(30);

    /// <summary>Set foreground color to red (31).</summary>
    public static readonly Ansi Red = new(31);

    /// <summary>Set foreground color to green (32).</summary>
    public static readonly Ansi Green = new(32);

    /// <summary>Set foreground color to yellow (33).</summary>
    public static readonly Ansi Yellow = new(33);

    /// <summary>Set foreground color to blue (34).</summary>
    public static readonly Ansi Blue = new(34);

    /// <summary>Set foreground color to purple (35).</summary>
    public static readonly Ansi Purple = new(35);

    /// <summary>Set foreground color to cyan (36).</summary>
    public static readonly Ansi Cyan = new(36);

    /// <summary>Set foreground color to white (37).</summary>
    public static readonly Ansi White = new(37);

    /// <summary>Set foreground color to default (39).</summary>
    public static readonly Ansi Default = new(39);

    /// <summary>Sets forground color using RGB grayscale (38).</summary>
    /// <param name="g">
    /// gradiant.
    /// </param>
    [Pure]
    public static Ansi Gray(byte g) => new(38, 5, g);

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
        : new(38, 2, r, g, b);

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
        public static readonly Ansi Black = new(90);

        /// <summary>Set foreground color to bright red (91).</summary>
        public static readonly Ansi Red = new(91);

        /// <summary>Set foreground color to bright green (92).</summary>
        public static readonly Ansi Green = new(92);

        /// <summary>Set foreground color to bright yellow (93).</summary>
        public static readonly Ansi Yellow = new(93);

        /// <summary>Set foreground color to bright blue (94).</summary>
        public static readonly Ansi Blue = new(94);

        /// <summary>Set foreground color to bright purple (95).</summary>
        public static readonly Ansi Purple = new(95);

        /// <summary>Set foreground color to bright cyan (96).</summary>
        public static readonly Ansi Cyan = new(96);

        /// <summary>Set foreground color to bright white (97).</summary>
        public static readonly Ansi White = new(97);

        /// <summary>Group bright background colors.</summary>
        public static class BG
        {
            /// <summary>Set background color to bright black (100).</summary>
            public static readonly Ansi Black = new(100);

            /// <summary>Set background color to bright red (101).</summary>
            public static readonly Ansi Red = new(101);

            /// <summary>Set background color to bright green (102).</summary>
            public static readonly Ansi Green = new(102);

            /// <summary>Set background color to bright yellow (103).</summary>
            public static readonly Ansi Yellow = new(103);

            /// <summary>Set background color to bright blue (104).</summary>
            public static readonly Ansi Blue = new(104);

            /// <summary>Set background color to bright purple (105).</summary>
            public static readonly Ansi Purple = new(105);

            /// <summary>Set background color to bright cyan (106).</summary>
            public static readonly Ansi Cyan = new(106);

            /// <summary>Set background color to bright white (107).</summary>
            public static readonly Ansi White = new(107);
        }
    }

    /// <summary>Group background colors.</summary>
    public static class BG
    {
        /// <summary>Set background color to black (40).</summary>
        public static readonly Ansi Black = new(40);

        /// <summary>Set background color to red (41).</summary>
        public static readonly Ansi Red = new(41);

        /// <summary>Set background color to green (42).</summary>
        public static readonly Ansi Green = new(42);

        /// <summary>Set background color to yellow (43).</summary>
        public static readonly Ansi Yellow = new(43);

        /// <summary>Set background color to blue (44).</summary>
        public static readonly Ansi Blue = new(44);

        /// <summary>Set background color to purple (45).</summary>
        public static readonly Ansi Purple = new(45);

        /// <summary>Set background color to cyan (46).</summary>
        public static readonly Ansi Cyan = new(46);

        /// <summary>Set background color to white (47).</summary>
        public static readonly Ansi White = new(47);

        /// <summary>Set background color to default (49).</summary>
        public static readonly Ansi Default = new(49);

        /// <summary>Sets background color using RGB grayscale (48).</summary>
        /// <param name="g">
        /// gradiant.
        /// </param>
        [Pure]
        public static Ansi Gray(byte g) => new(48, 5, g);

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
            : new(48, 2, r, g, b);

        /// <summary>Sets background color using RGB (48).</summary>
        /// <param name="rgb">
        /// 24-bit RGB code.
        /// </param>
        [Pure]
        public static Ansi Rgb(uint rgb) => rgb >> 24 is 0
            ? Rgb((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb)
            : throw new ArgumentOutOfRangeException(nameof(rgb), "RGB color contains more than 24 bit.");
    }
}
