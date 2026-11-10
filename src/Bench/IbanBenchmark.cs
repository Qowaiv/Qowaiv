using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using IbanNet;
using IbanNet.Registry;
using System.IO;
using System.Text.Json;
using Iban = Qowaiv.Financial.InternationalBankAccountNumber;
using IbanObject = IbanNet.Iban;

namespace Bench;

public partial class IbanBenchmark
{
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class IBAN_read_JSON : JsonReaderBenchmark
    {
        public IBAN_read_JSON()
            => JsonSerializer.Serialize(Stream1, Data.Iban.String, options: Default);

        [Benchmark(Baseline = true)]
        public string[]? String()
            => JsonSerializer.Deserialize<string[]>(Stream1, options: Default);

        [Benchmark]
        public Iban[]? Iban()
            => JsonSerializer.Deserialize<Iban[]>(Stream1, options: Default);
    }

    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class IBAN_write_JSON : JsonWriterBenchmark
    {
        [Benchmark(Baseline = true)]
        public Stream String()
        {
            JsonSerializer.Serialize(Writer1, Data.Iban.String, options: Default);
            return Stream1;
        }

        [Benchmark]
        public Stream Iban()
        {
            JsonSerializer.Serialize(Writer1, Data.Iban.Svo, options: Default);
            return Stream1;
        }
    }

    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser(true)]
    [MinColumn]
    public class IBAN_Parse : IbanBenchmark
    {
        public const int Iterations = 1000;
        public readonly string?[] Outputs = new string?[Iterations];
        public readonly Iban[] Ibans = new Iban[Iterations];
        public readonly IbanObject?[] IbanObjects = new IbanObject?[Iterations];
        public readonly string?[] Strings = new string?[Iterations];

        private static readonly IbanParser IbanParser = new(IbanRegistry.Default);

        [Benchmark(Description = "Qowaiv", Baseline = true)]
        [BenchmarkCategory("machine readable")]
        public Iban[] Qowaiv_MachineReadable()
        {
            if (Ibans.Length < Data.Iban.String.Length) throw new NotSupportedException("Range check could not been removed");

            for (var i = 0; i < Data.Iban.String.Length; i++)
            {
                Iban.TryParse(Data.Iban.String[i], null, out var iban);
                Ibans[i] = iban;
            }
            return Ibans;
        }

        [Benchmark(Description = "Iban.NET")]
        [BenchmarkCategory("machine readable")]
        public IbanObject?[] IbanNet_MachineReadable()
        {
            if (IbanObjects.Length < Data.Iban.String.Length) throw new NotSupportedException("Range check could not been removed");

            for (var i = 0; i < Data.Iban.String.Length; i++)
            {
                _ = IbanParser.TryParse(Data.Iban.String[i], out var iban);
                IbanObjects[i] = iban;
            }
            return IbanObjects;
        }

        [Benchmark(Description = "Regex")]
        [BenchmarkCategory("machine readable")]
        public string?[] Regex_MachineReadable()
        {
            if (Strings.Length < Data.Iban.String.Length) throw new NotSupportedException("Range check could not been removed");

            for (var i = 0; i < Data.Iban.String.Length; i++)
            {
                Strings[i] = RegexBasedParser.Parse(Data.Iban.String[i]);
            }
            return Strings;
        }

        [Benchmark(Description = "Qowaiv", Baseline = true)]
        [BenchmarkCategory("human readable")]
        public Iban[] Qowaiv_HumanReadable()
        {
            if (Ibans.Length < Data.Iban.HumanReadable.Length) throw new NotSupportedException("Range check could not been removed");

            for (var i = 0; i < Data.Iban.HumanReadable.Length; i++)
            {
                Iban.TryParse(Data.Iban.HumanReadable[i], null, out var iban);
                Ibans[i] = iban;
            }
            return Ibans;
        }

        [Benchmark(Description = "Iban.NET")]
        [BenchmarkCategory("human readable")]
        public IbanObject?[] IbanNet_HumanReadable()
        {
            if (IbanObjects.Length < Data.Iban.HumanReadable.Length) throw new NotSupportedException("Range check could not been removed");

            for (var i = 0; i < Data.Iban.HumanReadable.Length; i++)
            {
                _ = IbanParser.TryParse(Data.Iban.HumanReadable[i], out var iban);
                IbanObjects[i] = iban;
            }
            return IbanObjects;
        }

        [Benchmark(Description = "Regex")]
        [BenchmarkCategory("human readable")]
        public string?[] Regex_HumanReadable()
        {
            if (Strings.Length < Data.Iban.HumanReadable.Length) throw new NotSupportedException("Range check could not been removed");

            for (var i = 0; i < Data.Iban.HumanReadable.Length; i++)
            {
                Strings[i] = RegexBasedParser.Parse(Data.Iban.HumanReadable[i]);
            }
            return Strings;
        }
    }
}
