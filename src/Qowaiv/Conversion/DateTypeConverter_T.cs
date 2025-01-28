using Qowaiv.Reflection;

namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a Date types.</summary>
public abstract class DateTypeConverter<T> : TypeConverter where T : struct, IFormattable
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => IsConvertable(sourceType) || base.CanConvertFrom(context, sourceType);

#pragma warning disable S1541

    /// <inheritdoc />
    /// <remarks>
    /// the switch is considered too complex, but I see no way to write it down
    /// simpler than it is, and will stick to it.
    /// </remarks>
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) => value switch
    {
        null => default(T),
        T self => self,
        string str => FromString(str, culture),
        DateTime /*.......*/ date => FromDateTime(date),
#if NET6_0_OR_GREATER
        DateOnly /*.......*/ date => FromDate((Date)date),
#endif
        DateTimeOffset /*.*/ date => FromDateTimeOffset(date),
        LocalDateTime /*..*/ date => FromLocalDateTime(date),
        Date /*...........*/ date => FromDate(date),
        WeekDate /*.......*/ date => FromWeekDate(date),
        YearMonth /*......*/ date => FromYearMonth(date),
        _ => base.ConvertFrom(context, culture, value),
    };
#pragma warning restore S1541

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => (destinationType != typeof(T) && QowaivType.IsDate(destinationType))
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) => Guard.NotNull(destinationType) switch
    {
        _ when QowaivType.IsNullOrDefaultValue(value) => ConverToDefault(destinationType),
        _ when destinationType == typeof(string) => Guard.IsInstanceOf<T>(value).ToString(string.Empty, culture),
        _ when IsConvertable(destinationType) => ConvertToConvertable(value, destinationType),
        _ => base.ConvertTo(context, culture, value, destinationType),
    };

    [Pure]
    private static object? ConverToDefault(Type destinationType)
        => QowaivType.IsNullable(destinationType)
        ? null
        : Activator.CreateInstance(destinationType);

    [Pure]
    private object ConvertToConvertable(object? value, Type destinationType)
    {
        var date = Guard.IsInstanceOf<T>(value);
        var type = QowaivType.GetNotNullableType(destinationType);
        return ConvertTos[type](this, date);
    }

    /// <summary>Converts from <see cref="string" />.</summary>
    [Pure]
    protected abstract T FromString(string? str, CultureInfo? culture);

    /// <summary>Converts from <see cref="DateTime" />.</summary>
    [Pure]
    protected abstract T FromDateTime(DateTime dateTime);

    /// <summary>Converts from <see cref="DateTimeOffset" />.</summary>
    [Pure]
    protected abstract T FromDateTimeOffset(DateTimeOffset offset);

    /// <summary>Converts from <see cref="LocalDateTime" />.</summary>
    [Pure]
    protected abstract T FromLocalDateTime(LocalDateTime local);

    /// <summary>Converts from <see cref="Date" />.</summary>
    [Pure]
    protected abstract T FromDate(Date date);

    /// <summary>Converts from <see cref="WeekDate" />.</summary>
    [Pure]
    protected abstract T FromWeekDate(WeekDate weekDate);

    /// <summary>Converts from <see cref="YearMonth" />.</summary>
    [Pure]
    protected abstract T FromYearMonth(YearMonth yearMonth);

    /// <summary>Converts to <see cref="DateTime" />.</summary>
    [Pure]
    protected abstract DateTime ToDateTime(T date);

    /// <summary>Converts to <see cref="DateTimeOffset" />.</summary>
    [Pure]
    protected abstract DateTimeOffset ToDateTimeOffset(T date);

    /// <summary>Converts to <see cref="LocalDateTime" />.</summary>
    [Pure]
    protected abstract LocalDateTime ToLocalDateTime(T date);

    /// <summary>Converts to <see cref="Date" />.</summary>
    [Pure]
    protected abstract Date ToDate(T date);

    /// <summary>Converts to <see cref="WeekDate" />.</summary>
    [Pure]
    protected abstract WeekDate ToWeekDate(T date);

    /// <summary>Converts to <see cref="YearMonth" />.</summary>
    [Pure]
    protected abstract YearMonth ToYearMonth(T date);

    [Pure]
    private static bool IsConvertable(Type type)
        => type != typeof(T)
        && (type == typeof(string) || QowaivType.IsDate(type));

    private static readonly Dictionary<Type, Func<DateTypeConverter<T>, T, object>> ConvertTos = new()
    {
        [typeof(DateTime)] /*.......*/ = (c, d) => c.ToDateTime(d),
#if NET6_0_OR_GREATER
        [typeof(DateOnly)] /*.......*/ = (c, d) => (DateOnly)c.ToDate(d),
#endif
        [typeof(DateTimeOffset)] /*.*/ = (c, d) => c.ToDateTimeOffset(d),
        [typeof(LocalDateTime)] /*..*/ = (c, d) => c.ToLocalDateTime(d),
        [typeof(Date)] /*...........*/ = (c, d) => c.ToDate(d),
        [typeof(WeekDate)] /*.......*/ = (c, d) => c.ToWeekDate(d),
        [typeof(YearMonth)] /*......*/ = (c, d) => c.ToYearMonth(d),
    };
}
