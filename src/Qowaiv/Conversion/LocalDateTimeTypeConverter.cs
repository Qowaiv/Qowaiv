namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a local date time.</summary>
[Inheritable]
public class LocalDateTimeTypeConverter : DateTypeConverter<LocalDateTime>
{
    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime FromString(string? str, CultureInfo? culture) => LocalDateTime.Parse(str, culture);

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime FromDate(Date date) => (DateTime)date;

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime FromDateTime(DateTime dateTime) => dateTime;

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime FromDateTimeOffset(DateTimeOffset offset) => FromDateTime(offset.DateTime);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
    [DoesNotReturn]
    protected sealed override LocalDateTime FromLocalDateTime(LocalDateTime local) => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime FromWeekDate(WeekDate weekDate) => weekDate;

    /// <inheritdoc />
    [Pure]
    protected override DateTime ToDateTime(LocalDateTime date) => date;

    /// <inheritdoc />
    [Pure]
    protected override DateTimeOffset ToDateTimeOffset(LocalDateTime date)
        => new(new DateTime(date.Ticks, DateTimeKind.Utc), TimeSpan.Zero);

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime FromYearMonth(YearMonth yearMonth) => new(year: yearMonth.Year, month: yearMonth.Month, day: 01);
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
    [DoesNotReturn]
    protected sealed override LocalDateTime ToLocalDateTime(LocalDateTime date) => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    protected override Date ToDate(LocalDateTime date) => (Date)ToDateTime(date);

    /// <inheritdoc />
    [Pure]
    protected override WeekDate ToWeekDate(LocalDateTime date) => ToDate(date);

    /// <inheritdoc />
    [Pure]
    protected override YearMonth ToYearMonth(LocalDateTime date)=> new(date.Year, date.Month);
}
