namespace Qowaiv.Globalization.NL.Humanizer;

public static class Humanize
{
    extension(long value)
    {
        [Pure]
        public Burgerservicenummer BSN() => Burgerservicenummer.Create(value);
    }
}
