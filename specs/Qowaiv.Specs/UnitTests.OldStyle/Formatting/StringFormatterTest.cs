namespace Qowaiv.UnitTests.Formatting;

[TestFixture]
public class StringFormatterTest
{
    [Test]
    public void Apply_InvalidFormat_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            StringFormatter.Apply(int.MinValue, "\\", CultureInfo.InvariantCulture, []);
        },
        "Input string was not in a correct format.");
    }

    [Test]
    public void ToNonDiacritic_Null_AreEqual()
    {
        var str = Nil.String;

        var exp = Nil.String;
        var act = StringFormatter.ToNonDiacritic(str);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToNonDiacritic_StringEmpty_AreEqual()
    {
        var str = string.Empty;

        var exp = string.Empty;
        var act = StringFormatter.ToNonDiacritic(str);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void To_non_diacritic_Cafe_und_Straße_AreEqual()
    {
        var str = "Café & Straße";

        var exp = "Cafe & Strasze";
        var act = StringFormatter.ToNonDiacritic(str);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void To_non_diacritic_Cafe_und_Straße_ignore_e()
    {
        var str = "Café & Straße";

        var exp = "Café & Strasze";
        var act = StringFormatter.ToNonDiacritic(str, "é");

        Assert.AreEqual(exp, act);
    }
}
