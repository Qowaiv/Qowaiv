using System.Collections.Concurrent;
using System.Reflection;

namespace Qowaiv;

/// <summary>Helps handling the unknown status of Single Value Objects.</summary>
/// <remarks>
/// The 'unknown' case differs from the 'empty' case. 'Empty' means not set
/// (yet), i.e. an unset (default) value; 'unknown' means that the user has set
/// the value, saying there must be some value but they don't know which.
///
/// Note that not all scenarios that support 'empty' support 'unknown' too.
/// </remarks>
public readonly partial struct Unknown : IEquatable<Unknown>
{
    /// <summary>Returns true if the string represents unknown, otherwise false.</summary>
    /// <param name="val">
    /// The string value to test.
    /// </param>
    [Pure]
    public static bool IsUnknown(string? val) => TryParse(val, null, out _);

    /// <summary>Returns true if the string represents unknown, otherwise false.</summary>
    /// <param name="val">
    /// The string value to test.
    /// </param>
    /// <param name="culture">
    /// The culture to test for.
    /// </param>
    [Pure]
    public static bool IsUnknown(string? val, CultureInfo? culture) => TryParse(val, culture, out _);

    /// <summary>Gets the value that represents set but unknown.</summary>
    /// <param name="type">
    /// The type that could have an unknown value.
    /// </param>
    /// <returns>
    /// null, if not defined, otherwise the unknown value for a type.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// if type is not a value type.
    /// </exception>
    /// <remarks>
    /// The unknown value is expected to be a static field or property of the type with the name "Unknown".
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

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "?";

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Unknown;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Unknown other) => true;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => 0;

    /// <summary>The resource manager managing the culture based string values.</summary>
    private static readonly ResourceManager ResourceManager = new("Qowaiv.UnknownLabels", typeof(Unknown).Assembly);

    private static readonly UnknownValues ParseValues = new();

#pragma warning disable S1121 // Improves the readability in this case.
    private sealed class UnknownValues : ConcurrentDictionary<CultureInfo, FrozenSet<string>>
    {
        private readonly FrozenSet<string> Invariant = frozen_case_insensitive("?", "UNKNOWN", "NOT KNOWN", "NOTKNOWN");

        [Impure]
        public bool Contains(IFormatProvider? formatProvider, string value)
            => (formatProvider ?? CultureInfo.CurrentCulture) is CultureInfo { Name.Length: > 0 } culture
            ? Local(culture).Contains(value)
            : Invariant.Contains(value);

        [Impure]
        private FrozenSet<string> Local(CultureInfo culture)
            => TryGetValue(culture, out var existing)
            ? existing
            : Ancestors(culture);

        [Impure]
        private FrozenSet<string> Ancestors(CultureInfo culture)
        {
            if (culture == CultureInfo.InvariantCulture) return Invariant;

            var translations = TryGetValue(culture, out var existing)
                ? existing
                : Ancestors(culture.Parent);

            if (Translations(culture) is { } current)
                translations = frozen_case_insensitive([.. translations, .. current]);

            this[culture] = translations;
            return translations;
        }

        [Pure]
        private static string[]? Translations(CultureInfo culture)
            => ResourceManager.Contains(culture)
            && ResourceManager.GetString("Values", culture) is { Length: > 0 } values
            ? [.. values.Split(';').Select(v => v.ToUpper(culture))]
            : null;
    }
}

#if NETSTANDARD2_0
public readonly partial struct Unknown
#else
public readonly partial struct Unknown : IParsable<Unknown>
#endif
{
    /// <summary>Parses a string into unknown.</summary>
    /// <param name="s">
    /// The string to parse.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s" />.
    /// </param>
    /// <returns>
    /// The result of parsing <paramref name="s" />.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s" /> is not in the correct format.
    /// </exception>
    [Pure]
    public static Unknown Parse(string s, IFormatProvider? provider)
        => Parse(s.AsSpan(), provider);

    /// <summary>Tries to parse a string into unknown.</summary>
    /// <param name="s">
    /// The string to parse.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s" />.
    /// </param>
    /// <param name="result">
    /// The parse result.
    /// </param>
    /// <returns>
    /// <c>true</c> if <paramref name="s" /> was successfully parsed; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Unknown result)
        => TryParse(s.AsSpan(), provider, out result);
}

#if NETSTANDARD2_0
public readonly partial struct Unknown
#else
public readonly partial struct Unknown : ISpanParsable<Unknown>
#endif
{
    /// <summary>Parses a string into unknown.</summary>
    /// <param name="s">
    /// The string to parse.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s" />.
    /// </param>
    /// <returns>
    /// The result of parsing <paramref name="s" />.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s" /> is not in the correct format.
    /// </exception>
    [Pure]
    public static Unknown Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var result)
        ? result
        : throw Unparsable.ForValue<Unknown>(s.ToString(), QowaivMessages.FormatExceptionUnknown);

    /// <summary>Tries to parse a string into unknown.</summary>
    /// <param name="s">
    /// The string to parse.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s" />.
    /// </param>
    /// <param name="result">
    /// The parse result.
    /// </param>
    /// <returns>
    /// <c>true</c> if <paramref name="s" /> was successfully parsed; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Unknown result)
    {
        result = default;
        return ParseValues.Contains(provider, s.ToString());
    }
}
