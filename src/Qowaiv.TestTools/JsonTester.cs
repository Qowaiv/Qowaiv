using System.Reflection;
#if NET6_0_OR_GREATER
using System.Text.Json;
#endif
namespace Qowaiv.TestTools;

/// <summary>Helper class for testing JSON conversion.</summary>
public static class JsonTester
{
#if NET6_0_OR_GREATER
    /// <summary>Reads the JSON using System.Text.Json.</summary>
    [Pure]
    public static T? Read_System_Text_JSON<T>(object? val)
    {
        return JsonSerializer.Deserialize<T>(ToString(val));

        static string ToString(object? val)
            => val switch
            {
                string str => @$"""{str}""",
                bool boolean => boolean ? "true" : "false",
                IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                null => "null",
                _ => val?.ToString() ?? string.Empty,
            };

    }
    /// <summary>Writes the JSON using System.Text.Json.</summary>
    /// <remarks>
    /// <see cref="JsonSerializer.SerializeToElement(object?, Type, JsonSerializerOptions?)"/> is only available in .NET 6.0.
    /// </remarks>
    [Pure]
    public static object? Write_System_Text_JSON(object? svo)
    {
        var json = JsonSerializer.SerializeToElement(svo);

        if (json.ValueKind == JsonValueKind.String) return json.GetString();
        else if (json.ValueKind == JsonValueKind.Number)
        {
            if (json.TryGetInt32(out var int32)) return int32;
            else if (json.TryGetInt64(out var int64)) return int64;
            else if (json.TryGetDouble(out var number)) return number;
            else return null;
        }
        else return null;
    }

#endif

    /// <summary>Applies multiple FromJson scenario's.</summary>
    [Pure]
    public static T? Read<T>(object? val)
    {
        var parameterType = val?.GetType();
        var fromJson = typeof(T)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .FirstOrDefault(m => FromJson<T>(m, parameterType));

        if (fromJson is null)
        {
            throw new InvalidOperationException($"Could not find {typeof(T).Name}.FromJson({parameterType?.Name ?? "null"}).");
        }
        try
        {
            return (T?)fromJson.Invoke(null, new[] { val });
        }
        catch (TargetInvocationException x)
        {
            if (x.InnerException is null) throw;
            else throw x.InnerException;
        }
    }

    /// <summary>Applies <code>ToJson()</code>.</summary>
    [Pure]
    public static object? Write<T>(T val)
    {
        var toJson = typeof(T)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .FirstOrDefault(ToJson);

        return toJson is null
            ? throw new InvalidOperationException($"Could not find {typeof(T).Name}.ToJson().")
            : toJson.Invoke(val, Array.Empty<object>());
    }

    [Pure]
    private static bool FromJson<T>(MethodInfo method, Type? parameterType)
        => method.Name == nameof(FromJson)
        && method.GetParameters().Length == 1
        && method.GetParameters()[0].ParameterType == parameterType
        && method.ReturnType == typeof(T);

    [Pure]
    private static bool ToJson(MethodInfo method)
        => method.Name == nameof(ToJson)
        && method.GetParameters().Length == 0
        && method.ReturnType != null;
}
