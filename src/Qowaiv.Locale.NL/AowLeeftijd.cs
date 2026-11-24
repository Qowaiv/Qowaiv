using Qowaiv.Humanizer;

namespace Qowaiv.Locale.NL;

/// <summary>Represents the Dutch state pension (AOW) age that applies to a date of birth.</summary>
/// <remarks>
/// The AOW age depends on the date of birth and is set by the Sociale Verzekeringsbank (SVB).
/// For people born on or after 1 October 1965 the age has not been finalized; it may change
/// due to legislation.
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct AowLeeftijd :
    IEquatable<AowLeeftijd>,
    IEmpty<AowLeeftijd>,
    IEnumerated
{
    /// <summary>An unset or unknown AOW age.</summary>
    public static AowLeeftijd Empty => default;

    /// <summary>Gets all defined AOW ages, ordered by <see cref="Van"/>.</summary>
    public static IReadOnlyCollection<AowLeeftijd> DefinedValues => [.. Entries];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly AowLeeftijd[] Entries =
    [
        new(new(0001, 01), new(1947, 12), 65.Years() + 00.Months(), YesNo.Yes),
        new(new(1948, 01), new(1948, 11), 65.Years() + 01.Months(), YesNo.Yes),
        new(new(1948, 12), new(1949, 10), 65.Years() + 02.Months(), YesNo.Yes),
        new(new(1949, 11), new(1950, 09), 65.Years() + 03.Months(), YesNo.Yes),
        new(new(1950, 10), new(1951, 06), 65.Years() + 06.Months(), YesNo.Yes),
        new(new(1951, 07), new(1952, 03), 65.Years() + 09.Months(), YesNo.Yes),
        new(new(1952, 04), new(1952, 12), 66.Years() + 00.Months(), YesNo.Yes),
        new(new(1953, 01), new(1955, 08), 66.Years() + 04.Months(), YesNo.Yes),
        new(new(1955, 09), new(1956, 05), 66.Years() + 07.Months(), YesNo.Yes),
        new(new(1956, 06), new(1957, 02), 66.Years() + 10.Months(), YesNo.Yes),
        new(new(1957, 03), new(1960, 12), 67.Years() + 00.Months(), YesNo.Yes),
        new(new(1961, 01), new(1965, 09), 67.Years() + 03.Months(), YesNo.Yes),
        new(new(1965, 10), new(1966, 09), 67.Years() + 03.Months(), YesNo.No),
        new(new(1966, 10), new(1970, 09), 67.Years() + 06.Months(), YesNo.No),
        new(new(1970, 10), new(1973, 03), 67.Years() + 09.Months(), YesNo.No),
        new(new(1973, 04), new(1975, 12), 68.Years() + 00.Months(), YesNo.No),
        new(new(1976, 01), new(1978, 09), 68.Years() + 03.Months(), YesNo.No),
        new(new(1978, 10), new(1982, 06), 68.Years() + 06.Months(), YesNo.No),
        new(new(1982, 07), new(1985, 03), 68.Years() + 09.Months(), YesNo.No),
        new(new(1985, 04), new(1988, 12), 69.Years() + 00.Months(), YesNo.No),
        new(new(1989, 01), new(1991, 09), 69.Years() + 03.Months(), YesNo.No),
        new(new(1991, 10), new(1995, 06), 69.Years() + 06.Months(), YesNo.No),
        new(new(1995, 07), new(1999, 03), 69.Years() + 09.Months(), YesNo.No),
        new(new(1999, 04), new(2000, 12), 70.Years() + 00.Months(), YesNo.No),
        new(new(2001, 01), new(9999, 12), 70.Years() + 00.Months(), YesNo.No),
    ];

    /// <summary>Initializes a new instance of the <see cref="AowLeeftijd"/> struct.</summary>
    /// <param name="van">The first birth date (inclusive) for which the age applies.</param>
    /// <param name="tot">The last birth date (inclusive) for which the age applies.</param>
    /// <param name="leeftijd">The AOW age that applies to this birth-date interval.</param>
    /// <param name="definitief">Whether the age has been definitively established.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="tot"/> is not after <paramref name="van"/>.
    /// </exception>
    private AowLeeftijd(YearMonth van, YearMonth tot, MonthSpan leeftijd, YesNo definitief)
    {
        Van = van;
        Tot = tot;
        Leeftijd = leeftijd;
        Definitief = definitief;
    }

    /// <inheritdoc />
    public bool HasValue => !Equals(Empty);

    /// <summary>The earliest birth date (inclusive) for which this AOW age applies.</summary>
    public YearMonth Van { get; }

    /// <summary>The latest birth date (inclusive) for which this AOW age applies.</summary>
    public YearMonth Tot { get; }

    /// <summary>The AOW age that applies to this birth-date interval.</summary>
    public MonthSpan Leeftijd { get; }

    /// <summary>Whether the AOW age has been definitively established.</summary>
    public YesNo Definitief { get; }

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AowLeeftijd other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(AowLeeftijd other)
        => Van == other.Van
        && Tot == other.Tot
        && Leeftijd == other.Leeftijd
        && Definitief == other.Definitief;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash
        .Code(Van)
        .And(Tot)
        .And(Leeftijd)
        .And(Definitief);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"Van = {Van}, Tot = {Tot}, Leeftijd = {Leeftijd}, Definitief = {Definitief}";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage(/*Justification = Debugger experience only*/)]
    private string DebuggerDisplay
    {
        get
        {
            var leeftijd = Leeftijd.Months switch
            {
                0 => $"{Leeftijd.Years} jaar",
                1 => $"{Leeftijd.Years} jaar en {Leeftijd.Months} maand",
                _ => $"{Leeftijd.Years} jaar en {Leeftijd.Months} maanden",
            };
            return $"[{Van:yyyy-MM}, {Tot:yyyy-MM}] = {leeftijd}{(Definitief ? string.Empty : "*")}";
        }
    }

    /// <summary>Gets the AOW age that applies to the given date of birth.</summary>
    /// <param name="geboortedatum">The date of birth for which to determine the AOW age.</param>
    /// <returns>
    /// The corresponding <see cref="AowLeeftijd"/>. Every date within the range of <see cref="Date"/>
    /// falls within one of the defined intervals.
    /// </returns>
    /// <example>
    /// <code>
    /// var aow = AowLeeftijd.Geboortedatum(new Date(1980, 02, 14)); // 68 jaar en 6 maanden
    /// </code>
    /// </example>
    [Pure]
    public static AowLeeftijd Geboortedatum(Date geboortedatum)
        => Entries[Entries.AsSpan().BinarySearch(new Comparer(geboortedatum))];

    /// <summary>Compares a date with the inclusive range of a defined value, for binary search.</summary>
    private readonly struct Comparer(Date date) : IComparable<AowLeeftijd>
    {
        private readonly YearMonth Date = (YearMonth)date;

        /// <inheritdoc />
        [Pure]
        public int CompareTo(AowLeeftijd other) => true switch
        {
            _ when Date > other.Tot => +1,
            _ when Date < other.Van => -1,
            _ => 0,
        };
    }
}
