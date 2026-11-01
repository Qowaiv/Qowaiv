namespace Qowaiv.Text;

/// <summary>Char extension to help with parsing.</summary>
/// <remarks>
/// The _ after the Is ensures that we do not collide with methods on <see cref="char"/>.
/// </remarks>
internal static class Chars
{
    /// <summary>ASCII " (34 | 0x22).</summary>
    public const byte Quote = (byte)'"';

    extension(char ch)
    {
        /// <summary>Gets the digit (assumes it to be an ASCII-digit).</summary>
        public int Digit => ch - '0';

        /// <summary>Gets the upper case char (only valid for [a-z]).</summary>
        public char To_Upper_ASCII => (char)(ch & 0x5F);

        public Categories Categories => Cat(ch);

        public bool Is_ASCII => ch <= 127;

        /// <summary>True if [0-9].</summary>
        public bool Is_0_9 => (uint)(ch - '0') <= '9' - '0';

        /// <summary>True if [A-Z].</summary>
        public bool Is_A_Z => (uint)(ch - 'A') <= 'Z' - 'A';

        /// <summary>True if [a-z].</summary>
        public bool Is_a_z => (uint)(ch - 'a') <= 'z' - 'a';

        /// <summary>True if markup (including whitespace).</summary>
        public bool Is_Markup => ch <= 127
            ? (Cat(ch) & (Categories.Markup | Categories.Whitespace)) is not 0
            : char.IsWhiteSpace(ch) || Is_NonAsciiMarkup(ch);

        /// <summary>Is [0-9A-Z] (not [a-z]).</summary>
        public bool Is_IBAN => (Cat(ch) & (Categories.Upper | Categories.Digit)) is not 0;

        public bool Is_EMAIL_Local => ch > 127 || (Cat(ch) & Categories.Email_Local) is not 0;

        public bool Is_EMAIL_Domain => ch > 127 || (Cat(ch) & Categories.Email_Domain) is not 0;

        public bool Is_EMAIL_TopDomain => ch > 127 || (Cat(ch) & Categories.Email_TopDomain) is not 0;

        public bool Is_EMAIL_Punycode => ch <= 127 && (Cat(ch) & Categories.Email_Punycode) is not 0;
    }

    [Pure]
    private static bool Is_NonAsciiMarkup(char ch) => ch switch
    {
        '\u00B7' // middle dot
        or '\u22C5' // dot operator
        or '\u2022' // bullet
        or '\u2012' // figure dash / minus
        or '\u2013' // en dash
        or '\u2014' // em dash
        or '\u2015' => true, // horizontal bar
        _ => false,
    };

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Categories Cat(char c) => c <= 127 ? (Categories)Lookup[c] : default;

    [Flags]
    public enum Categories : uint
    {
        _0 /*..............*/ = 1 << 00,
        _1 /*..............*/ = 1 << 01,
        _2 /*..............*/ = 1 << 02,
        _3 /*..............*/ = 1 << 03,
        _4 /*..............*/ = 1 << 04,
        _5 /*..............*/ = 1 << 05,
        _6 /*..............*/ = 1 << 06,
        _7 /*..............*/ = 1 << 07,
        _8 /*..............*/ = 1 << 08,
        _9 /*..............*/ = 1 << 09,
        Upper /*...........*/ = 1 << 10,
        Lower /*...........*/ = 1 << 11,
        Whitespace /*......*/ = 1 << 12,
        Markup /*..........*/ = 1 << 13,
        Email_Local /*.....*/ = 1 << 14,
        Email_Domain /*....*/ = 1 << 15,
        Email_TopDomain /*.*/ = 1 << 16,
        Email_Punycode /*..*/ = 1 << 17,
        Letter = Upper | Lower,
        Digit = _0 | _1 | _2 | _3 | _4 | _5 | _6 | _7 | _8 | _9,
    }

#if NETSTANDARD2_0
    private static readonly uint[] Lookup =
#else
    private static ReadOnlySpan<uint> Lookup =>
#endif
    [
        0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x00000, 0x00000,
        0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000, 0x00000,
        0x01000, 0x04000, 0x00000, 0x04000, 0x04000, 0x04000, 0x04000, 0x04000, 0x00000, 0x00000, 0x04000, 0x04000, 0x00000, 0x26000, 0x02000, 0x04000,
        0x2c001, 0x2c002, 0x2c004, 0x2c008, 0x2c010, 0x2c020, 0x2c040, 0x2c080, 0x2c100, 0x2c200, 0x00000, 0x00000, 0x00000, 0x04000, 0x00000, 0x04000,
        0x00000, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400,
        0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x3c400, 0x00000, 0x00000, 0x00000, 0x04000, 0x0e000,
        0x04000, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800,
        0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x3c800, 0x04000, 0x04000, 0x04000, 0x04000, 0x00000,
    ];
}
