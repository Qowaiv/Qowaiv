using System.Reflection;

namespace Qowaiv.OpenApi;

/// <summary>Creates <see cref="OpenApiDataType" />s.</summary>
public static class OpenApiDataTypes
{
    /// <summary>Gets all <see cref="OpenApiDataTypeAttribute" />s specified in the assemblies.</summary>
    [Pure]
    [RequiresUnreferencedCode("Types might be removed")]
    [RequiresDynamicCode("Calls System.Type.MakeGenericType(params Type[])")]
    public static IEnumerable<OpenApiDataType> FromAssemblies(params Assembly[] assemblies)
        => FromTypes(Guard.NotNull(assemblies)
        .SelectMany(assembly => assembly.GetExportedTypes()));

    /// <summary>Gets all <see cref="OpenApiDataTypeAttribute" />s of the
    /// specified types that are decorated as such.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    [RequiresDynamicCode("Calls System.Type.MakeGenericType(params Type[])")]
    [RequiresUnreferencedCode("Calls System.Type.MakeGenericType(params Type[])")]
    public static IEnumerable<OpenApiDataType> FromTypes(params Type[] types)
        => FromTypes(types?.AsEnumerable() ?? []);

    /// <summary>Gets all <see cref="OpenApiDataTypeAttribute" />s of the
    /// specified types that are decorated as such.
    /// </summary>
    [Pure]
    [RequiresDynamicCode("Calls System.Type.MakeGenericType(params Type[])")]
    [RequiresUnreferencedCode("Calls System.Type.MakeGenericType(params Type[])")]
    public static IEnumerable<OpenApiDataType> FromTypes(IEnumerable<Type> types)
        => Guard.NotNull(types)
        .Select(OpenApiDataType.FromType)
        .Where(data => data is { })!;
}
