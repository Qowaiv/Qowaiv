#if NET10_0_OR_GREATER

using System.Text.Json.Serialization.Metadata;
using static System.Linq.Expressions.Expression;

namespace Qowaiv.Json;

/// <summary>Helper to modify <see cref="JsonTypeInfo"/>'s.</summary>
public static class ModifyTypeInfo
{
    /// <summary>
    /// Updates all properties that implement <see cref="IEmpty" /> not
    /// to serialize when <see cref="IEmpty.HasValue"/> is false.
    /// </summary>
    public static void IgnoreEmptySvos(JsonTypeInfo info)
    {
        Guard.NotNull(info);

        foreach (var prop in info.Properties.Where(IsApplicable))
        {
            prop.ShouldSerialize = ShouldSerialize;
        }
    }

    [Pure]
    private static bool ShouldSerialize(object _, object? prop) => ((IEmpty)prop!).HasValue;

    /// <summary>Is applicable for structs implementing <see cref="IEmpty{TSelf}"/>.</summary>
    [Pure]
    private static bool IsApplicable(JsonPropertyInfo prop)
        => prop.PropertyType is { IsValueType: true, IsGenericType: false }
        && prop.PropertyType.GetInterfaces().Contains(typeof(IEmpty));
}

#endif
