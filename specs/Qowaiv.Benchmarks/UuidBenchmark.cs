using Qowaiv;
using Qowaiv.Identifiers;

namespace Benchmarks;

public partial class UuidBenchmark
{
    [EmptyTestClass]
    public sealed class ForUuid : UuidBehavior { }

    internal const int Iterations = 1000;
    internal Guid[] Guids = new Guid[Iterations];
    internal Uuid[] Uuids = new Uuid[Iterations];
    internal readonly Id<ForUuid>[] IDs = new Id<ForUuid>[Iterations];
    internal readonly UuidVersion[] Versions = new UuidVersion[Iterations];

    public class Parse : UuidBenchmark
    {
        private string[] Strings { get; set; } = [];
        private string[] Base64s { get; set; } = [];
        private string[] Base32s { get; set; } = [];

        [GlobalSetup]
        public void Setup()
        {
            Uuids = Enumerable.Range(0, Iterations).Select(_ => Uuid.NewUuid()).ToArray();
            Guids = Uuids.Select(x => (Guid)x).ToArray();
            Strings = Uuids.Select(g => g.ToString("D")).ToArray();
            Base64s = Uuids.Select(g => g.ToString("S")).ToArray();
            Base32s = Uuids.Select(g => g.ToString("H")).ToArray();
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
        public Id<ForUuid>[] ID_for_UUID_Parse()
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

    public class StringOutput : UuidBenchmark
    {
        private string[] Strings { get; set; } = new string[Iterations];

        [GlobalSetup]
        public void Setup()
        {
            Uuids = Enumerable.Range(0, Iterations).Select(_ => Uuid.NewUuid()).ToArray();
            Guids = Uuids.Select(x => (Guid)x).ToArray();
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

        [Benchmark]
        public string[] Convert_ToBase64()
        {
            for (var i = 0; i < Uuids.Length; i++)
            {
                Strings[i] = Reference.ToString(Uuids[i]);
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

        [Benchmark]
        public UuidVersion[] From_byte_array()
        {
            for (var i = 0; i < Uuids.Length; i++)
            {
                Versions[i] = Reference.GetVersion(Uuids[i]);
            }
            return Versions;
        }
    }
}
