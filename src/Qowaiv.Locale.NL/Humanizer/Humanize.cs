namespace Qowaiv.Locale.NL.Humanizer;

/// <summary>Extensions inspired by Humanizer.NET.</summary>
public static class Humanize
{
    extension(long value)
    {

        /// <summary>Creates a <see cref="Burgerservicenummer" />.</summary>
        [Pure]
        public Burgerservicenummer BSN() => Burgerservicenummer.Create(value);
    }

    extension(int value)
    {
        /// <summary>Creates a <see cref="KvkNummer" />.</summary>
        [Pure]
        public KvkNummer KvK() => KvkNummer.Create(value);
    }
}
