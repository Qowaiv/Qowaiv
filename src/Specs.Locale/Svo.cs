using Qowaiv.Locale.NL;

namespace Specs.Locale;

public static class Svo
{
    /// <summary>AOW-leeftijd voor 1964-10-13</summary>
    public static AowLeeftijd AowLeeftijd => Qowaiv.Locale.NL.AowLeeftijd.Geboortedatum(new(1964, 10, 13));

    /// <summary>1234.56.782</summary>
    public static Burgerservicenummer Burgerservicenummer => 1234_56_782L.BSN();

    /// <summary>DEL00 (Delft)</summary>
    public static KadastraleGemeente KadastraleGemeente => KadastraleGemeente.Parse("DEL00");

    /// <summary>Kvk: 8764 0540</summary>
    public static KvkNummer KvkNummer => KvkNummer.Create(8764_0540);
    /// <summary>Kvk: 8764 0540</summary>
    public static Rsin RSIN => Rsin.Create(1234_56_782);
}
