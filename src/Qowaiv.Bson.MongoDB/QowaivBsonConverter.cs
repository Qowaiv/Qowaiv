using System.Reflection;

namespace Qowaiv.Bson.MongoDB;

/// <summary>Static helper class to register <see cref="QowaivBsonConverter{TSvo}"/> for specific types.</summary>
[Obsolete("Use QowaivBsonSerializationProvider instead.")]
public static class QowaivBsonConverter
{
    /// <summary>Registers all supported types of the assembly.</summary>
    /// <param name="assembly">
    /// The assembly to register its supported types for.
    /// </param>
    public static void RegisterAssembly(Assembly assembly)
    {
        foreach (var type in TypeHelper.GetCandidateTypes(Guard.NotNull(assembly, nameof(assembly)).GetExportedTypes()))
        {
            if (CreateConverter(type) is var converter && TypeIsSupported(converter))
            {
                BsonSerializer.RegisterSerializer(type, converter);
            }
        }
    }

    /// <summary>Registers all types.</summary>
    /// <param name="types">
    /// The types to register a <see cref="QowaivBsonConverter{TSvo}"/> for.
    /// </param>
    public static void RegisterTypes(params Type[] types)
    {
        foreach (var type in Guard.HasAny(types, nameof(types)))
        {
            RegisterType(type);
        }
    }

    /// <summary>Registers a type.</summary>
    /// <param name="type">
    /// The type to register a <see cref="QowaivBsonConverter{TSvo}"/> for.
    /// </param>
    public static void RegisterType(Type type)
    {
        Guard.NotNull(type, nameof(type));
        var converter = CreateConverter(type);
        BsonSerializer.RegisterSerializer(type, GuardType(converter));
    }

    /// <summary>Registers a type.</summary>
    /// <typeparam name="TSvo">
    /// The type to register a <see cref="QowaivBsonConverter{TSvo}"/> for.
    /// </typeparam>
    public static void RegisterType<TSvo>()
    {
        var converter = new QowaivBsonConverter<TSvo>();
        GuardType(converter);
        BsonSerializer.RegisterSerializer(converter);
    }

    /// <summary>Guard that the converter actually supports conversion based on conventions.</summary>
    [Impure]
    private static IBsonSerializer GuardType(IBsonSerializer converter)
        => TypeIsSupported(converter) ? converter : throw new NotSupportedException();

    /// <summary>Returns true if a name based convention is supported.</summary>
    [Pure]
    private static bool TypeIsSupported(IBsonSerializer converter)
        => (bool)converter
        .GetType()
        .GetProperty(nameof(QowaivBsonConverter<object>.TypeIsSupported), NonPublicInstance)!
        .GetValue(converter, Array.Empty<object>())!;

    /// <summary>Creates an instance of <see cref="QowaivBsonConverter{TSvo}"/> based on the specified type.</summary>
    [Pure]
    private static IBsonSerializer CreateConverter(Type type)
    {
        var converterType = typeof(QowaivBsonConverter<>).MakeGenericType(type);
        return (IBsonSerializer)Activator.CreateInstance(converterType)!;
    }

    private const BindingFlags NonPublicInstance = (BindingFlags)36;
}
