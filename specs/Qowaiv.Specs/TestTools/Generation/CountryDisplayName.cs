using Qowaiv.TestTools.Resx;
using Qowaiv.TestTools.Wikipedia;


namespace Qowaiv.TestTools.Generation;

public static class CountryDisplayName
{
    public static async Task<Action> Update(
        string unknown,
        CultureInfo language,
        Func<Country, Task<string?>> getDisplay)
    {
        var resource = new XResourceFile(Data(unknown, Country.Unknown));

        foreach (var country in Country.All)
        {
            var displayName = await getDisplay(country) ?? country.GetDisplayName(language);

            if (displayName != country.EnglishName)
            {
                resource.Add(Data(displayName, country));
            }
        }
        return () => resource.Save(Solution.Root.File($"src/Qowaiv/Globalization/CountryLabels.{language}.resx"));
    }


    public static XResourceFileData Data(string displayName, Country country)
        => new(
            $"{(country.IsUnknown() ? "ZZ" : country.Name)}_DisplayName",
            displayName,
            $"{country.EnglishName} ({country.IsoAlpha2Code})");

    private const string DE_prefix = "{{{3|";

    public static string? DE(string str)
    {
        var index = str.LastIndexOf(DE_prefix);
        if (index > -1)
        {
            var text = str[(index + DE_prefix.Length)..];
            return text[..text.IndexOf('}')];
        }
        else return null;
    }

    public static string? NL(string str)
    {
        var index = str.LastIndexOf("-VLAG}}&nbsp;") + 1;
        var text = str[index..];

        if (WikiLink.Parse(text).FirstOrDefault() is { } link)
        {
            var display = link.Display;
            display = display[(display.IndexOf('|') + 1)..];
            display = display.Trim('{').Trim('}');
            return display;
        }
        else return null;
    }
}
