using Qowaiv.TestTools.Resx;
using Qowaiv.TestTools.Wikipedia;


namespace Qowaiv.TestTools.Generation;

public static class CountryDisplayName
{
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
