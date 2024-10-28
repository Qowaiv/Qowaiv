#if NET8_0_OR_GREATER

using System.Text.Json.Serialization.Metadata;

namespace Qowaiv.Json;

/// <summary>Helper to modify <see cref="JsonTypeInfo"/>'s.</summary>
public static class ModifyTypeInfo
{
    /// <summary>
    /// Updates all properties that implement <see cref="IEmpty{TSelf}"/> not
    /// to serialize when <see cref="IEmpty{TSelf}.HasValue"/> is false.
    /// </summary>
    public static void IgnoreEmptySvos(JsonTypeInfo info)
    {
        Guard.NotNull(info);

        foreach (var prop in info.Properties.Where(IsApplicable))
        {
            prop.ShouldSerialize = ShouldSerialize(Activator.CreateInstance(prop.PropertyType)!);
        }
    }

    /// <summary>True when the propValue is not equal to the empty/default value.</summary>
    [Pure]
    private static Func<object, object?, bool> ShouldSerialize(object emptyValue) =>
        (_, propValue) => !emptyValue.Equals(propValue);

    /// <summary>Is applicable for structs implementing <see cref="IEmpty{TSelf}"/>.</summary>
    [Pure]
    private static bool IsApplicable(JsonPropertyInfo prop)
        => prop.PropertyType is { IsValueType: true, IsGenericType: false }
        && prop.PropertyType
            .GetInterfaces()
            .Exists(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEmpty<>));
}

#endif
