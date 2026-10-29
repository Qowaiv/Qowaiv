using System.Diagnostics.SymbolStore;

namespace Qowaiv.Text;

/// <summary>Represents an ANSI instruction.</summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly partial struct Ansi : IEquatable<Ansi>
{
    private Ansi(params byte[] codes) => Codes = codes;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly byte[] Codes;

    /// <summary>Adds to ANSI instructions together.</summary>
    /// <param name="other">
    /// The ANSI instruction to add.
    /// </param>
    [Pure]
    private Ansi Merge(Ansi other)
    {
        // Other is/starts with a reset.
        if (other.Codes is null || other.Codes[0] is 0) return other;

        Span<byte> buffer = stackalloc byte[32];
        var size = 0;
        var f = (Codes ?? [0]).AsSpan();
        var s = (other.Codes ?? [0]).AsSpan();

        var styles = 0u;

        for (var i = 0; i < f.Length; i++)
        {
            if (f[i] is not { IsStyle: true } c) break;
            styles |= 1u << c;
            styles &= Replacements[c];
        }

        for (var i = 0; i < s.Length; i++)
        {
            if (s[i] is not { IsStyle: true } c) break;
            styles |= 1u << c;
            styles &= Replacements[c];
        }

        byte style = 0;
        while (styles is not 0)
        {
            if ((styles & 1) is 1)
                buffer[size++] = style;

            style++;
            styles >>= 1;
        }

        var fore = s.Foreground;
        if (fore.Length is 0)
            fore = f.Foreground;

        var back = s.Background;
        if (back.Length is 0)
            back = f.Background;

        fore.CopyTo(buffer[size..]);
        size += fore.Length;
        back.CopyTo(buffer[size..]);
        size += back.Length;

        return size is 0
            ? Reset
            : new(buffer[..size].ToArray());
    }

    private static readonly uint[] Replacements =
    [
        1,
        // Bold        (1) => Not Bold, Dim   (2, 21)
        0b11111_11110_00000_00000_01111_11101_1,
        // Dim         (2) => Bold                (1)
        0b11111_11111_00000_00000_01111_11110_1,
        // Italic      (3) => Not Italic         (23)
        0b11111_11011_00000_00000_01111_11111_1,
        // Underline   (4) => Not Underline      (24)
        0b11111_10111_00000_00000_01111_11111_1,
        // Slow blink  (5) => Fast/Not Blink  (6, 25)
        0b11111_01111_00000_00000_01110_11111_1,
        // Rapid blink (6) => Rapid/Not Blink (5, 25)
        0b11111_01111_00000_00000_01111_01111_1,
        // Invert     (7) => Not Invert          (27)
        0b11101_11111_00000_00000_01111_11111_1,
        // Hide       (8) => Not Hide            (28)
        0b11011_11111_00000_00000_01111_11111_1,
        // Strike     (9) => Not Strike          (29)
        0b10111_11111_00000_00000_01111_11111_1,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        // Not Bold      (21) => Bold                (1)
        0b11111_11111_00000_00000_01111_11110_1,
        // Not Bold/Dim  (22) => Bold, Dim        (1, 2)
        0b11111_11111_00000_00000_01111_11100_1,
        // Not Italic    (23) => Italic              (3)
        0b11111_11111_00000_00000_01111_11011_1,
        // Not Underline (24) => Underline           (4)
        0b11111_11111_00000_00000_01111_10111_1,
        // Not Blink     (25) => Rapid/SLow Blink (5, 6)
        0b11111_11111_00000_00000_01110_01111_1,
        0,
        // Not Invert     (27) => Invert             (7)
        0b11111_11111_00000_00000_01101_11111_1,
        // Not Hide       (28) => Hide               (8)
        0b11111_11111_00000_00000_01011_11111_1,
        // Not Strike     (29) => Strike             (9)
        0b11111_11111_00000_00000_00111_11111_1,
    ];

    /// <inheritdoc />
    [Pure]
    public override string ToString()
#if NETSTANDARD2_0
        => Codes is not { Length: > 0 }
        ? "\x1b[0m"
        : $"\x1b[{string.Join(";", Codes)}m";
#else
    {
        Span<char> buffer = stackalloc char[128];
        TryFormat(buffer, out var length, default, null);
        return buffer[..length].ToString();
    }
#endif

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"""\x1b{ToString()[1..]}""";

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Ansi other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Ansi other) => (Codes, other.Codes) switch
    {
        ({ }, { }) => Enumerable.SequenceEqual(Codes, other.Codes),
        (null, null) => true,
        _ => false,
    };

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(Codes);

    /// <summary>Merges the two instructions.</summary>
    public static Ansi operator |(Ansi l, Ansi r) => l.Merge(r);
}

file static class Extensions
{
    extension(ReadOnlySpan<byte> bytes)
    {
        public ReadOnlySpan<byte> Foreground
        {
            get
            {
                var span = bytes;
                while (span.Length is not 0 && span[0].IsStyle) span = span[1..];

                return span switch
                {
                    { Length: 0 } => [],
                    _ when span[0].IsForeground => span[0] switch
                    {
                        38 when span[1].IsRgb => span[..5],
                        38 when span[1].IsGray => span[..2],
                        _ => span[..1],
                    },
                    _ => [],
                };
            }
        }

        public ReadOnlySpan<byte> Background
        {
            get
            {
                var span = bytes;
                while (span.Length is not 0 && span[0] is { IsBackground: false }) span = span[1..];

                return span switch
                {
                    { Length: 0 } => [],
                    _ when span[0].IsBackground => span[0] switch
                    {
                        48 when span[1].IsRgb => span[..5],
                        48 when span[1].IsGray => span[..2],
                        _ => span[..1],
                    },
                    _ => [],
                };
            }
        }

        public bool HasReset => bytes.Length > 0 && bytes[0] is 0;
    }

    extension(byte c)
    {
        public bool IsGray => c is 5;

        public bool IsRgb => c is 2;

        public bool IsStyle => c is <= 29;

        public bool IsForeground => c is >= 30 and <= 39 or >= 90 and <= 97;

        public bool IsBackground => c is >= 40 and <= 49 or >= 100 and <= 107;
    }
}
