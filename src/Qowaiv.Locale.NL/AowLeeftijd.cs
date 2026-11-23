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

    /// <summary>Initializes a new instance of the <see cref="AowLeeftijd"/> struct.</summary>
    /// <param name="van">The first birth date (inclusive) for which the age applies.</param>
    /// <param name="tot">The last birth date (inclusive) for which the age applies.</param>
    /// <param name="leeftijd">The AOW age that applies to this birth-date interval.</param>
    /// <param name="definitief">Whether the age has been definitively established.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="tot"/> is not after <paramref name="van"/>.
    /// </exception>
    public AowLeeftijd(Date van, Date tot, MonthSpan leeftijd, YesNo definitief)
    {
        if (tot <= van) throw new ArgumentOutOfRangeException(nameof(tot), $"{nameof(tot)} should be after {nameof(van)}.");

        Van = van;
        Tot = tot;
        Leeftijd = leeftijd;
        Definitief = definitief;
    }

    /// <inheritdoc />
    public bool HasValue => !Equals(Empty);

    /// <summary>The earliest birth date (inclusive) for which this AOW age applies.</summary>
    public Date Van { get; }

    /// <summary>The latest birth date (inclusive) for which this AOW age applies.</summary>
    public Date Tot { get; }

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

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            return $"[{Van} .. {Tot}] = {leeftijd}{(Definitief ? string.Empty : "*")}";
        }
    }

    private static readonly AowLeeftijd[] Entries = [..Init(
        new Entry(1947, 12, 31, 65.Years() + 00.Months(), true),
        new Entry(1948, 11, 30, 65.Years() + 01.Months(), true),
        new Entry(1949, 10, 31, 65.Years() + 02.Months(), true),
        new Entry(1950, 09, 30, 65.Years() + 03.Months(), true),
        new Entry(1951, 06, 30, 65.Years() + 06.Months(), true),
        new Entry(1952, 03, 31, 65.Years() + 09.Months(), true),
        new Entry(1952, 12, 31, 66.Years() + 00.Months(), true),
        new Entry(1955, 08, 31, 66.Years() + 04.Months(), true),
        new Entry(1956, 05, 31, 66.Years() + 07.Months(), true),
        new Entry(1957, 02, 28, 66.Years() + 10.Months(), true),
        new Entry(1960, 12, 31, 67.Years() + 00.Months(), true),
        new Entry(1965, 09, 30, 67.Years() + 03.Months(), true),
        new Entry(1966, 09, 30, 67.Years() + 03.Months()),
        new Entry(1970, 09, 30, 67.Years() + 06.Months()),
        new Entry(1973, 03, 31, 67.Years() + 09.Months()),
        new Entry(1975, 12, 31, 68.Years() + 00.Months()),
        new Entry(1978, 09, 30, 68.Years() + 03.Months()),
        new Entry(1982, 06, 30, 68.Years() + 06.Months()),
        new Entry(1985, 03, 31, 68.Years() + 09.Months()),
        new Entry(1988, 12, 31, 69.Years() + 00.Months()),
        new Entry(1991, 09, 30, 69.Years() + 03.Months()),
        new Entry(1995, 06, 30, 69.Years() + 06.Months()),
        new Entry(1999, 03, 31, 69.Years() + 09.Months()),
        new Entry(2000, 12, 31, 70.Years() + 00.Months()),
        new Entry(9999, 12, 31, 70.Years() + 00.Months()))
    ];

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

    /// <summary>Builds contiguous, inclusive date ranges from the table entries.</summary>
    [Pure]
    private static IEnumerable<AowLeeftijd> Init(params Entry[] entries)
    {
        var vanaf = Date.MinValue;
        foreach (var entry in entries)
        {
            var aow = new AowLeeftijd(vanaf, new(entry.Year, entry.Month, entry.Day), entry.Leeftijd, entry.Definitief ? YesNo.Yes : YesNo.No);
            yield return aow;

            if (aow.Tot != Date.MaxValue) vanaf = aow.Tot.AddDays(1);
        }
    }

    /// <summary>Helper record to declare the table concisely.</summary>
    private readonly record struct Entry(int Year, int Month, int Day, MonthSpan Leeftijd, bool Definitief = false);

    /// <summary>Compares a date with the inclusive range of a defined value, for binary search.</summary>
    private readonly struct Comparer(Date date) : IComparable<AowLeeftijd>
    {
        private readonly Date Date = date;

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
