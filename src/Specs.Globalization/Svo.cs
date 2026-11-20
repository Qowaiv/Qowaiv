using Qowaiv.Globalization.NL;

namespace Specs.Globalization;

public static class Svo
{
    /// <summary>1234.56.782</summary>
    public static Burgerservicenummer Burgerservicenummer => 1234_56_782L.BSN();

    /// <summary>DEL00 (Delft)</summary>
    public static KadastraleGemeente KadastraleGemeente => KadastraleGemeente.Parse("DEL00");

    /// <summary>Kvk: 8764 0540</summary>
    public static KvkNummer KvkNummer => KvkNummer.Create(8764_0540);
}
