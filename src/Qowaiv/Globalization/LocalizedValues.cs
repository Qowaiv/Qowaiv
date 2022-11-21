namespace Qowaiv.Globalization;

/// <summary>Helper method to store localized string representations of SVO's.</summary>
/// <typeparam name="TValue">
/// The type of the underlying value of the SVO.
/// </typeparam>
internal abstract class LocalizedValues<TValue> : Dictionary<CultureInfo, Dictionary<string, TValue>>
{
    protected LocalizedValues() { }

    protected LocalizedValues(Dictionary<string, TValue> invariant)
        => this[CultureInfo.InvariantCulture] = invariant;

    /// <summary>Adds a culture to the parsings.</summary>
    /// <param name="culture">
    /// The culture to add.
    /// </param>
    protected abstract void AddCulture(CultureInfo culture);

    public bool TryGetValue(IFormatProvider? provider, string str, out TValue? value)
    {
        if (this[CheckCulture(provider)].TryGetValue(str, out value) ||
            this[CultureInfo.InvariantCulture].TryGetValue(str, out value))
        {
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    [Pure]
    private CultureInfo CheckCulture(IFormatProvider? provider)
    {
        var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;
        if (!ContainsKey(culture))
        {
            lock (locker)
            {
                this[culture] = new();
                AddCulture(culture);
            }
        }
        return culture;
    }

    /// <summary>The locker for adding a culture.</summary>
    protected readonly object locker = new();
}
