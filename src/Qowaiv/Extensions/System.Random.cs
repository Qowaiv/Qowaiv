using Qowaiv.Financial;
using Qowaiv.Globalization;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="Random"/>.</summary>
public static class QowaivRandom
{
    extension(Random rnd)
    {
        /// <summary>Creates an unbounded (pseudo) random sequence.</summary>
        /// <param name="next">
        /// The selector of a single random item.
        /// </param>
        /// <remarks>
        /// Values are generated lazily while enumerating. As with any random
        /// sequence, values may repeat.
        /// </remarks>
        [Impure]
        public IEnumerable<T> NextSequence<T>(Func<Random, T> next) where T : notnull
            => new RandomSequence<T>(rnd, Guard.NotNull(next));

        /// <summary>Returns a <see cref="char"/>.</summary>
        /// <param name="values">
        /// The values to pick from.
        /// </param>
        [Impure]
        public char NextChar(string values) => Guard.NotNullOrEmpty(values)[rnd.Next(values.Length)];

        /// <summary>Returns a <see cref="char"/>.</summary>
        /// <param name="minValue">
        /// The minimum character to pick from.
        /// </param>
        /// <param name="maxValue">
        /// The maximum character to pick from (included).
        /// </param>
        [Impure]
        public char NextChar(char minValue, char maxValue) => (char)rnd.Next(minValue, maxValue + 1);

        /// <summary>Returns a random <see cref="Fraction"/>.</summary>
        /// <remarks>
        /// The defaults are <see cref="Fraction.Zero"/> (inclusive), <see cref="Fraction.One"/>
        /// (exclusive), and <see cref="int.MaxValue"/> as the maximum denominator.
        /// </remarks>
        [Impure]
        public Fraction NextFraction()
            => rnd.NextFraction(Fraction.Zero, Fraction.One, int.MaxValue);

