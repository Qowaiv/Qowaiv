using System.Collections.Concurrent;
using System.Reflection;

namespace Qowaiv;

/// <summary>Helps handling the unknown status of Single Value Objects.</summary>
/// <remarks>
/// The 'unknown' case differences from the 'empty' case. Where 'empty' just means:
/// Not set (yet), this is an unset (default) value, 'unknown' means that the user
/// has set the value, saying, there must be some value, but I just don't know
/// which value it should be.
///
/// Note that not all scenarios that support 'empty' support 'unknown' too.
/// </remarks>
public static class Unknown
{
    /// <summary>Returns true if the string represents unknown, otherwise false.</summary>
    /// <param name="val">
    /// The string value to test.
    /// </param>
    [Pure]
    public static bool IsUnknown(string? val) => IsUnknown(val, null);

    /// <summary>Returns true if the string represents unknown, otherwise false.</summary>
    /// <param name="val">
    /// The string value to test.
    /// </param>
    /// <param name="culture">
    /// The culture to test for.
    /// </param>
    [Pure]
    public static bool IsUnknown(string? val, CultureInfo? culture)
        => val is { Length: > 0 }
        && ParseValues.Contains(culture ?? CultureInfo.CurrentCulture, val);

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
    public static object? Value(Type? type)
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

    private static readonly UnknownValues ParseValues = new();

#pragma warning disable S1121 // improves the readabillity in this case.
    private sealed class UnknownValues : ConcurrentDictionary<CultureInfo, FrozenSet<string>>
    {
        private readonly FrozenSet<string> Invariant = new HashSet<string>(["?", "UNKNOWN", "NOT KNOWN", "NOTKNOWN"]).ToFrozenSet(StringComparer.OrdinalIgnoreCase);

        /// <summary>Adds a culture to the parsings.</summary>
        /// <param name="culture">
        /// The culture to add.
        /// </param>
        [Pure]
        private static HashSet<string> AddCulture(CultureInfo culture)
            => ResourceManager.Contains(culture)
            && ResourceManager.GetString("Values", culture) is { Length: > 0 } values
            ? new HashSet<string>([.. values.Split(';').Select(v => v.ToUpper(culture))])
            : [];

        [Impure]
        public bool Contains(IFormatProvider? formatProvider, string value)
            => (formatProvider != CultureInfo.InvariantCulture
            && Local(formatProvider as CultureInfo ?? CultureInfo.CurrentCulture).Contains(value))
            || Invariant.Contains(value);

        [Impure]
        private FrozenSet<string> Local(CultureInfo culture) => true switch
        {
            _ when TryGetValue(culture, out var existing) => existing,
            _ when AddCulture(culture).ToFrozenSet(StringComparer.OrdinalIgnoreCase) is { Count: > 0 } localized => this[culture] = localized,
            _ => Ancestors(culture),
        };

        [Impure]
        private FrozenSet<string> Ancestors(CultureInfo culture)
        {
            var ancestor = culture.Parent;

            while (ancestor != CultureInfo.InvariantCulture)
            {
                if (TryGetValue(ancestor, out var existing))
                {
                    // Ancestor and culture have the same existing localization.
                    this[culture] = existing;
                    return existing;
                }
                else if (AddCulture(ancestor).ToFrozenSet(StringComparer.OrdinalIgnoreCase) is { Count: > 0 } localized)
                {
                    // Ancestor and culture have the same localization.
                    this[ancestor] = localized;
                    this[culture] = localized;
                    return localized;
                }
                else ancestor = ancestor.Parent;
            }

            // No localization for this culture.
            var empty = FrozenSet<string>.Empty;
            this[culture] = empty;
            return empty;
        }
    }
}
