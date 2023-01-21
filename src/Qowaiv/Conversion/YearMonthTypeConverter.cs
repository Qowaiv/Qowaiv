namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a year-month.</summary>
[Inheritable]
public class YearMonthTypeConverter : DateTypeConverter<YearMonth>
{
    /// <inheritdoc />
    [Pure]
    protected override YearMonth FromDate(Date date) => (YearMonth)date;

    /// <inheritdoc />
    [Pure]
    protected override YearMonth FromDateTime(DateTime dateTime) => (YearMonth)dateTime;

    /// <inheritdoc />
    [Pure]
    protected override YearMonth FromDateTimeOffset(DateTimeOffset offset) => new(offset.Year, offset.Month);

    /// <inheritdoc />
    [Pure]
    protected override YearMonth FromLocalDateTime(LocalDateTime local) => (YearMonth)local;

    /// <inheritdoc />
    [Pure]
    protected override YearMonth FromString(string? str, CultureInfo? culture) => YearMonth.Parse(str, culture);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
#if NET5_0_OR_GREATER
    [DoesNotReturn]
#endif
    protected override YearMonth FromWeekDate(WeekDate weekDate) => throw new NotImplementedException();

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
#if NET5_0_OR_GREATER
    [DoesNotReturn]
#endif
    protected override YearMonth FromYearMonth(YearMonth yearMonth) => throw new NotImplementedException();

    /// <inheritdoc />
    [Pure]
    protected override Date ToDate(YearMonth date) => (Date)date;
    
    /// <inheritdoc />
    [Pure]
    protected override DateTime ToDateTime(YearMonth date) => (DateTime)date;

    /// <inheritdoc />
    [Pure]
    protected override DateTimeOffset ToDateTimeOffset(YearMonth date) => new(date.ToDate(01), TimeSpan.Zero);

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime ToLocalDateTime(YearMonth date) => (LocalDateTime)date;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
#if NET5_0_OR_GREATER
    [DoesNotReturn]
#endif
    protected override WeekDate ToWeekDate(YearMonth date) => throw new NotImplementedException();

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [Pure]
#if NET5_0_OR_GREATER
    [DoesNotReturn]
#endif
    protected override YearMonth ToYearMonth(YearMonth date) => throw new NotImplementedException();
}
