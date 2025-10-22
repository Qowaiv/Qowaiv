using Qowaiv;

namespace Benchmarks;

public partial class UuidBenchmark
{
    internal const int Iterations = 1000;
    internal Guid[] Guids = new Guid[Iterations];
    internal Uuid[] Uuids = new Uuid[Iterations];
    internal readonly UuidVersion[] Versions = new UuidVersion[Iterations];

    public class Parse : UuidBenchmark
    {
        private string[] Strings { get; set; } = [];
        private string[] Base64s { get; set; } = [];
        private string[] Base32s { get; set; } = [];

        [GlobalSetup]
        public void Setup()
        {
            Uuids = [.. Enumerable.Range(0, Iterations).Select(_ => Uuid.NewUuid())];
            Guids = [.. Uuids.Select(x => (Guid)x)];
            Strings = [.. Uuids.Select(g => g.ToString("D"))];
            Base64s = [.. Uuids.Select(g => g.ToString("S"))];
            Base32s = [.. Uuids.Select(g => g.ToString("H"))];
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
    }

    public class StringOutput : UuidBenchmark
    {
        private string[] Strings { get; set; } = new string[Iterations];

        [GlobalSetup]
        public void Setup()
        {
            Uuids = [.. Enumerable.Range(0, Iterations).Select(_ => Uuid.NewUuid())];
            Guids = [.. Uuids.Select(x => (Guid)x)];
        }

        [Benchmark(Baseline = true)]
        public string[] GUID_ToString()
        {
            for (var i = 0; i < Guids.Length; i++)
            {
                Strings[i] = Guids[i].ToString();
            }
            return Strings;
        }

        [Benchmark]
        public string[] GUID()
        {
            for (var i = 0; i < Uuids.Length; i++)
            {
                Strings[i] = Uuids[i].ToString("B");
            }
            return Strings;
        }

        [Benchmark]
        public string[] Base64()
        {
            for (var i = 0; i < Uuids.Length; i++)
            {
                Strings[i] = Uuids[i].ToJson()!;
            }
            return Strings;
        }
    }

    public class Version : UuidBenchmark
    {
        [Benchmark(Baseline = true)]
        public UuidVersion[] Layout()
        {
            for (var i = 0; i < Uuids.Length; i++)
            {
                Versions[i] = Uuids[i].Version;
            }
            return Versions;
        }
    }
}
