namespace Globalization.Culture_info_scope_specs;

public class Can_scope
{
    [Test]
    public void with_culture_and_ui_culture()
    {
        var current = CultureInfo.CurrentCulture;
        var currentUI = CultureInfo.CurrentUICulture;

        using (new CultureInfoScope("es-ES", "fr-FR"))
        {
            CultureInfo.CurrentCulture.Name.Should().Be("es-ES");
            CultureInfo.CurrentUICulture.Name.Should().Be("fr-FR");
        }

        CultureInfo.CurrentCulture.Should().Be(current);
        CultureInfo.CurrentUICulture.Should().Be(currentUI);
    }

    [Test]
    public void via_extension_method()
    {
        var current = CultureInfo.CurrentCulture;
        var currentUI = CultureInfo.CurrentUICulture;

        using (new CultureInfo("es-ES").Scoped())
        {
            CultureInfo.CurrentCulture.Name.Should().Be("es-ES");
            CultureInfo.CurrentUICulture.Name.Should().Be("es-ES");
        }

        CultureInfo.CurrentCulture.Should().Be(current);
        CultureInfo.CurrentUICulture.Should().Be(currentUI);
    }
}