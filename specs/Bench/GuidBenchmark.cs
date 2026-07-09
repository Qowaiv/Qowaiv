using BenchmarkDotNet.Configs;
using Specs_Generated;

namespace Bench;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[MinColumn]
public partial class GuidBenchmark
{
    private const int Iterations = 1000;
    private Guid[] Guids = new Guid[Iterations];
    private Uuid[] Uuids = new Uuid[Iterations];
    private GuidBasedId[] GuidBasedIds = new GuidBasedId[Iterations];
    private UuidBasedId[] UuidBasedIds = new UuidBasedId[Iterations];
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

    [BenchmarkCategory("Parse()")]
    [Benchmark(Description = "System.Guid", Baseline = true)]
    public Guid[] GUID_Parse()
    {
        for (var i = 0; i < Strings.Length; i++)
            Guids[i] = Guid.Parse(Strings[i]);

        return Guids;
    }

    [BenchmarkCategory("Parse()")]
    [Benchmark(Description = "UUID.Base16")]
    public Uuid[] UUID_Parse()
    {
        for (var i = 0; i < Strings.Length; i++)
            Uuids[i] = Uuid.Parse(Strings[i]);

        return Uuids;
    }

    [BenchmarkCategory("Parse()")]
    [Benchmark(Description = "UUID.Base64")]
    public Uuid[] UUID_Parse_Base64()
    {
        for (var i = 0; i < Strings.Length; i++)
            Uuids[i] = Uuid.Parse(Base64s[i]);

        return Uuids;
    }

    [BenchmarkCategory("Parse()")]
    [Benchmark(Description = "UUID.Base32")]
    public Uuid[] UUID_Parse_Base32()
    {
        for (var i = 0; i < Strings.Length; i++)
            Uuids[i] = Uuid.Parse(Base32s[i]);

        return Uuids;
    }

    [BenchmarkCategory("Parse()")]
    [Benchmark(Description = "GUID based ID")]
    public GuidBasedId[] GUID_Based_Id_Parse()
    {
        for (var i = 0; i < Strings.Length; i++)
            GuidBasedIds[i] = GuidBasedId.Parse(Base32s[i]);

        return GuidBasedIds;
    }

    [BenchmarkCategory("Parse()")]
    [Benchmark(Description = "UUID based ID")]
    public UuidBasedId[] UUID_Based_Id_Parse()
    {
        for (var i = 0; i < Strings.Length; i++)
            UuidBasedIds[i] = UuidBasedId.Parse(Base32s[i]);

        return UuidBasedIds;
    }

    [BenchmarkCategory("ToString()")]
    [Benchmark(Description = "System.Guid", Baseline = true)]
    public string[] GUID_ToString()
    {
        for (var i = 0; i < Guids.Length; i++)
            Strings[i] = Guids[i].ToString("D");

        return Strings;
    }

    [BenchmarkCategory("ToString()")]
    [Benchmark(Description = "UUID.Base16")]
    public string[] GUID()
    {
        for (var i = 0; i < Uuids.Length; i++)
            Strings[i] = Uuids[i].ToString("D");

        return Strings;
    }

    [BenchmarkCategory("ToString()")]
    [Benchmark(Description = "UUID.Base64")]
    public string[] Base64()
    {
        for (var i = 0; i < Uuids.Length; i++)
            Strings[i] = Uuids[i].ToJson()!;

        return Strings;
    }

    [BenchmarkCategory("ToString()")]
    [Benchmark(Description = "UUID.Base32")]
    public string[] Base32()
    {
        for (var i = 0; i < Uuids.Length; i++)
            Strings[i] = Uuids[i].ToString("H");

        return Strings;
    }

    [BenchmarkCategory("ToString()")]
    [Benchmark(Description = "GUID based ID")]
    public string[] GuidBasedId_ToString()
    {
        for (var i = 0; i < GuidBasedIds.Length; i++)
            Strings[i] = GuidBasedIds[i].ToString();

        return Strings;
    }

    [BenchmarkCategory("ToString()")]
    [Benchmark(Description = "UUID based ID")]
    public string[] UuidBasedId_ToString()
    {
        for (var i = 0; i < UuidBasedIds.Length; i++)
            Strings[i] = UuidBasedIds[i].ToString();

        return Strings;
    }
}
