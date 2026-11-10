using Qowaiv.Validation.DataAnnotations.Reflection;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the size of the collection.</summary>
public static partial class Collection
{
    [Pure]
    private static long? GetCount<TAttribute>(object? value) where TAttribute : ValidationAttribute => value switch
    {
        null => null,
        ICollection collection => collection.Count,
        _ when value.TryCount() is { } count => count,
        _ => throw UnsupportedType.ForAttribute<TAttribute>(value.GetType()),
    };
}
