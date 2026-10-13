#pragma warning disable S1121 // improves the readabillity in this case.

using System.Collections.Concurrent;

namespace Qowaiv.Globalization;

/// <summary>Helper class to store localized string representations of SVOs.</summary>
/// <typeparam name="TValue">
/// The type of the underlying value of the SVO.
/// </typeparam>
internal abstract class LocalizedValues<TValue> : ConcurrentDictionary<CultureInfo, FrozenDictionary<string, TValue>>
{
    private readonly FrozenDictionary<string, TValue> Invariant;

    [SuppressMessage("Critical Code Smell", "S1699:Constructors should only call non-overridable methods", Justification = "Leads to cleaner code")]
    protected LocalizedValues() => Invariant = Init().ToFrozenDictionary();

    /// <summary>Initializes the invariant values.</summary>
    [Pure]
    protected abstract Dictionary<string, TValue> Init();

    /// <summary>Adds a culture to the parsings.</summary>
    /// <param name="culture">
    /// The culture to add.
    /// </param>
    [Pure]
    protected abstract Dictionary<string, TValue> AddCulture(CultureInfo culture);

    public bool TryGetValue(IFormatProvider? formatProvider, string key, out TValue? value)
        => (formatProvider != CultureInfo.InvariantCulture
        && Local(formatProvider as CultureInfo ?? CultureInfo.CurrentCulture).TryGetValue(key, out value))
        || Invariant.TryGetValue(key, out value);

    [Impure]
    private FrozenDictionary<string, TValue> Local(CultureInfo culture) => true switch
    {
        _ when TryGetValue(culture, out var existing) => existing,
        _ when AddCulture(culture).ToFrozenDictionary() is { Count: > 0 } localized => this[culture] = localized,
        _ => Ancestors(culture),
    };

    [Impure]
    private FrozenDictionary<string, TValue> Ancestors(CultureInfo culture)
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
            else if (AddCulture(ancestor).ToFrozenDictionary() is { Count: > 0 } localized)
            {
                // Ancestor and culture have the same localization.
                this[ancestor] = localized;
                this[culture] = localized;
                return localized;
            }
            else ancestor = ancestor.Parent;
        }

        // No localization for this culture.
        var empty = FrozenDictionary<string, TValue>.Empty;
        this[culture] = empty;
        return empty;
    }
}
