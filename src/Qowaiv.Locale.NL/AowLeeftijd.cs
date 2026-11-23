using Qowaiv.Humanizer;

namespace Qowaiv.Locale.NL;

/// <summary>Representeert de AOW-leeftijd.</summary>
/// <remarks>
/// Published by https://www.svb.nl/nl/aow/aow-leeftijd/uw-aow-leeftijd.
/// </remarks>
public readonly struct AowLeeftijd :
    IEquatable<AowLeeftijd>,
    IEmpty<AowLeeftijd>,
    IEnumerated
{
    /// <summary>Een niet-gezette/onbekende AOW-leeftijd.</summary>
    public static AowLeeftijd Empty => default;

    /// <summary>Gets all defined values.</summary>
    public static IReadOnlyCollection<AowLeeftijd> DefinedValues => [.. Entries];

    /// <summary>Initializes a new instance of the <see cref="AowLeeftijd"/> struct.</summary>
    public AowLeeftijd(Date vanaf, Date tot, MonthSpan leeftijd, YesNo definitief)
    {
        if (tot <= vanaf) throw new ArgumentOutOfRangeException(nameof(tot), $"{nameof(tot)} should be after {nameof(vanaf)}.");

        Vanaf = vanaf;
        Tot = tot;
        Leeftijd = leeftijd;
        Definitief = definitief;
    }

    /// <inheritdoc />
    public bool HasValue => !Equals(Empty);

    /// <summary>De minimale geboortedatum waarvoor deze AOW-leeftijd geldt.</summary>
    public Date Vanaf { get; }

    /// <summary>De maximale geboortedatum waarvoor deze AOW-leeftijd geldt.</summary>
    public Date Tot { get; }

    /// <summary>De vastgestelde AOW-leeftijd.</summary>
    public MonthSpan Leeftijd { get; }

    /// <summary>Geeft aan of de vasstelling definitief is.</summary>
    public YesNo Definitief { get; }

    /// <inheritdoc />
    [Pure]
    public override string ToString()
        => $"[{Vanaf} .. {Tot}] = {Leeftijd.Years} jaar en {Leeftijd.Months} maand(en){(Definitief ? string.Empty : "*")}";

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AowLeeftijd other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(AowLeeftijd other)
        => Vanaf == other.Vanaf
        && Tot == other.Tot
        && Leeftijd == other.Leeftijd
        && Definitief == other.Definitief;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash
        .Code(Vanaf)
        .And(Tot)
        .And(Leeftijd)
        .And(Definitief);

    private static readonly AowLeeftijd[] Entries = [..Init(
        new Entry(1947, 12, 31, 65.Years() + 00.Months(), true),
        new Entry(1948, 11, 30, 65.Years() + 01.Months(), true),
        new Entry(1949, 10, 31, 65.Years() + 02.Months(), true),
        new Entry(1950, 09, 30, 65.Years() + 03.Months(), true),
        new Entry(1951, 06, 30, 65.Years() + 06.Months(), true),
        new Entry(1952, 03, 31, 65.Years() + 09.Months(), true),
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

    /// <summary>Geeft de AOW-leeftijd voor de gevraagde geboortedatum.</summary>
    [Pure]
    public static AowLeeftijd Geboortedatum(Date geboortedatum)
        => Entries[Entries.AsSpan().BinarySearch(new Comparer(geboortedatum))];

    [Pure]
    private static IEnumerable<AowLeeftijd> Init(params Entry[] entries)
    {
        var vanaf = Date.MinValue;
        foreach (var entry in entries)
        {
            var aow = new AowLeeftijd(vanaf, new(entry.Jaar, entry.Maand, entry.Dag), entry.Leeftijd, entry.Definitief ? YesNo.Yes : YesNo.No);
            yield return aow;

            if (aow.Tot != Date.MaxValue) vanaf = aow.Tot.AddDays(1);
        }
    }

    /// <summary>Entry only exists to simplify initialization of the table.</summary>
    private readonly record struct Entry(int Jaar, int Maand, int Dag, MonthSpan Leeftijd, bool Definitief = false);

    /// <summary>Allows binary-search on records.</summary>
    private readonly struct Comparer(Date date) : IComparable<AowLeeftijd>
    {
        private readonly Date Date = date;

        /// <inheritdoc />
        [Pure]
        public int CompareTo(AowLeeftijd other) => true switch
        {
            _ when Date > other.Tot => +1,
            _ when Date < other.Vanaf => -1,
            _ => 0,
        };
    }
}
