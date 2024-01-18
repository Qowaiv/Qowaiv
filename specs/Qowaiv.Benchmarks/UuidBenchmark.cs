using BenchmarkDotNet.Attributes;
using Qowaiv;
using Qowaiv.Identifiers;

namespace Benchmarks;

public partial class UuidBenchmark
{
    public sealed class ForUuid : UuidBehavior { }

    private const int Iterations = 1000;
    private static readonly Guid[] Guids = new Guid[Iterations];
    private static readonly Uuid[] Uuids = new Uuid[Iterations];
    private static readonly Id<ForUuid>[] IDs = new Id<ForUuid>[Iterations];

    public class Parse: UuidBenchmark
    {
        private string[] Strings { get; set; } = [];
        private string[] Base64s { get; set; } = [];
        private string[] Base32s { get; set; } = [];

        [GlobalSetup]
        public void Setup()
        {
            Strings = Enumerable.Range(0, Iterations).Select(_ => Guid.NewGuid().ToString()).ToArray();
            Base64s = Enumerable.Range(0, Iterations).Select(_ => Uuid.NewUuid().ToString()).ToArray();
            Base32s = Enumerable.Range(0, Iterations).Select(_ => Uuid.NewUuid().ToString("H")).ToArray();
        }

        [Benchmark(Baseline = true)]
        public Guid[] GUID_Parse()
        {
            for (var i = 0; i < Strings.Length; i++)
            {
                Guids[i] = Guid.Parse(Strings[i]);
            }
            return Guids;
        }
        
        [Benchmark]
        public Uuid[] UUID_Parse()
        {
            for (var i = 0; i < Strings.Length; i++)
            {
                Uuids[i] = Uuid.Parse(Strings[i]);
            }
            return Uuids;
        }

        [Benchmark]
        public Uuid[] UUID_Parse_Base64()
        {
            for (var i = 0; i < Strings.Length; i++)
            {
                Uuids[i] = Uuid.Parse(Base64s[i]);
            }
            return Uuids;
        }

        [Benchmark]
        public Uuid[] UUID_Parse_Base32()
        {
            for (var i = 0; i < Strings.Length; i++)
            {
                Uuids[i] = Uuid.Parse(Base32s[i]);
            }
            return Uuids;
        }

        [Benchmark]
        public  Id<ForUuid>[] ID_for_UUID_Parse()
        {
            for (var i = 0; i < Base64s.Length; i++)
            {
                IDs[i] = Id<ForUuid>.Parse(Base64s[i]);
            }
            return IDs;
        }

        [Benchmark]
        public Uuid[] Convert_fromBase64()
        {
            for (var i = 0; i < Base64s.Length; i++)
            {
                Uuids[i] = Reference.Convert_FromBase64(Base64s[i]);
            }
            return Uuids;
        }
    }
}
