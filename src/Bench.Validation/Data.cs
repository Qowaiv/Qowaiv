using MathNet.Numerics.Random;
using System.Collections.Immutable;
using System.Linq;
using System.Net;

namespace Benchmarks;

internal static class Data
{
    public static class Enums
    {
        [Flags]
        public enum Bits
        {
            One = 1,
            Two = 2,
            Tre = 4,
            For = 8,
            Fiv = 16,
            Six = 32,
            Sev = 64,
            Eig = 128,
        };

        private static readonly HttpStatusCode[] codes = Enum.GetValues<HttpStatusCode>();

        public static readonly ImmutableArray<HttpStatusCode> Values = [.. Seeds().Select(_ => codes[Rnd.Next(codes.Length)])];

        public static readonly ImmutableArray<Bits> Flags = [.. Seeds().Select(_ => (Bits)Rnd.Next(256))];
    }

    public static readonly MersenneTwister Rnd = new(42);

    private static IEnumerable<int> Seeds(int count = 1_000_000) => System.Linq.Enumerable.Range(0, count);
}
