namespace Qowaiv.Conversion;

public class YearMonthTypeConverter : DateTypeConverter<YearMonth>
{
    protected override YearMonth FromDate(Date date)
    {
        throw new NotImplementedException();
    }

    protected override YearMonth FromDateTime(DateTime dateTime)
    {
        throw new NotImplementedException();
    }

    protected override YearMonth FromDateTimeOffset(DateTimeOffset offset)
    {
        throw new NotImplementedException();
    }

    protected override YearMonth FromLocalDateTime(LocalDateTime local)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    [Pure]
    protected override YearMonth FromString(string str, CultureInfo culture)=> YearMonth.Parse(str, culture);

    protected override YearMonth FromWeekDate(WeekDate weekDate)
    {
        throw new NotImplementedException();
    }

    protected override Date ToDate(YearMonth date)
    {
        throw new NotImplementedException();
    }

    protected override DateTime ToDateTime(YearMonth date)
    {
        throw new NotImplementedException();
    }

    protected override DateTimeOffset ToDateTimeOffset(YearMonth date)
    {
        throw new NotImplementedException();
    }

    protected override LocalDateTime ToLocalDateTime(YearMonth date)
    {
        throw new NotImplementedException();
    }

    protected override WeekDate ToWeekDate(YearMonth date)
    {
        throw new NotImplementedException();
    }
}
