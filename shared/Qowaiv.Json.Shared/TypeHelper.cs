namespace Qowaiv.Internals;

/// <summary>Internal helper class for analyzing <see cref="Type"/>s.</summary>
internal static class TypeHelper
{
    extension(Type type)
    {
        /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
        [Pure]
        public Type NotNullable
            => Nullable.GetUnderlyingType(type)
            ?? type;
    }

    /// <summary>Gets the candidate types.</summary>
    /// <param name="types">
    /// The types to select from.
    /// </param>
    [Pure]
    public static IEnumerable<Type> GetCandidateTypes(IEnumerable<Type> types)
        => types.Select(GetCandidateType).OfType<Type>();

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type? GetCandidateType(Type? type) => type is
    {
        NotNullable:
        {
            IsAbstract: false,
            IsGenericTypeDefinition: false,
            ContainsGenericParameters: false,
            IsGenericTypeDefinition: false,
        } tp
    }
    ? tp : null;
}
