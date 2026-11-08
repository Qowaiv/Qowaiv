using System.Collections.Concurrent;
using System.Reflection;

namespace Qowaiv.Bson.MongoDB;

/// <summary>
/// Implements <see cref="IBsonSerializationProvider"/> using <see cref="QowaivBsonSerializer{TSvo}"/>.
/// </summary>
public sealed class QowaivBsonSerializationProvider : IBsonSerializationProvider
{
    private readonly ConcurrentDictionary<Type, IBsonSerializer?> Serializers = new();

    /// <inheritdoc />
    [Pure]
    public IBsonSerializer? GetSerializer(Type? type)
    {
        if (GetCandidateType(type) is not { } tp) return null;

        var serializer = Serializers.TryGetValue(tp, out var existing)
            ? existing
            : CreateSerializer(tp);

        return serializer is { } && !tp.Equals(type)
            ? AsNulllableSerializer(serializer)
            : serializer;
    }

    /// <summary>Creates an instance of <see cref="QowaivBsonSerializer{TSvo}"/> based on the specified type.</summary>
    [Pure]
    private IBsonSerializer? CreateSerializer(Type type)
    {
        var converterType = typeof(QowaivBsonSerializer<>).MakeGenericType(type);
        var serializer = (IBsonSerializer)Activator.CreateInstance(converterType)!;
        serializer = IsSupported(serializer) ? serializer : null;
        Serializers[type] = serializer;
        return serializer;
    }

    [Pure]
    private static IBsonSerializer AsNulllableSerializer(IBsonSerializer serializer)
    {
        var nullable = typeof(NullableSerializer<>).MakeGenericType(serializer.ValueType);
        return (IBsonSerializer)Activator.CreateInstance(nullable, serializer)!;
    }

    /// <summary>Returns true if a name based convention is supported.</summary>
    [Pure]
    private static bool IsSupported(IBsonSerializer converter)
        => (bool)converter
        .GetType()
        .GetProperty(nameof(QowaivBsonSerializer<object>.TypeIsSupported), NonPublicInstance)!
        .GetValue(converter, Array.Empty<object>())!;

    [Pure]
    private static Type? GetCandidateType(Type? type)
    {
        return TypeHelper.NotNullable(type) is { } tp && IsSupported(tp)
            ? tp
            : null;

        static bool IsSupported(Type type)
            => !type.IsAbstract
            && !type.IsGenericTypeDefinition
            && !type.ContainsGenericParameters
            && !type.IsGenericTypeDefinition;
    }

#pragma warning disable S3011

    // Reflection should not be used to increase accessibility of classes, methods, or fields
    // We do not want to publicly expose this, but are certain it will always exist.
    private const BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;
#pragma warning restore S3011
}
