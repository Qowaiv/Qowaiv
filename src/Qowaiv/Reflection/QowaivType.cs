namespace Qowaiv.Reflection;

/// <summary>Helper class for some operations on <see cref="Type"/>.</summary>
public static class QowaivType
{
    /// <summary>Returns true if the value is null or equal to the default value.</summary>
    [Pure]
    public static bool IsNullOrDefaultValue(object? value)
        => value is null || value.Equals(Activator.CreateInstance(value.GetType()));

    /// <summary>Returns true if the object is null-able, otherwise false.</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    [Pure]
    public static bool IsNullable(Type objectType) => Nullable.GetUnderlyingType(objectType) is not null;

    /// <summary>Returns true if the object type is a number.</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    [Pure]
    public static bool IsNumeric(Type objectType)
    {
        var code = Type.GetTypeCode(GetNotNullableType(objectType));
        return code >= TypeCode.SByte && code <= TypeCode.Decimal;
    }

#if NET6_0_OR_GREATER
    /// <summary>Returns true if the object type is a date (of any kind).</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    /// <remarks>
    /// Tests on the types:
    /// * <see cref="DateTime"/>
    /// * <see cref="DateOnly"/>
    /// * <see cref="DateTimeOffset"/>
    /// * <see cref="LocalDateTime"/>
    /// * <see cref="Date"/>
    /// * <see cref="Date"/>
    /// * <see cref="WeekDate"/>.
    /// * <see cref="YearMonth"/>.
    /// </remarks>
#else
    /// <summary>Returns true if the object type is a date (of any kind).</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    /// <remarks>
    /// Tests on the types:
    /// * <see cref="DateTime"/>
    /// * <see cref="DateTimeOffset"/>
    /// * <see cref="LocalDateTime"/>
    /// * <see cref="Date"/>
    /// * <see cref="Date"/>
    /// * <see cref="WeekDate"/>.
    /// * <see cref="YearMonth"/>.
    /// </remarks>
#endif
    [Pure]
    public static bool IsDate(Type? objectType)
    {
        var type = objectType is null ? null : GetNotNullableType(objectType);
        return DateTypes.Contains(type);
    }

    private static readonly Type[] DateTypes =
    [
        typeof(DateTime),
#if NET6_0_OR_GREATER
        typeof(DateOnly),
#endif
        typeof(DateTimeOffset),
        typeof(LocalDateTime),
        typeof(Date),
        typeof(WeekDate),
        typeof(YearMonth)
    ];

    /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    [Pure]
    public static Type GetNotNullableType(Type objectType)
        => Nullable.GetUnderlyingType(objectType) ?? objectType;
}
