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
    [DoesNotReturn]
    protected sealed override Date FromDate(Date date) => throw new NotSupportedException();

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
    [Pure]
    protected override Date FromYearMonth(YearMonth yearMonth) => yearMonth.ToDate(01);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [DoesNotReturn]
    protected sealed override Date ToDate(Date date) => throw new NotSupportedException();

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

    /// <inheritdoc />
    [Pure]
    protected override YearMonth ToYearMonth(Date date) => new(date.Year, date.Month);

#if NET8_0_OR_GREATER
    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
        => value is DateOnly date
        ? (Date)date
        : base.ConvertFrom(context, culture, value);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => value is Date date && Qowaiv.Reflection.QowaivType.GetNotNullableType(destinationType) == typeof(DateOnly)
        ? (DateOnly)date
        : base.ConvertTo(context, culture, value, destinationType);
#endif
}
