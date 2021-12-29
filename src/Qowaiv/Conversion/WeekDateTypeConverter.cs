namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a week date.</summary>
public class WeekDateTypeConverter : DateTypeConverter<WeekDate>
{
    /// <inheritdoc />
    [Pure]
    protected override WeekDate FromString(string? str, CultureInfo? culture) => WeekDate.Parse(str, culture);

    /// <inheritdoc />
    [Pure]
    protected override WeekDate FromDate(Date date) => date;

    /// <inheritdoc />
    [Pure]
    protected override WeekDate FromDateTime(DateTime dateTime) => (Date)dateTime;

    /// <inheritdoc />
    [Pure]
    protected override WeekDate FromDateTimeOffset(DateTimeOffset offset) => FromDateTime(offset.Date);

    /// <inheritdoc />
    [Pure]
    protected override WeekDate FromLocalDateTime(LocalDateTime local) => FromDateTime(local);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
    protected override WeekDate FromWeekDate(WeekDate weekDate) => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    protected override DateTime ToDateTime(WeekDate date) => date;

    /// <inheritdoc />
    [Pure]
    protected override DateTimeOffset ToDateTimeOffset(WeekDate date) => new(date, TimeSpan.Zero);

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime ToLocalDateTime(WeekDate date) => date;

    /// <inheritdoc />
    [Pure]
    protected override Date ToDate(WeekDate date) => date;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
    protected override WeekDate ToWeekDate(WeekDate date) => throw new NotSupportedException();
}
