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

    /// <summary>Gets the Arabic (ar) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo ar => new("ar");

    /// <summary>Gets the German (de) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo de => new("de");

    /// <summary>Gets the English (en) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo en => new("en");

    /// <summary>Gets the Spanish (es) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo es => new("es");

    /// <summary>Gets the French (fr) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo fr => new("fr");

    /// <summary>Gets the Italian (it) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo it => new("it");

    /// <summary>Gets the Japanese (ja) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo ja => new("ja");

    /// <summary>Gets the Dutch (nl) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo nl => new("nl");

    /// <summary>Gets the Portuguese (pt) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo pt => new("pt");

    /// <summary>Gets the Russian (ru) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo ru => new("ru");

    /// <summary>Gets the Chinese (zh) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo zh => new("zh");

    /// <summary>Gets the German (de-DE) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo de_DE => new("de-DE");

    /// <summary>Gets the British (en-GB) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo en_GB => new("en-GB");

    /// <summary>Gets the American (en-US) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo en_US => new("en-US");

    /// <summary>Gets the Ecuadorian (es-EC) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo es_EC => new("es-EC");

    /// <summary>Gets the Iranian (fa-IR) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo fa_IR
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
    public static CultureInfo fr_FR => new("fr-FR");

    /// <summary>Gets the Dutch (nl-NL) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo nl_NL => new("nl-NL");

    /// <summary>Gets the Flemish (nl-BE) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo nl_BE
    {
        get
        {
            var culture = new CultureInfo("nl-BE");
            culture.DateTimeFormat.AbbreviatedMonthNames =
            [
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
            ];
            return culture;
        }
    }

    /// <summary>Gets the Portuguese (pt-PT) <see cref="CultureInfo"/>.</summary>
    public static CultureInfo pt_PT => new("pt-PT");

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
