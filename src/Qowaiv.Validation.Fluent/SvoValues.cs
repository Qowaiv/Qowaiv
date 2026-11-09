using System.Collections.Concurrent;

namespace Qowaiv.Validation.Fluent;

internal static class SvoValues
{
    [Pure]
    public static object? Unknown(Type type) => Unknowns.GetOrAdd(type, t => Qowaiv.Unknown.Value(t));

    private static readonly ConcurrentDictionary<Type, object?> Unknowns = new();
}
