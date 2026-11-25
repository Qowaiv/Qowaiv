namespace Qowaiv.Locale.NL.Humanizer;

/// <summary>Extensions inspired by Humanizer.NET.</summary>
public static class Humanize
{
    extension(long value)
    {
        /// <summary>Creates a <see cref="NL.BagId" />.</summary>
        [Pure]
        public BagId BagId() => NL.BagId.Create(value);

        /// <summary>Creates a <see cref="Burgerservicenummer" />.</summary>
        [Pure]
        public Burgerservicenummer BSN() => Burgerservicenummer.Create(value);

        /// <summary>Creates a <see cref="Rsin" />.</summary>
        [Pure]
        public Rsin RSIN() => Rsin.Create(value);
    }

    extension(int value)
    {
        /// <summary>Creates a <see cref="KvkNummer" />.</summary>
        [Pure]
        public KvkNummer KvK() => KvkNummer.Create(value);
    }
}
