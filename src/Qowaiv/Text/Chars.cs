namespace Qowaiv.Text;

/// <summary>Char extension to help whith parsing.</summary>
/// <remarks>
/// The _ after the Is ensures that we do not colide with methods on <see cref="char"/>.
/// </remarks>
internal static class Chars
{
    extension(char ch)
    {
        /// <summary>Gets the digit (assumes it to be an ASCII-digit).</summary>
        public int Digit => ch - '0';

        public char With_UpperMask => (char)(ch & 0x5F);

        public Catagories Categories => Cat(ch);

        /// <summary>True if [0-9].</summary>
        public bool Is_0_9 => (uint)(ch - '0') <= '9' - '0';

        /// <summary>True if [A-Z].</summary>
        public bool Is_A_Z => (uint)(ch - 'A') <= 'Z' - 'A';

        /// <summary>True if [a-z].</summary>
        public bool Is_a_z => (uint)(ch - 'a') <= 'z' - 'a';

        /// <summary>True if markup (including whitespace).</summary>
        public bool Is_Markup => Cat(ch) switch
        {
            var c when (c & (Catagories.Markup | Catagories.Whitespace)) is not 0 => true,
            _ when ch <= (char)127 => false,
            _ when char.IsWhiteSpace(ch) => true,
            _ => Is_NonAsciiMarkup(ch),
        };

        /// <summary>Is [0-9A-Z] (not [a-z]).</summary>
        public bool Is_IBAN => (Cat(ch) & (Catagories.Upper | Catagories.Digit)) is not 0;

        public bool Is_EMAIL_Local => ch > 127 || (Cat(ch) & Catagories.Email_Local) is not 0;

        public bool Is_EMAIL_Domain => ch > 127 || (Cat(ch) & Catagories.Email_Domain) is not 0;

        public bool Is_EMAIL_TopDomain => ch > 127 || (Cat(ch) & Catagories.Email_TopDomain) is not 0;

        public bool Is_EMAIL_Punycode => (Cat(ch) & Catagories.Email_Punycode) is not 0;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Is_NonAsciiMarkup(char ch) => ch
        is (char)0x00B7 // middle dot
        or (char)0x22C5 // dot operator
        or (char)0x2202 // bullet
        or (char)0x2012 // figure dash / minus
        or (char)0x2013 // en dash
        or (char)0x2014 // em dash
        or (char)0x2015; // horizontal bar

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Catagories Cat(char c) => (Catagories)Lookup[(byte)c];

    [Flags]
    public enum Catagories : uint
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

        // Non-ASCII
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    ];
}
