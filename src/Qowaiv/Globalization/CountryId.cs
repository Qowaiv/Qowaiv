namespace Qowaiv.Globalization;

/// <summary>Handles a a key for Country based lookups.</summary>
/// <remarks>
/// Current countries indexes are calculated <see cref="Iso(char, char)"/>.
/// Others have an assigned value.
/// </remarks>
internal readonly struct CountryId(int index) : IEquatable<CountryId>
{
    public const int Regular = 26 * 26;
    private const int Unknown = Regular - 1;
    private const int Empty = Regular + 1;
    private const int ANHH = Empty + 1;
    private const int CSXX = ANHH + 1;
    private const int YUCS = CSXX + 1;
    private const int TPTL = YUCS + 1;
    private const int FXFR = TPTL + 1;
    private const int ZRCD = FXFR + 1;
    private const int CSHH = ZRCD + 1;
    private const int NTHH = CSHH + 1;
    private const int SUHH = NTHH + 1;
    private const int BYAA = SUHH + 1;
    private const int DDDE = BYAA + 1;
    private const int BUMM = DDDE + 1;
    private const int YDYE = BUMM + 1;
    private const int JTUM = YDYE + 1;
    private const int MIUM = JTUM + 1;
    private const int PCHH = MIUM + 1;
    private const int PUUM = PCHH + 1;
    private const int WKUM = PUUM + 1;
    private const int HVBF = WKUM + 1;
    private const int CTKI = HVBF + 1;
    private const int NQAQ = CTKI + 1;
    private const int BQAQ = NQAQ + 1;
    private const int GEHH = BQAQ + 1;
    private const int FQHH = GEHH + 1;
    private const int NHVU = FQHH + 1;
    private const int PZPA = NHVU + 1;
    private const int RHZW = PZPA + 1;
    private const int AIDJ = RHZW + 1;
    private const int VDVN = AIDJ + 1;
    private const int DYBJ = VDVN + 1;
    private const int SKIN = DYBJ + 1;
    public const int MaxSize = SKIN + 1;
    private readonly int Index = index;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is CountryId other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(CountryId other) => Index == other.Index;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Index;

    /// <inheritdoc />
    [Pure]
    public override string ToString()
        => Index is >= 0 and < Regular
        ? $"{(char)((Index / 26) + 'A')}{(char)((Index % 26) + 'A')}"
        : Index.ToString();

    public static explicit operator int(CountryId id) => id.Index;

    [Pure]
#pragma warning disable S1541 // Methods and properties should not be too complex
    internal static CountryId New(Country c) => c switch
    {
        { HasValue: false } => new(Empty),
        _ when c.Name is { Length: 2 } name => Iso(name),
        _ when c == Country.ANHH => new(ANHH),
        _ when c == Country.CSXX => new(CSXX),
        _ when c == Country.YUCS => new(YUCS),
        _ when c == Country.TPTL => new(TPTL),
        _ when c == Country.FXFR => new(FXFR),
        _ when c == Country.ZRCD => new(ZRCD),
        _ when c == Country.CSHH => new(CSHH),
        _ when c == Country.NTHH => new(NTHH),
        _ when c == Country.SUHH => new(SUHH),
        _ when c == Country.BYAA => new(BYAA),
        _ when c == Country.DDDE => new(DDDE),
        _ when c == Country.BUMM => new(BUMM),
        _ when c == Country.YDYE => new(YDYE),
        _ when c == Country.JTUM => new(JTUM),
        _ when c == Country.MIUM => new(MIUM),
        _ when c == Country.PCHH => new(PCHH),
        _ when c == Country.PUUM => new(PUUM),
        _ when c == Country.WKUM => new(WKUM),
        _ when c == Country.HVBF => new(HVBF),
        _ when c == Country.CTKI => new(CTKI),
        _ when c == Country.NQAQ => new(NQAQ),
        _ when c == Country.BQAQ => new(BQAQ),
        _ when c == Country.GEHH => new(GEHH),
        _ when c == Country.FQHH => new(FQHH),
        _ when c == Country.NHVU => new(NHVU),
        _ when c == Country.PZPA => new(PZPA),
        _ when c == Country.RHZW => new(RHZW),
        _ when c == Country.AIDJ => new(AIDJ),
        _ when c == Country.VDVN => new(VDVN),
        _ when c == Country.DYBJ => new(DYBJ),
        _ when c == Country.SKIN => new(SKIN),
        _ => new(Unknown),
    };
#pragma warning restore S1541 // Methods and properties should not be too complex

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CountryId Iso(char f, char s) => new(((f - 'A') * 26) + (s - 'A'));

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CountryId Iso(ReadOnlySpan<char> iso2) => Iso(iso2[0], iso2[1]);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CountryId? TryIso(char f, char s)
        => IsLetter(f) && IsLetter(s)
        ? Iso(f, s)
        : null;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLetter(uint c) => c - 'A' <= 'Z' - 'A';
}
