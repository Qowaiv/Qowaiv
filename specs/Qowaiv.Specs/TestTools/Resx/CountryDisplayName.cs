namespace Qowaiv.TestTools.Resx;

public static class CountryDisplayName
{
    public static XResourceFileData Data(string displayName, Country country)
        => new(
            $"{(country.IsUnknown() ? "ZZ" : country.Name)}_DisplayName",
            displayName,
            $"{country.EnglishName} ({country.IsoAlpha2Code})");
}
