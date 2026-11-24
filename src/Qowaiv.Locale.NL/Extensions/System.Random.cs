namespace Qowaiv.Locale.NL.Extensions;

/// <summary>Extensions on <see cref="Random"/>.</summary>
public static class QowaivRandom
{
    extension(Random rnd)
    {
        /// <summary>Returns a <see cref="Burgerservicenummer"/>.</summary>
        /// <remarks>Generates a nine-digit test number that satisfies the elfproef.</remarks>
        [Impure]
        public Burgerservicenummer NextBurgerservicenummer()
            => Burgerservicenummer.Create(rnd.NextElfproef());

        /// <summary>Returns an <see cref="Rsin"/>.</summary>
        /// <remarks>Generates a nine-digit test number that satisfies the elfproef.</remarks>
        [Impure]
        public Rsin NextRsin()
            => Rsin.Create(rnd.NextElfproef());

        /// <summary>Returns a nine-digit identifier that satisfies the elfproef.</summary>
        /// <remarks>
        /// The first eight digits are generated in the 9990.00.00 - 9999.99.99 range;
        /// the ninth digit is the checksum. A checksum of 10 is discarded.
        /// </remarks>
        [Impure]
        private long NextElfproef()
        {
            while (true)
            {
                var cur = (long)rnd.Next(9990_00_00, 9999_99_99);
                var val = cur * 10;
                var sum = 0L;

                for (var pos = 2; pos <= 9; pos++)
                {
                    var nxt = cur / 10;
                    sum += (cur - (nxt * 10)) * pos;
                    cur = nxt;
                }

                var mod = sum % 11;
                if (mod is not 10) return val + mod;
            }
        }
    }
}
