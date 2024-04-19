using System.Reflection;

namespace Qowaiv.Data;

/// <summary>Factory class for creating database parameters.</summary>
public static class SvoParameter
{
    /// <summary>Creates a <see cref="SqlParameter"/> based on the single value object.</summary>
    /// <param name="parameterName">
    /// The name of the parameter to map.
    /// </param>
    /// <param name="value">
    /// An <see cref="object"/>that is the value of the <see cref="SqlParameter"/>.
    /// </param>
    /// <returns>
    /// A <see cref="SqlParameter"/> with a converted value if the value is a
    /// single value object, otherwise with a non-converted value.
    /// </returns>
    [Pure]
    public static SqlParameter CreateForSql(string parameterName, object? value)
    {
        // If null, return DBNull.
        if (value is null) return new SqlParameter(parameterName, DBNull.Value);
        else
        {
            var sourceType = value.GetType();

            lock (locker)
            {
                if (!Attributes.TryGetValue(sourceType, out var attr))
                {
                    attr = sourceType.GetCustomAttribute<SingleValueObjectAttribute>();
                }

                // No attribute, so not supported.
                if (attr is null)
                {
                    Attributes[sourceType] = null;
                    return new SqlParameter(parameterName, value);
                }
                else if (IsDbNullValue(value, sourceType, attr))
                {
                    return new SqlParameter(parameterName, DBNull.Value);
                }
                else
                {
                    MethodInfo cast = GetCast(sourceType, attr);
                    var casted = cast.Invoke(null, [value]);
                    return new SqlParameter(parameterName, casted);
                }
            }
        }
    }

    /// <summary>Returns true if the value should be represented by a <see cref="DBNull.Value"/>, otherwise false.</summary>
    [Pure]
    private static bool IsDbNullValue(object value, Type sourceType, SingleValueObjectAttribute attr)
    {
        if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue))
        {
            var defaultValue = Activator.CreateInstance(sourceType);
            return Equals(value, defaultValue);
        }
        else return false;
    }

    /// <summary>Gets the cast needed for casting to the database type.</summary>
    /// <exception cref="InvalidCastException">
    /// If the required cast is not defined.
    /// </exception>
    [Pure]
    private static MethodInfo GetCast(Type sourceType, SingleValueObjectAttribute attr)
    {
        if (!Casts.TryGetValue(sourceType, out var cast))
        {
            var returnType = attr.DatabaseType ?? attr.UnderlyingType;
            cast = sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Find(m =>
                    m.IsHideBySig &&
                    m.IsSpecialName &&
                    m.GetParameters().Length == 1 &&
                    m.ReturnType == returnType)
                ?? throw new InvalidCastException(string.Format(QowaivMessages.InvalidCastException_FromTo, sourceType, returnType));
            Casts[sourceType] = cast;
        }
        return cast;
    }

    private static readonly Dictionary<Type, SingleValueObjectAttribute?> Attributes = [];
    private static readonly Dictionary<Type, MethodInfo> Casts = [];

    /// <summary>The locker for adding a casts and unsupported types.</summary>
    private static readonly object locker = new();
}
