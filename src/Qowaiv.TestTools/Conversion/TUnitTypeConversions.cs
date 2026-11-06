using Qowaiv.Reflection;
using System.Reflection;

namespace Qowaiv.TestTools.Conversion;

/// <summary>Helper to resolve TUNit type conversions.</summary>
public static class TUnitTypeConversions
{
    /// <summary>Gets the TUnit.Core.ArgumentsAttribute type.</summary>
    /// <remarks>
    /// Is null if TUnit could not be loaded.
    /// </remarks>
    public static Type? ArgumentsAttributeType => lazyArgumentsAttributeType.Value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly Lazy<Type?> lazyArgumentsAttributeType = new(() => Type.GetType("TUnit.Core.ArgumentsAttribute, TUnit.Core"));

    /// <summary>Scans an assembly for TUnit type conversions.</summary>
    /// <param name="assembly">
    /// The assembly to scan for.
    /// </param>
    /// <remarks>
    /// Only scans public non-static methods on public  non-static classes.
    /// </remarks>
    [Pure]
    public static ISet<TypeConversion> Scan(Assembly assembly) => new HashSet<TypeConversion>(Guard.NotNull(assembly)
        .GetExportedTypes()
        .Where(IsSupported)
        .SelectMany(TestMethods)
        .SelectMany(TypeCoversions));

    [Pure]
    private static IEnumerable<TypeConversion> TypeCoversions(MethodInfo method)
    {
        if (ArgumentsAttributeType is null) yield break;

        foreach (var values in method.GetCustomAttributes(ArgumentsAttributeType).Select(Values).OfType<object?[]>())
        {
            if (ParameterTypes(method) is not { } parameters || parameters.Length != values.Length) continue;

            for (var i = 0; i < values.Length; i++)
            {
                if (values[i]?.GetType() is { } source &&
                    new TypeConversion(source, parameters[i]) is { IsUpcast: false } conversion)
                    yield return conversion;
            }
        }
    }

    [Pure]
    private static IEnumerable<MethodInfo> TestMethods(Type type) => type.GetMethods().Where(IsSupported);

    /// <summary>Public non-static methods only.</summary>
    [Pure]
    private static bool IsSupported(MethodInfo method) => method is { IsPublic: true, IsStatic: false };

    /// <summary>Public non-static, non-abstract classes only.</summary>
    [Pure]
    private static bool IsSupported(Type type) => type is { IsClass: true, IsPublic: true, IsAbstract: false };

    [Pure]
    private static object?[]? Values(Attribute attr) => attr
        .GetType()
        .GetProperty(nameof(Values))?
        .GetValue(attr) as object?[];

    [Pure]
    private static Type[] ParameterTypes(MethodInfo method) =>
    [
        ..method.GetParameters().Select(p => p.ParameterType.GetNotNullableType())
    ];
}
