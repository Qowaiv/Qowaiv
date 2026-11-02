#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members: Naming safes the day here.

namespace Qowaiv.Text;

/// <summary>Represents an ANSI instruction.</summary>
/// <remarks>
/// Layout: <c>m_Styles</c> has one bit per SGR code (bit n == code n), so 0-9 are
/// the styles (0 is reset), and 21-29 are the styles that disable. Bits 10-13 are flags
/// that indicate if the foreground/background is specified, and if it is RGB or a 256-color
/// palette index (instead of an SGR color code). <c>m_Fore</c> and <c>m_Back</c> contain the
/// SGR color code, the palette index, or the 24-bit RGB value, and are 0 if not specified.
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly partial struct Ansi : IEquatable<Ansi>
{
    private static class Bits
    {
        // 0-9 and 21-29 except 26
        public const uint Style = 0b11101_11110_00000_00000_11111_11111;
        public const uint Reset = 1u << 0;

        public static class Fore
        {
            public const uint Set = 1u << 10;
            public const uint Rgb = 1u << 11;
            public const uint Pal = 1u << 12;
        }

        public static class Back
        {
            public const uint Set = 1u << 13;
            public const uint Rgb = 1u << 14;
            public const uint Pal = 1u << 15;
        }
    }

    /// <summary>Masks of the styles that replace each other.</summary>
    private static class Group
    {
        public const uint Weight /*....*/ = 1u << 1 | 1u << 2 | 1u << 21 | 1u << 22;
        public const uint Italic /*....*/ = 1u << 3 | 1u << 23;
        public const uint Underline /*.*/ = 1u << 4 | 1u << 24;
        public const uint Blink /*.....*/ = 1u << 5 | 1u << 6 | 1u << 25;
        public const uint Invert /*....*/ = 1u << 7 | 1u << 27;
        public const uint Hide /*......*/ = 1u << 8 | 1u << 28;
        public const uint Strike /*....*/ = 1u << 9 | 1u << 29;
    }

    private Ansi(uint styles, uint fore, uint back)
    {
        m_Styles = styles;
        m_Fore = fore;
        m_Back = back;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly uint m_Styles;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly uint m_Fore;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly uint m_Back;

    /// <summary>The default (nothing set) is a reset.</summary>
    private bool IsReset => (m_Styles & Bits.Reset) is not 0 || (m_Styles | m_Fore | m_Back) is 0;

    /// <summary>Merges the two instructions, the second one wins on conflicts.</summary>
    [Pure]
    private Ansi Merge(Ansi other)
    {
        if (other.IsReset) return other;

        var replaced = Replace(other.m_Styles);

        if ((other.m_Styles & Bits.Fore.Set) is not 0) replaced |= Bits.Fore.Set | Bits.Fore.Rgb | Bits.Fore.Pal;
        if ((other.m_Styles & Bits.Back.Set) is not 0) replaced |= Bits.Back.Set | Bits.Back.Rgb | Bits.Back.Pal;

        var styles = (m_Styles & ~replaced) | other.m_Styles;

        // Merging onto a reset, remains a reset.
        if (IsReset) styles |= Bits.Reset;

        return new(
            styles,
            (other.m_Styles & Bits.Fore.Set) is 0 ? m_Fore : other.m_Fore,
            (other.m_Styles & Bits.Back.Set) is 0 ? m_Back : other.m_Back);
    }

    [Pure]
    private static uint Replace(uint styles)
        => ((styles & Group.Weight) is 0 ? 0 : Group.Weight)
        | ((styles & Group.Italic) is 0 ? 0 : Group.Italic)
        | ((styles & Group.Underline) is 0 ? 0 : Group.Underline)
        | ((styles & Group.Blink) is 0 ? 0 : Group.Blink)
        | ((styles & Group.Invert) is 0 ? 0 : Group.Invert)
        | ((styles & Group.Hide) is 0 ? 0 : Group.Hide)
        | ((styles & Group.Strike) is 0 ? 0 : Group.Strike);

    /// <summary>Creates an instruction for a single SGR (color or style) code.</summary>
    [Pure]
    private static Ansi Single(byte sgr) => sgr switch
    {
        <= 29 => new(1u << sgr, 0, 0),
        <= 39 or (>= 90 and <= 97) => new(Bits.Fore.Set, sgr, 0),
        _ => new(Bits.Back.Set, 0, sgr),
    };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"""\x1b{ToString()[1..]}""";

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Ansi other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Ansi other)
        => m_Styles == other.m_Styles
        && m_Fore == other.m_Fore
        && m_Back == other.m_Back;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Styles).And(m_Fore).And(m_Back);

    /// <summary>Merges the two instructions.</summary>
    public static Ansi operator |(Ansi l, Ansi r) => l.Merge(r);

    /// <summary>Returns true if the two instructions are equal.</summary>
    public static bool operator ==(Ansi l, Ansi r) => l.Equals(r);

    /// <summary>Returns false if the two instructions are equal.</summary>
    public static bool operator !=(Ansi l, Ansi r) => !(l == r);
}
