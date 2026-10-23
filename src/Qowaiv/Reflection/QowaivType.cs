namespace Qowaiv.Reflection;

/// <summary>Helper class for some operations on <see cref="Type" />.</summary>
public static partial class QowaivType
{
    extension(Type type)
    {
        /// <summary>Returns true if the object is null-able, otherwise false.</summary>
        [Pure]
        public bool IsNullable => Nullable.GetUnderlyingType(type) is not null;

        /// <summary>Returns true if the object type is a number.</summary>
        [Pure]
        public bool IsNumeric
        {
            get
            {
                var code = Type.GetTypeCode(GetNotNullableType(type));
                return code is >= TypeCode.SByte and <= TypeCode.Decimal;
            }
        }

#if NET10_0_OR_GREATER
        /// <summary>Returns true if the object type is a date (of any kind).</summary>
        /// <remarks>
        /// Tests on the types:
        /// * <see cref="DateTime" />
        /// * <see cref="DateOnly" />
        /// * <see cref="DateTimeOffset" />
        /// * <see cref="LocalDateTime" />
        /// * <see cref="Date" />
        /// * <see cref="Date" />
        /// * <see cref="WeekDate" />.
        /// * <see cref="YearMonth" />.
        /// </remarks>
#else
        /// <summary>Returns true if the object type is a date (of any kind).</summary>
        /// <remarks>
        /// Tests on the types:
        /// * <see cref="DateTime" />
        /// * <see cref="DateTimeOffset" />
        /// * <see cref="LocalDateTime" />
        /// * <see cref="Date" />
        /// * <see cref="Date" />
        /// * <see cref="WeekDate" />.
        /// * <see cref="YearMonth" />.
        /// </remarks>
#endif
        [Pure]
        public bool IsDate => DateTypes.Contains(GetNotNullableType(type));
    }

    extension([NotNullIfNotNull(nameof(type))] Type? type)
    {
        [Pure]
        internal bool IsAnyOf(params Type[] types) => type is { } && types.Contains(type);
    }

    /// <summary>Returns true if the value is null or equal to the default value.</summary>
    [Pure]
    public static bool IsNullOrDefaultValue(object? value)
        => value is null || value.Equals(Activator.CreateInstance(value.GetType()));

    /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    [Pure]
    public static Type GetNotNullableType(Type objectType)
        => Nullable.GetUnderlyingType(objectType) ?? objectType;

    private static readonly Type[] DateTypes =
    [
       typeof(DateTime),
#if NET10_0_OR_GREATER
        typeof(DateOnly),
#endif
        typeof(DateTimeOffset),
        typeof(LocalDateTime),
        typeof(Date),
        typeof(WeekDate),
        typeof(YearMonth)
    ];
}
