using System.Reflection;

namespace Qowaiv.TestTools.Globalization;

/// <summary>Contains <see cref="CultureInfo"/>'s for test purposes.</summary>
public static class TestCultures
{
    /// <summary>Selects a culture, first checking a culture defined here.</summary>
    [Pure]
    public static CultureInfo Select(string name)
        => typeof(TestCultures).GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Select(prop => (CultureInfo)prop.GetValue(null)!)
        .FirstOrDefault(culture => culture.Name == name) ?? new CultureInfo(name);

    /// <summary>Gets the German (de-DE) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo De_DE => new("de-DE");

    /// <summary>Gets the English (en) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo En => new("en");

    /// <summary>Gets the British (en-GB) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo En_GB => new("en-GB");

    /// <summary>Gets the American (en-US) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo En_US => new("en-US");

    /// <summary>Gets the Ecuadorian (es-EC) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo Es_EC => new("es-EC");

    /// <summary>Gets the Iranian (fa-IR) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo Fa_IR
    {
        get
        {
            var culture = new CultureInfo("fa-IR");
            culture.NumberFormat.PercentSymbol = "٪";
            culture.NumberFormat.PercentDecimalSeparator = ",";
            return culture;
        }
    }

    /// <summary>Gets the French (fr-FR) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo Fr_FR => new("fr-FR");

    /// <summary>Gets the Dutch (nl-NL) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo Nl_NL => new("nl-NL");

    /// <summary>Gets the Flemish (nl-BE) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo Nl_BE
    {
        get
        {
            var culture = new CultureInfo("nl-BE");
            culture.DateTimeFormat.AbbreviatedMonthNames = new[]
            {
                "jan.",
                "feb.",
                "mrt.",
                "apr.",
                "mei",
                "juni",
                "juli",
                "aug.",
                "sep.",
                "okt.",
                "nov.",
                "dec.",
                string.Empty,
            };
            return culture;
        }
    }

    /// <summary>Updates percentage symbols of the culture.</summary>
    [FluentSyntax]
    public static CultureInfo WithPercentageSymbols(this CultureInfo? culture, string percentSymbol, string perMilleSymbol)
    {
        culture ??= CultureInfo.CurrentCulture;
        var info = (NumberFormatInfo)culture.NumberFormat.Clone()!;
        var custom = new CultureInfo(culture.Name);
        info.PercentSymbol = percentSymbol;
        info.PerMilleSymbol = perMilleSymbol;
        custom.NumberFormat = info;
        return custom;
    }
}
