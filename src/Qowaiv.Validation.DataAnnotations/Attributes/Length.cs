using Qowaiv.Validation.DataAnnotations.Reflection;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the length of a value.</summary>
public static partial class Length
{
    [Pure]
    private static long? GetLength<TAttribute>(object? value) where TAttribute : ValidationAttribute => value switch
    {
        null => null,
        string str => str.Length,
        Array array => array.LongLength,
        _ when value.TryLength() is { } length => length,
        _ => throw UnsupportedType.ForAttribute<TAttribute>(value.GetType()),
    };
}