        /// <summary>Returns a <see cref="Fraction"/>.</summary>
        /// <param name="minValue">
        /// The minimum fraction to pick from.
        /// </param>
        /// <param name="maxValue">
        /// The maximum fraction to pick from (excluded).
        /// </param>
        /// <param name="maxDenominator">
        /// The maximum denominator to pick from (included).
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxValue"/> is less than or equal to <paramref name="minValue"/>,
        /// <paramref name="maxDenominator"/> is less than 2, or no fraction within the boundaries
        /// can be represented with a denominator up to <paramref name="maxDenominator"/>.
        /// </exception>
        [Impure]
        public Fraction NextFraction(Fraction minValue, Fraction maxValue, int maxDenominator)
        {
            if (maxValue <= minValue) throw new ArgumentOutOfRangeException(nameof(maxValue), "The max value should be bigger than the min value.");
            if (maxDenominator < 2) throw new ArgumentOutOfRangeException(nameof(maxDenominator), "The Max denominator should be [2, ..].");

            var denominator = rnd.Next(maxDenominator) + 1L;

            if (!Numerators(denominator, out var minNumerator, out var maxNumeratorExclusive)
                && !Fallback(out denominator, out minNumerator, out maxNumeratorExclusive))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxDenominator),
                    $"No fraction in [{minValue}, {maxValue}) can be represented with a denominator up to {maxDenominator}.");
            }

            var numerator = rnd.NextInt64(minNumerator, maxNumeratorExclusive);

            return numerator.DividedBy(denominator);

            bool Fallback(out long fallbackDenominator, out long min, out long maxExclusive)
            {
                var width = maxValue - minValue;
                var requiredDenominator = Ceiling(width.Denominator, width.Numerator);

                foreach (var candidate in new[] { minValue.Denominator, maxValue.Denominator, requiredDenominator, maxDenominator })
                {
                    if (candidate <= maxDenominator
                        && Numerators(candidate, out min, out maxExclusive))
                    {
                        fallbackDenominator = candidate;
                        return true;
                    }
                }

                fallbackDenominator = default;
                min = default;
                maxExclusive = default;
                return false;
            }

            bool Numerators(long candidate, out long min, out long max)
            {
                min = Ceiling(minValue.Numerator * candidate, minValue.Denominator);
                max = Ceiling(maxValue.Numerator * candidate, maxValue.Denominator);
                return min < max;
            }

            static long Ceiling(long dividend, long divisor)
            {
                var quotient = dividend / divisor;
                return dividend % divisor > 0 ? quotient + 1 : quotient;
            }
        }

        /// <summary>Returns an <see cref="InternationalBankAccountNumber"/>.</summary>
        [Impure]
        public InternationalBankAccountNumber NextInternationalBankAccountNumber()
            => rnd.NextInternationalBankAccountNumber(
                InternationalBankAccountNumber.Supported
                    .Skip(rnd.Next(InternationalBankAccountNumber.Supported.Count))
                    .First());

        /// <summary>Returns an <see cref="InternationalBankAccountNumber"/>.</summary>
        [Impure]
        public InternationalBankAccountNumber NextInternationalBankAccountNumber(Country country)
        {
            if (country is not { IsKnown: true, Name.Length: 2 })
                throw new ArgumentOutOfRangeException(nameof(country), "Only existing countries are supported.");

            _ = Bbans.TryGetValue(country, out var bban);
            bban ??= $"{country}kkTESTnncccccccccc";
            return rnd.NextInternationalBankAccountNumber(bban);
        }

        /// <summary>Returns an <see cref="InternationalBankAccountNumber"/>.</summary>
        [Impure]
        private InternationalBankAccountNumber NextInternationalBankAccountNumber(string pattern)
        {
            Span<char> iban = stackalloc char[pattern.Length];

            for (var i = 0; i < pattern.Length; i++)
            {
                iban[i] = pattern[i] switch
                {
                    'a' => A(),
                    'n' => N(),
                    'c' => C(),
                    var ch => ch,
                };
            }

            var checkDigits = pattern[^1] switch
            {
                // IBANs without a fixed checksum
                not 'k' => Checksum([.. iban[4..], .. iban[..2]]),

                // When the [country/checksum] is 71, get the checksum for
                // the BBAN part only.
                _ when Mod97(iban[..4]) is 71 => Checksum(iban[4..^2]),

                // In the other cases (TN, MR) the [country/checksum] is 1
                //  get the checksum for BBAN part and subtract 1.
                _ => Checksum(iban[4..^2]) - 1,
            };

            return InternationalBankAccountNumber.Parse(iban.ToString().Replace("kk", checkDigits.ToString("00")));

            char A() => rnd.NextChar('A', 'Z');
            char N() => rnd.NextChar('0', '9');
            char C() => rnd.NextDouble() > 0.5 ? A() : N();

            static int Checksum(ReadOnlySpan<char> iban)
                => 98 - ((Mod97(iban) * 100) % 97);

            static int Mod97(ReadOnlySpan<char> iban)
            {
                var mod = 0;
                foreach (var c in iban)
                {
                    mod = c <= '9'
                        ? (mod * 010) + (c - '0')
                        : (mod * 100) + (c - 'A' + 10);
                    mod %= 97;
                }
                return mod;
            }
        }

        /// <summary>Returns an <see cref="long"/>.</summary>
        [Impure]
        private long NextInt64()
        {
            var buffer = new byte[8];

            rnd.NextBytes(buffer);
            var candidate = BitConverter.ToInt64(buffer, 0);

            // wrap negative numbers around, mapping every negative number to a distinct nonnegative number
            // MinValue -> 0, -1 -> MaxValue
            candidate &= long.MaxValue;

            // skip candidate if it is MaxValue. Recursive since rare.
            return (candidate == long.MaxValue) ? rnd.NextInt64() : candidate;
        }

        /// <summary>Returns an <see cref="long"/>.</summary>
        [Impure]
        private long NextInt64(long minValue, long maxValue)
        {
            var number = rnd.NextInt64();
            var delta = maxValue - minValue;
            return minValue + number % delta;
        }
    }

    private struct RandomSequence<T>(Random rnd, Func<Random, T> next) : IEnumerator<T>, IEnumerable<T> where T : notnull
    {
        private readonly Random Rnd = rnd;
        private readonly Func<Random, T> Next = next;

        /// <inheritdoc />
        public T Current { get; private set; } = default!;

        /// <inheritdoc />
        readonly object IEnumerator.Current => Current;

        /// <inheritdoc />
        [Impure]
        public bool MoveNext()
        {
            Current = Next(Rnd);
            return true;
        }

        /// <inheritdoc />
        [Pure]
        public readonly IEnumerator<T> GetEnumerator() => this;

        /// <inheritdoc />
        [Pure]
        readonly IEnumerator IEnumerable.GetEnumerator() => this;

        /// <inheritdoc />
        [DoesNotReturn]
        public void Reset() => throw new NotSupportedException();

        /// <inheritdoc />
        public void Dispose() { /* Nothing to dispose. */ }
    }

    private static readonly CountryLookup<string?> Bbans = CountryLookup<string?>.New(
    [
        new(Country.AD, "ADkknnnnnnnnTESTcccccccc"),
        new(Country.AE, "AEkknnnnnnnnnnnnnnnnnnn"),
        new(Country.AL, "ALkknnnnnnnnTESTcccccccccccc"),
        new(Country.AO, "AOkknnnnnnnnnnnnnnnnnnnnn"),
        new(Country.AT, "ATkknnnnnnnnnnnnnnnn"),
        new(Country.AZ, "AZkkTESTcccccccccccccccccccc"),
        new(Country.BA, "BA39nnnnnnnnnnnnnnkk"),
        new(Country.BE, "BEkknnnnnnnnnnnn"),
        new(Country.BF, "BFkkTT00nnnnnnnnnnnnnnnnnnnn"),
        new(Country.BG, "BGkkTESTnnnnnncccccccc"),
        new(Country.BH, "BHkkTESTcccccccccccccc"),
        new(Country.BI, "BIkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.BJ, "BJkkTT00nnnnnnnnnnnnnnnnnnnn"),
        new(Country.BR, "BRkknnnnnnnnnnnnnnnnnnnnnnnac"),
        new(Country.BY, "BYkkTESTnnnncccccccccccccccc"),
        new(Country.CF, "CFkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.CG, "CGkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.CH, "CHkknnnnnTESTcccccccc"),
        new(Country.CI, "CIkkTT00nnnnnnnnnnnnnnnnnnnn"),
        new(Country.CM, "CMkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.CR, "CRkk0nnnnnnnnnnnnnnnnn"),
        new(Country.CV, "CVkknnnnnnnnnnnnnnnnnnnnn"),
        new(Country.CY, "CYkknnnnnnnnTESTcccccccccccc"),
        new(Country.CZ, "CZkknnnnnnnnnnnnnnnnnnnn"),
        new(Country.DE, "DEkknnnnnnnnnnnnnnnnnn"),
        new(Country.DJ, "DJkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.DK, "DKkknnnnnnnnnnnnnn"),
        new(Country.DO, "DOkkTESTnnnnnnnnnnnnnnnnnnnn"),
        new(Country.DZ, "DZkknnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.EE, "EEkknnnnnnnnnnnnnnnn"),
        new(Country.EG, "EGkknnnnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.ES, "ESkknnnnnnnnnnnnnnnnnnnn"),
        new(Country.FI, "FIkknnnnnnnnnnnnnn"),
        new(Country.FK, "FKkkTT00nnnnnnnnnn"),
        new(Country.FO, "FOkknnnnnnnnnnnnnn"),
        new(Country.FR, "FRkknnnnnnnnnnTESTcccccTT00"),
        new(Country.GA, "GAkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.GB, "GBkkTESTnnnnnnnnnnnnnn"),
        new(Country.GE, "GEkkTT00nnnnnnnnnnnnnn"),
        new(Country.GI, "GIkkTESTccccccccccccccc"),
        new(Country.GL, "GLkknnnnnnnnnnnnnn"),
        new(Country.GQ, "GQkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.GR, "GRkknnnnnnnTESTcccccccccccc"),
        new(Country.GT, "GTkkTESTcccccccccccccccccccc"),
        new(Country.GW, "GWkkTT00nnnnnnnnnnnnnnnnn"),
        new(Country.HN, "HNkkTESTnnnnnnnnnnnnnnnnnnnn"),
        new(Country.HR, "HRkknnnnnnnnnnnnnnnnn"),
        new(Country.HU, "HUkknnnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.IE, "IEkkTESTnnnnnnnnnnnnnn"),
        new(Country.IL, "ILkknnnnnnnnnnnnnnnnnnn"),
        new(Country.IQ, "IQkkTESTnnnnnnnnnnnnnnn"),
        new(Country.IR, "IRkknnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.IS, "ISkknnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.IT, "ITkkannnnnnnnnnTESTcccccccc"),
        new(Country.JO, "JOkkTESTnnnncccccccccccccccccc"),
        new(Country.KM, "KMkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.KW, "KWkkTESTcccccccccccccccccccccc"),
        new(Country.KZ, "KZkknnnTESTccccccccc"),
        new(Country.LB, "LBkknnnnTESTcccccccccccccccc"),
        new(Country.LC, "LCkkTESTcccccccccccccccccccccccc"),
        new(Country.LI, "LIkknnnnnTESTcccccccc"),
        new(Country.LT, "LTkknnnnnnnnnnnnnnnn"),
        new(Country.LU, "LUkknnnTESTccccccccc"),
        new(Country.LV, "LVkkTESTccccccccccccc"),
        new(Country.LY, "LYkknnnnnnnnnnnnnnnnnnnnn"),
        new(Country.MA, "MAkknnnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.MC, "MCkknnnnnnnnnnTESTcccccTT00"),
        new(Country.MD, "MDkkTESTcccccccccccccccc"),
        new(Country.ME, "ME25nnnnnnnnnnnnnnnnkk"),
        new(Country.MG, "MGkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.MK, "MK07nnnTESTccccTTkk"),
        new(Country.ML, "MLkkTT00nnnnnnnnnnnnnnnnnnnn"),
        new(Country.MN, "MNkknnnnnnnnnnnnnnnn"),
        new(Country.MR, "MR13nnnnnnnnnnnnnnnnnnnnnkk"),
        new(Country.MT, "MTkkTESTnnnnncccccccccccccccccc"),
        new(Country.MU, "MUkkTESTnnnnnnnnnnnnnnnn000EUR"),
        new(Country.MZ, "MZkknnnnnnnnnnnnnnnnnnnnn"),
        new(Country.NE, "NEkkTT00nnnnnnnnnnnnnnnnnnnn"),
        new(Country.NI, "NIkkTESTnnnnnnnnnnnnnnnnnnnn"),
        new(Country.NL, "NLkkTESTnnnnnnnnnn"),
        new(Country.NO, "NOkknnnnnnnnnnn"),
        new(Country.OM, "OMkknnnTESTcccccccccccc"),
        new(Country.PK, "PKkkTESTcccccccccccccccc"),
        new(Country.PL, "PLkknnnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.PS, "PSkkTESTccccccccccccccccccccc"),
        new(Country.PT, "PT50nnnnnnnnnnnnnnnnnnnkk"),
        new(Country.QA, "QAkkTESTccccccccccccccccccccc"),
        new(Country.RO, "ROkkTESTcccccccccccccccc"),
        new(Country.RS, "RS35nnnnnnnnnnnnnnnnkk"),
        new(Country.RU, "RUkknnnnnnnnnnnnnnTESTccccccccccc"),
        new(Country.SA, "SAkknnTESTcccccccccccccc"),
        new(Country.SC, "SCkkTESTnnnnnnnnnnnnnnnnnnnnEUR"),
        new(Country.SD, "SDkknnnnnnnnnnnnnn"),
        new(Country.SE, "SEkknnnnnnnnnnnnnnnnnnnn"),
        new(Country.SI, "SI56nnnnnnnnnnnnnkk"),
        new(Country.SK, "SKkknnnnnnnnnnnnnnnnnnnn"),
        new(Country.SM, "SMkkannnnnnnnnnTESTcccccccc"),
        new(Country.SN, "SNkkTT00nnnnnnnnnnnnnnnnnnnn"),
        new(Country.SO, "SOkknnnnnnnnnnnnnnnnnnn"),
        new(Country.ST, "STkknnnnnnnnnnnnnnnnnnnnn"),
        new(Country.SV, "SVkkTESTnnnnnnnnnnnnnnnnnnnn"),
        new(Country.TD, "TDkknnnnnnnnnnnnnnnnnnnnnnn"),
        new(Country.TG, "TGkkTT00nnnnnnnnnnnnnnnnnnnn"),
        new(Country.TL, "TL38nnnnnnnnnnnnnnnnnkk"),
        new(Country.TN, "TN59nnnnnnnnnnnnnnnnnnkk"),
        new(Country.TR, "TRkknnnnn0TESTcccccccccccc"),
        new(Country.UA, "UAkknnnnnnTESTccccccccccccccc"),
        new(Country.VA, "VAkknnnnnnnnnnnnnnnnnn"),
        new(Country.VG, "VGkkTESTnnnnnnnnnnnnnnnn"),
        new(Country.XK, "XKkknnnnnnnnnnnnnnnn"),
        new(Country.YE, "YEkkTESTnnnncccccccccccccccccc"),
    ]);
}
