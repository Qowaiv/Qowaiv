using BenchmarkDotNet.Configs;
using Specs_Generated;

namespace Bench;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[MinColumn]
public partial class GuidBenchmark(Uuid[] uuids)
{
    public GuidBenchmark() : this([.. Enumerable.Range(0, Iterations).Select(_ => Uuid.NewUuid())]) { }

    private const int Iterations = 1000;

    private readonly Guid[] Guids = [.. uuids];
    private readonly Uuid[] Uuids = [.. uuids];
    private readonly GuidBasedId[] GuidBasedIds = new GuidBasedId[Iterations];
    private readonly UuidBasedId[] UuidBasedIds = new UuidBasedId[Iterations];

    private readonly string[] Strings = [.. uuids.Select(g => g.ToString("D"))];
    private readonly string[] Base64s = [.. uuids.Select(g => g.ToString("S"))];
    private readonly string[] Base32s = [.. uuids.Select(g => g.ToString("H"))];

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
