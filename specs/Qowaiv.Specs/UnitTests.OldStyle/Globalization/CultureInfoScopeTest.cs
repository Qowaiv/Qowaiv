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
            Assert.AreEqual("es-ES", CultureInfo.CurrentCulture.Name);
            Assert.AreEqual("fr-FR", CultureInfo.CurrentUICulture.Name);
        }

        Assert.AreEqual(current, CultureInfo.CurrentCulture);
        Assert.AreEqual(currentUI, CultureInfo.CurrentUICulture);
    }

    [Test]
    public void Scoped_ExtensionMethod()
    {
        var current = CultureInfo.CurrentCulture;
        var currentUI = CultureInfo.CurrentUICulture;

        using (new CultureInfo("es-ES").Scoped())
        {
            Assert.AreEqual("es-ES", CultureInfo.CurrentCulture.Name);
            Assert.AreEqual("es-ES", CultureInfo.CurrentUICulture.Name);
        }

        Assert.AreEqual(current, CultureInfo.CurrentCulture);
        Assert.AreEqual(currentUI, CultureInfo.CurrentUICulture);
    }
}
