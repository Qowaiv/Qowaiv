using System.Reflection;

internal static class GenericSvoJsonConverterHelpers
{

    [Pure]
    private static object FromJson(string? str, Type type)
    {
        if (!parsers.TryGetValue(type, out var parser))
        {
            parser = type.GetMethod(nameof(FromJson), BindingFlags.Public | BindingFlags.Static)!;
            parsers[type] = parser;
        }
        return parser.Invoke(null, new object?[] { str })!;
    }
}