namespace Qowaiv.Locale.NL.Extensions;

/// <summary>Extensions on <see cref="Random"/>.</summary>
public static class QowaivRandom
{
    extension(Random rnd)
    {
        /// <summary>Returns a <see cref="Burgerservicenummer"/>.</summary>
        [Impure]
        public Burgerservicenummer NextBurgerservicenummer()
            => Burgerservicenummer.Create(rnd.NextElfproof());

        /// <summary>Returns an <see cref="Rsin"/>.</summary>
        [Impure]
        public Rsin NextRsin()
            => Rsin.Create(rnd.NextElfproof());

        [Impure]
        private long NextElfproof()
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

            return mod is 10
                ? rnd.NextElfproof()
                : val + mod;
        }
    }
}
