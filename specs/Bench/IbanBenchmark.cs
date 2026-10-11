using BenchmarkDotNet.Configs;
using IbanNet;
using IbanNet.Registry;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Qowaiv.Json;
using Qowaiv.Json.Financial;
using System.Buffers;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Iban = Qowaiv.Financial.InternationalBankAccountNumber;
using IbanObject = IbanNet.Iban;

namespace Bench;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[MemoryDiagnoser(true)]
[MinColumn]
public partial class IbanBenchmark
{
    [MemoryDiagnoser(true)]
    [MinColumn]
    public class JSON : IbanBenchmark
    {
        private static JsonSerializerOptions Options(JsonConverter converter)
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Strict)
            {
                WriteIndented = false,
            };
            options.Converters.Add(converter);
            return options;
                    }

        private MemoryStream Stream = null!;
        private Utf8JsonWriter Writer = null!;

        private static readonly JsonSerializerOptions current = Options(new IbanJsonConverter());
        private static readonly JsonSerializerOptions next = Options(new InternationalBankAccountNumberJsonConverter());

        [IterationSetup]
        public void Setup()
        {
            Stream = new(4096);
            Writer = new Utf8JsonWriter(Stream);
        }

        [IterationCleanup]
        public void Cleanup()
        {
            Writer.Dispose();
            Stream.Dispose();
        }


        [Benchmark(Baseline = true)]
        public Stream Baseline()
        {
            JsonSerializer.Serialize(Writer, Data.Iban.Raw, options: current);
            Writer.Flush();
            return Stream;
        }


        [Benchmark]
        public Stream Current()
        {
            JsonSerializer.Serialize(Writer, Data.Iban.Typed, options: current);
            Writer.Flush();
            return Stream;
        }

        [Benchmark]
        public Stream Next()
        {
            JsonSerializer.Serialize(Writer, Data.Iban.Typed, options: next);
            Writer.Flush();
            return Stream;
        }

        public class IbanJsonConverter : SvoJsonConverter<Iban>
        {
            protected override Iban FromJson(string? json) => Iban.FromJson(json);

            protected override object? ToJson(Iban svo) => svo.ToJson();
        }
    }

    public const int Iterations = 1000;
    public readonly string?[] Outputs = new string?[Iterations];
    public readonly Iban[] Ibans = new Iban[Iterations];
    public readonly IbanObject?[] IbanObjects = new IbanObject?[Iterations];
    public readonly string?[] Strings = new string?[Iterations];

    private static readonly IbanParser IbanParser = new(IbanRegistry.Default);

    public class Parse : IbanBenchmark
    {
        [Benchmark(Description = "Qowaiv", Baseline = true)]
        [BenchmarkCategory("machine readable")]
        public Iban[] Qowaiv_MachineReadable()
        {
            if (Ibans.Length < Data.Iban.Raw.Length) throw new NotSupportedException("Range check could not been removed");
            
            for (var i = 0; i < Data.Iban.Raw.Length; i++)
            {
                Iban.TryParse(Data.Iban.Raw[i], null, out var iban);
                Ibans[i] = iban;
            }
            return Ibans;
        }

        [Benchmark(Description = "Iban.NET")]
        [BenchmarkCategory("machine readable")]
        public IbanObject?[] IbanNet_MachineReadable()
        {
            if (IbanObjects.Length < Data.Iban.Raw.Length) throw new NotSupportedException("Range check could not been removed");

            for (var i = 0; i < Data.Iban.Raw.Length; i++)
            {
                _ = IbanParser.TryParse(Data.Iban.Raw[i], out var iban);
                IbanObjects[i] = iban;
            }
            return IbanObjects;
        }

        [Benchmark(Description = "Regex")]
        [BenchmarkCategory("machine readable")]
        public string?[] Regex_MachineReadable()
        {
            if (Strings.Length < Data.Iban.Raw.Length) throw new NotSupportedException("Range check could not been removed");

            for (var i = 0; i < Data.Iban.Raw.Length; i++)
            {
                Strings[i] = RegexBasedParser.Parse(Data.Iban.Raw[i]);
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
