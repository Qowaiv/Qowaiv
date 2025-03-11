namespace Qowaiv.UnitTests.Globalization;

public class CultureInfoScopeTest
{
    [Test]
    public void Scoped_Ctor()
    {
        var current = CultureInfo.CurrentCulture;
        var currentUI = CultureInfo.CurrentUICulture;

        using (new CultureInfoScope("es-ES", "fr-FR"))
        {
            Should.BeEqual("es-ES", CultureInfo.CurrentCulture.Name);
            Should.BeEqual("fr-FR", CultureInfo.CurrentUICulture.Name);
        }

        CultureInfo.CurrentCulture.Should().Be(current);
        CultureInfo.CurrentUICulture.Should().Be(currentUI);
    }

    [Test]
    public void Scoped_ExtensionMethod()
    {
        var current = CultureInfo.CurrentCulture;
        var currentUI = CultureInfo.CurrentUICulture;

        using (new CultureInfo("es-ES").Scoped())
        {
            Should.BeEqual("es-ES", CultureInfo.CurrentCulture.Name);
            Should.BeEqual("es-ES", CultureInfo.CurrentUICulture.Name);
        }

        CultureInfo.CurrentCulture.Should().Be(current);
        CultureInfo.CurrentUICulture.Should().Be(currentUI);
    }
}
