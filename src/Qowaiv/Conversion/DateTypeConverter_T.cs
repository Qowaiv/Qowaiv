using Qowaiv.Reflection;

namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a Date types.</summary>
public abstract class DateTypeConverter<T> : TypeConverter where T : struct, IFormattable
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => IsConvertable(sourceType) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is null) return Activator.CreateInstance<T>();
        else if (value is string str) return FromString(str, culture);
        else if (IsConvertable(value.GetType()))
        {
            if (value is DateTime dateTime) return FromDateTime(dateTime);
            else if (value is DateTimeOffset offset) return FromDateTimeOffset(offset);
            else if (value is LocalDateTime local) return FromLocalDateTime(local);
            else if (value is Date date) return FromDate(date);
            else if (value is WeekDate weekDate) return FromWeekDate(weekDate);
        }
        return base.ConvertFrom(context, culture, value);
    }

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => (destinationType != typeof(T) && QowaivType.IsDate(destinationType))
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        Guard.NotNull(destinationType);

        // If the value is null or default value.
        if (QowaivType.IsNullOrDefaultValue(value))
        {
            return QowaivType.IsNullable(destinationType) ? null : Activator.CreateInstance(destinationType);
        }
        else if (destinationType == typeof(string))
        {
            var typed = Guard.IsInstanceOf<T>(value, nameof(value));
            return typed.ToString(string.Empty, culture);
        }
        else if (IsConvertable(destinationType))
        {
            var date = Guard.IsInstanceOf<T>(value, nameof(value));
            var type = QowaivType.GetNotNullableType(destinationType);

            if (type == typeof(DateTime)) return ToDateTime(date);
            else if (type == typeof(DateTimeOffset)) return ToDateTimeOffset(date);
            else if (type == typeof(LocalDateTime)) return ToLocalDateTime(date);
            else if (type == typeof(Date)) return ToDate(date);
            else if (type == typeof(WeekDate)) return ToWeekDate(date);
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }

    /// <summary>Converts from <see cref="string"/>.</summary>
    [Pure]
    protected abstract T FromString(string? str, CultureInfo? culture);

    /// <summary>Converts from <see cref="DateTime"/>.</summary>
    [Pure]
    protected abstract T FromDateTime(DateTime dateTime);

    /// <summary>Converts from <see cref="DateTimeOffset"/>.</summary>
    [Pure]
    protected abstract T FromDateTimeOffset(DateTimeOffset offset);

    /// <summary>Converts from <see cref="LocalDateTime"/>.</summary>
    [Pure]
    protected abstract T FromLocalDateTime(LocalDateTime local);

    /// <summary>Converts from <see cref="Date"/>.</summary>
    [Pure]
    protected abstract T FromDate(Date date);

    /// <summary>Converts from <see cref="WeekDate"/>.</summary>
    [Pure]
    protected abstract T FromWeekDate(WeekDate weekDate);

    /// <summary>Converts to <see cref="DateTime"/>.</summary>
    [Pure]
    protected abstract DateTime ToDateTime(T date);

    /// <summary>Converts to <see cref="DateTimeOffset"/>.</summary>
    [Pure]
    protected abstract DateTimeOffset ToDateTimeOffset(T date);

    /// <summary>Converts to <see cref="LocalDateTime"/>.</summary>
    [Pure]
    protected abstract LocalDateTime ToLocalDateTime(T date);

    /// <summary>Converts to <see cref="Date"/>.</summary>
    [Pure]
    protected abstract Date ToDate(T date);

    /// <summary>Converts to <see cref="WeekDate"/>.</summary>
    [Pure]
    protected abstract WeekDate ToWeekDate(T date);

    [Pure]
    private static bool IsConvertable(Type type)
        => type != typeof(T)
        && (type == typeof(string) || QowaivType.IsDate(type));
}
