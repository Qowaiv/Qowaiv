namespace Qowaiv.Internals;

/// <summary>Internal helper class for analyzing <see cref="Type"/>s.</summary>
internal static class TypeHelper
{
    /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    [Pure]
    public static Type? NotNullable(Type? objectType)
        => objectType is { } ? Nullable.GetUnderlyingType(objectType) ?? objectType : null;

    /// <summary>Gets the candidate types.</summary>
    /// <param name="types">
    /// The types to select from.
    /// </param>
    [Pure]
    public static IEnumerable<Type> GetCandidateTypes(IEnumerable<Type> types)
        => types.Select(GetCandidateType).OfType<Type>();

    [Pure]
    public static Type? GetCandidateType(Type? type)
    {
        return NotNullable(type) is { } tp && IsSupported(tp)
            ? tp
            : null;

        static bool IsSupported(Type type)
            => !type.IsAbstract
            && !type.IsGenericTypeDefinition
            && !type.ContainsGenericParameters
            && !type.IsGenericTypeDefinition;
    }
}
