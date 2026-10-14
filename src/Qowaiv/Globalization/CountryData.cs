namespace Qowaiv.Globalization;

internal sealed partial record CountryData(Country Country, string Iso2, string Iso3, int Iso, bool Independent, Date StartDate, Date? EndDate = null)
{
    private const bool Y = true;
    private const bool N = false;

    public RegionInfo? RegionInfo { get; } = new Lazy<RegionInfo?>(() =>
    {
        try
        {
            return new RegionInfo(Iso2);
        }
        catch
        {
            return null;
        }
    }).Value;

    [Pure]
    private static CountryLookup<CountryData> Init(params IEnumerable<CountryData> items)
    {
        var lookup = new CountryData[CountryId.MaxSize];

        foreach (var item in items)
            lookup[(int)CountryId.New(item.Country)] = item;

        return new(lookup);
    }
}
