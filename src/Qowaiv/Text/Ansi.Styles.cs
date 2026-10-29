#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members: Naming safes the day here.
#pragma warning disable IDE0230 // Use UTF-8 string literal: this is not about UTF-8 literals.

namespace Qowaiv.Text;

public readonly partial struct Ansi
{
    /// <summary>Reset/set to normal (0).</summary>
    public static readonly Ansi Reset;

    /// <summary>Sets style to bold (1).</summary>
    public static readonly Ansi Bold = Single(1);

    /// <summary>Set style to dim (2).</summary>
    public static readonly Ansi Dim = Single(2);

    /// <summary>Set style to italic (3).</summary>
    public static readonly Ansi Italic = Single(3);

    /// <summary>Set style to underline (4).</summary>
    public static readonly Ansi Underline = Single(4);

    /// <summary>Set style to slow blink (5).</summary>
    public static readonly Ansi SlowBlink = Single(5);

    /// <summary>Set style to rapid blink (6).</summary>
    public static readonly Ansi RapidBlink = Single(6);

    /// <summary>Invert foreground and background color (7).</summary>
    public static readonly Ansi Invert = Single(7);

    /// <summary>Set style to hide/conceal (8).</summary>
    public static readonly Ansi Hide = Single(8);

    /// <summary>Set style to strike (through) (9).</summary>
    public static readonly Ansi Strike = Single(9);

    /// <summary>Group of style dislable instructions.</summary>
    public static class Not
    {
        /// <summary>Clears bold style (21).</summary>
        public static readonly Ansi Bold = Single(21);

        /// <summary>Clears bold and dim style (22).</summary>
        public static readonly Ansi BoldOrDim = Single(22);

        /// <summary>Clears italic style (23).</summary>
        public static readonly Ansi Italic = Single(23);

        /// <summary>Clears italic style (24).</summary>
        public static readonly Ansi Underline = Single(24);

        /// <summary>Clears blink style (24).</summary>
        public static readonly Ansi Blink = Single(25);

        /// <summary>Clears style invert (27).</summary>
        public static readonly Ansi Invert = Single(27);

        /// <summary>Clears style to hide/conceal (28).</summary>
        public static readonly Ansi Hide = Single(28);

        /// <summary>Set style to strike (through) (29).</summary>
        public static readonly Ansi Strike = Single(29);
    }
}
