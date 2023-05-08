namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a Date.</summary>
[Inheritable]
public class DateTypeConverter : DateTypeConverter<Date>
{
    /// <inheritdoc />
    [Pure]
    protected override Date FromString(string? str, CultureInfo? culture) => Date.Parse(str, culture);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
    [DoesNotReturn]
    [WillBeSealed]
    protected override Date FromDate(Date date) => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    protected override Date FromDateTime(DateTime dateTime) => (Date)dateTime;

    /// <inheritdoc />
    [Pure]
    protected override Date FromDateTimeOffset(DateTimeOffset offset) => FromDateTime(offset.Date);

    /// <inheritdoc />
    [Pure]
    protected override Date FromLocalDateTime(LocalDateTime local) => FromDateTime(local);

    /// <inheritdoc />
    [Pure]
    protected override Date FromWeekDate(WeekDate weekDate) => weekDate;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
    [DoesNotReturn]
    [WillBeSealed]
    protected override Date ToDate(Date date) => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    protected override DateTime ToDateTime(Date date) => date;

    /// <inheritdoc />
    [Pure]
    protected override DateTimeOffset ToDateTimeOffset(Date date) => new(date, TimeSpan.Zero);

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime ToLocalDateTime(Date date) => ToDateTime(date);

    /// <inheritdoc />
    [Pure]
    protected override WeekDate ToWeekDate(Date date) => date;
}
