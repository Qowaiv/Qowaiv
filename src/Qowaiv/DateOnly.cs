#if NET6_0_OR_GREATER

#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"

using Qowaiv.Globalization;

namespace Qowaiv
{
    public readonly partial struct Date
    {
        /// <summary>Casts a <see cref="Date"/> implicitly to a <see cref="DateOnly"/>.</summary>
        public static implicit operator DateOnly(Date date) => new(date.Year, date.Month, date.Day);

        /// <summary>Casts a <see cref="DateOnly"/> explicitly to a <see cref="Date"/>.</summary>
        public static explicit operator Date(DateOnly date) => new(date.Year, date.Month, date.Day);
    }

    public readonly partial struct LocalDateTime
    {
        /// <summary>Casts a <see cref="LocalDateTime"/> implicitly to a <see cref="DateOnly"/>.</summary>
        public static implicit operator DateOnly(LocalDateTime date) => new(date.Year, date.Month, date.Day);

        /// <summary>Casts a <see cref="DateOnly"/> explicitly to a <see cref="Date"/>.</summary>
        public static explicit operator LocalDateTime(DateOnly date) => new(date.Year, date.Month, date.Day);
    }

    public readonly partial struct WeekDate
    {
        /// <summary>Casts a <see cref="WeekDate"/> implicitly to a <see cref="DateOnly"/>.</summary>
        public static implicit operator WeekDate(DateOnly date) => date;

        /// <inheritdoc cref="Create(Date)"/>
        [Pure]
        public static WeekDate Create(DateOnly val) => Create((Date)val);
    }

    public readonly partial struct DateSpan
    {
        /// <inheritdoc cref="Age(Date, Date)" />
        [Pure]
        public static DateSpan Age(DateOnly date) => Age(date, Clock.Today());

        /// <inheritdoc cref="Age(Date, Date)" />
        [Pure]
        public static DateSpan Age(DateOnly date, DateOnly reference) => Subtract(reference, date, DateSpanSettings.WithoutMonths);

        /// <inheritdoc cref="Subtract(Date, Date)" />
        [Pure]
        public static DateSpan Subtract(DateOnly d1, DateOnly d2) => Subtract(d1, d2, DateSpanSettings.Default);

        /// <inheritdoc cref="Subtract(Date, Date, DateSpanSettings)" />
        [Pure]
        public static DateSpan Subtract(DateOnly d1, DateOnly d2, DateSpanSettings settings) => Subtract((Date)d1, (Date)d2, settings);
    }

    public readonly partial struct MonthSpan
    {
        /// <inheritdoc cref="Subtract(Date, Date)" />
        [Pure]
        public static MonthSpan Subtract(DateOnly d1, DateOnly d2) => Subtract((Date)d1, (Date)d2);
    }
}

namespace Qowaiv.Globalization
{
    public readonly partial struct Country
    {
        /// <inheritdoc cref="ExistsOnDate(Date)" />
        [Pure]
        public bool ExistsOnDate(DateOnly measurement) => ExistsOnDate((Date)measurement);

        /// <inheritdoc cref="GetCurrency(Date)" />
        [Pure]
        public Financial.Currency GetCurrency(DateOnly measurement) => GetCurrency((Date)measurement);

        /// <inheritdoc cref="GetExisting(Date)" />
        [Pure]
        public static IEnumerable<Country> GetExisting(DateOnly measurement) => GetExisting((Date)measurement);
    }
}

namespace Qowaiv.Financial
{
    public readonly partial struct Currency
    {
        /// <inheritdoc cref="ExistsOnDate(Date)" />
        [Pure]
        public bool ExistsOnDate(DateOnly measurement) => ExistsOnDate((Date)measurement);

        /// <inheritdoc cref="GetCountries(Date)" />
        [Pure]
        public IEnumerable<Country> GetCountries(DateOnly measurement) => GetCountries((Date)measurement);

        /// <inheritdoc cref="GetExisting(Date)" />
        [Pure]
        public static IEnumerable<Currency> GetExisting(DateOnly measurement) => GetExisting((Date)measurement);
    }
}
#endif
