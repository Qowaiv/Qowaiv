using Qowaiv.TestTools.Resx;

namespace Qowaiv.TestTools.Generation;

public sealed record CountryData(string Name)
{
    public string DisplayName { get; init; } = string.Empty;
    public int ISO { get; init; }
    public string ISO2 { get; init; } = string.Empty;
    public string ISO3 { get; init; } = string.Empty;
    public Date StartDate { get; init; } = new Date(1974, 01, 01);
    public Date? EndDate { get; init; }
    public string? CallingCode { get; init; }

    public IEnumerable<XResourceFileData> Data()
    {
        var pre = Name + '_';
        yield return new(pre + nameof(DisplayName), DisplayName);
        yield return new(pre + nameof(ISO), ISO.ToString("000"));
        yield return new(pre + nameof(ISO2), ISO2);
        yield return new(pre + nameof(ISO3), ISO3);
        yield return new(pre + nameof(StartDate), StartDate.ToString("yyyy-MM-dd"));
        if (EndDate is { } endDate) yield return new(pre + nameof(EndDate), endDate.ToString("yyyy-MM-dd"));
        if (CallingCode is { Length: > 0 }) yield return new(pre + nameof(CallingCode), CallingCode);
    }
}
