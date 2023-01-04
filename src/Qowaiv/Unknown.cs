using System.Reflection;

namespace Qowaiv;

/// <summary>Helps handling the unknown status of Single Value Objects.</summary>
/// <remarks>
/// The 'unknown' case differences from the 'empty' case. Where 'empty' just means:
/// Not set (yet), this is an unset (default) value, 'unknown' means that the user
/// has set the value, saying, there must be some value, but I just don't know
/// which value it should be.
/// 
/// Note that not all scenario's that support 'empty' support 'unknown' too.
/// </remarks>
public static class Unknown
{
    /// <summary>Returns true if the string represents unknown, otherwise false.</summary>
    /// <param name="val">
    /// The string value to test.
    /// </param>
    [Pure]
    public static bool IsUnknown(string val) => IsUnknown(val, null);

    /// <summary>Returns true if the string represents unknown, otherwise false.</summary>
    /// <param name="val">
    /// The string value to test.
    /// </param>
    /// <param name="culture">
    /// The culture to test for.
    /// </param>
    [Pure]
    public static bool IsUnknown(string? val, CultureInfo? culture)
    {
        if (val is { Length: > 0 })
        {
            var c = culture ?? CultureInfo.CurrentCulture;
            if (!Strings.TryGetValue(c, out var values))
            {
                lock (addCulture)
                {
                    if (!Strings.TryGetValue(c, out values))
                    {
                        values = ResourceManager
                            .GetString("Values", c)!
                            .Split(';')
                            .Select(v => v.ToUpper(c))
                            .ToArray();
                        Strings[c] = values;
                    }
                }
            }
            return values.Contains(val.ToUpper(c)) ||
            (
                !c.Equals(CultureInfo.InvariantCulture) &&
                Strings[CultureInfo.InvariantCulture].Contains(val.ToUpperInvariant())
            );
        }
        else return false;
    }

    /// <summary>Gets the value that represents set but unknown.</summary>
    /// <param name="type">
    /// The type that should could have an unknown value.
    /// </param>
    /// <returns>
    /// null, if not defined, otherwise the unknown value for a type.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// if type is not a value type.
    /// </exception>
    /// <remarks>
    /// The unknown value is expected to be static field or property of the type with the name "Unknown".
    /// </remarks>
    [Pure]
    public static object? Value(Type type)
    {
        if (type is not null)
        {
            var field = type.GetField(nameof(Unknown), BindingFlags.Public | BindingFlags.Static);
            if (field is not null)
            {
                return field.FieldType == type ? field.GetValue(null) : null;
            }
            else
            {
                var property = type.GetProperty(nameof(Unknown), BindingFlags.Public | BindingFlags.Static);
                return property?.PropertyType == type ? property.GetValue(null) : null;
            }
        }
        else return null;
    }

    /// <summary>The resource manager managing the culture based string values.</summary>
    private static readonly ResourceManager ResourceManager = new("Qowaiv.UnknownLabels", typeof(Unknown).Assembly);
    private readonly static Dictionary<CultureInfo, string[]> Strings = new()
    {
        { CultureInfo.InvariantCulture, new[] { "?", "UNKNOWN", "NOT KNOWN", "NOTKNOWN" } },
    };
    private static readonly object addCulture = new();
}
