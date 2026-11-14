namespace Qowaiv.Globalization.NL.Conversion;

/// <summary>Provides a conversion for an email address.</summary>
[Inheritable]
public class BurgerservicenummerTypeConverter : NumericTypeConverter<Burgerservicenummer, long>
{
    /// <inheritdoc/>
    [Pure]
    protected override Burgerservicenummer FromRaw(long raw) => Burgerservicenummer.Create(raw);

    /// <inheritdoc/>
    [Pure]
    protected override Burgerservicenummer FromString(string? str, CultureInfo? culture) => Burgerservicenummer.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override long ToRaw(Burgerservicenummer svo) => (long)svo;
}
