using Qowaiv.Financial;
using System.Collections.Immutable;
using Qowaiv.Mathematics;
using Qowaiv.Humanizer;
using Qowaiv.Extensions;

namespace Bench;

internal static class Data
{
    public static class Date
    {
        public static readonly ImmutableArray<Qowaiv.Date> Svo =
        [
            .. Enumerable.Range(0, 1000)
            .Select(i => new Qowaiv.Date(2017, 06, 11).AddDays(i))
            .OrderBy(_ => Rnd.Next())
        ];

        public static readonly ImmutableArray<DateOnly> DateOnly = [.. Svo.Select(t => (DateOnly)t)];
    }

    public static class Iban
    {
        public const int Iterations = 1_000;

        public static readonly ImmutableArray<InternationalBankAccountNumber> Svo = [.. Rnd.NextSequence(r => r.NextInternationalBankAccountNumber()).Take(Iterations)];

        public static readonly ImmutableArray<string> HumanReadable = [.. Svo.Select(svo => svo.HumanReadable())];

        public static readonly ImmutableArray<string> String = [.. Svo.Select(s => s.MachineReadable())];
    }

    public static class ModifyTypeInfo
    {
        public static readonly EmailAddress[] Svo =
        [
            .. Enumerable.Range(0, 1_000_000)
            .Select(i => (i %10 is 0) ? EmailAddress.Empty : EmailAddress.Parse($"info.{i}@qowaiv.org"))
            .OrderBy(_ => Rnd.Next())
        ];
    }

    public static class Percentage
    {
        public static readonly ImmutableArray<decimal> Decimal = [.. Enumerable.Range(0, 1000).Select(_ => (Rnd.NextDecimal() * Rnd.NextDecimal() * 200m).Round(2))];
        public static readonly ImmutableArray<Qowaiv.Percentage> Svo = [.. Decimal.Select(d => d.Percent())];

        public static readonly ImmutableArray<string> String = [.. Svo.Select(p => p.ToJson())];
    }

    public static class Unknown
    {
        public static readonly ImmutableArray<string> Values =
        [
            .. Enumerable.Repeat("?", 8000)
            .Concat(Enumerable.Repeat("Unknown", 1000))
            .Concat(Enumerable.Repeat("not known", 500))
            .Concat(Enumerable.Repeat("UNKNOWN", 250))
            .Concat(Enumerable.Repeat("NotKnown", 250))
            .Concat(Enumerable.Range(0, 990_000).Select(_ => Rnd.NextInternationalBankAccountNumber().ToString()))
            .OrderBy(_ => Rnd.Next())
        ];
    }
    public static class YN
    {
        public static readonly ImmutableArray<YesNo> Svo =
        [
            .. Enumerable.Repeat(YesNo.Yes, 450)
            .Concat(Enumerable.Repeat(YesNo.No, 450))
            .Concat(Enumerable.Repeat(YesNo.Unknown, 50))
            .Concat(Enumerable.Repeat(YesNo.Empty, 50))
            .OrderBy(_ => Rnd.Next())
        ];

        public static readonly ImmutableArray<bool?> Bool = [.. Svo.Select(t => (bool?)t)];

        public static readonly ImmutableArray<string?> String = [.. Svo.Select(t => t.ToJson())];
    }

    internal static readonly MersenneTwister Rnd = new(42);
}
