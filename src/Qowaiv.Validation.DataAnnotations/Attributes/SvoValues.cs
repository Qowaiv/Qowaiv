namespace Qowaiv.Validation.DataAnnotations;

internal static class SvoValues
{
    [Pure]
    public static object? Default(Type type) => Defaults.GetOrAdd(type, Activator.CreateInstance);

    [Pure]
    public static object? Unknown(Type type) => Unknowns.GetOrAdd(type, t => Qowaiv.Unknown.Value(t));

    private static readonly ConcurrentDictionary<Type, object?> Defaults = new();

    private static readonly ConcurrentDictionary<Type, object?> Unknowns = new();
}
