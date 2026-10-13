namespace Qowaiv.Globalization;

/// <summary>Handles a a key for Country based lookups.</summary>
internal readonly struct CountryId(int value) : IEquatable<CountryId>
{
    private const int Regular = 26 * 26;
    private readonly int Value = value;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is CountryId other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(CountryId other) => Value == other.Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Value;

    /// <inheritdoc />
    [Pure]
    public override string ToString()
    {
        if (Value < Regular)
            return $"{(char)((Value / 26) + 'A')}{(char)((Value % 26) + 'A')}";
        else
            return Value.ToString();
    }

    public static explicit operator int(CountryId id) => id.Value;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CountryId Iso(char f, char s) => new(((f - 'A') * 26) + (s - 'A'));

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
