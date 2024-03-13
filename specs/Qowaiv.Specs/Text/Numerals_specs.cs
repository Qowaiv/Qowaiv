namespace Text.Numerals_specs;

public class To_Numeral
{
    [TestCase(-1, "min een")]
    [TestCase(0, "nul")]
    [TestCase(1, "een")]
    [TestCase(12, "twaalf")]
    [TestCase(20, "twintig")]
    [TestCase(22, "tweeëntwintig")]
    [TestCase(23, "drieëntwintig")]
    [TestCase(88, "achtentachtig")]
    [TestCase(100, "honderd")]
    [TestCase(221, "tweehonderd eenentwintig")]
    [TestCase(1000, "duizend")]
    [TestCase(2017, "tweeduizend zeventien")]
    [TestCase(long.MinValue, "min negen triljoen tweehonderd drieëntwintig biljard driehonderd tweeënzeventig biljoen zesendertig miljard achthonderd vierenvijftig miljoen zevenhonderd vijfenzeventigduizend achthonderd acht")]
    public void Dutch(long number, string words) => number.ToNumeral(TestCultures.nl_NL).Should().Be(words);

    [TestCase(-1, "minus one")]
    [TestCase(0, "zero")]
    [TestCase(1, "one")]
    [TestCase(12, "twelve")]
    [TestCase(20, "twenty")]
    [TestCase(23, "twenty-three")]
    [TestCase(100, "one hundred")]
    [TestCase(1000, "one thousand")]
    [TestCase(221, "two hundred twenty-one")]
    [TestCase(456, "four hundred fifty-six")]
    [TestCase(999000, "nine hundred and ninety-nine thousand")]
    [TestCase(2000407123, "two billion four hundred seven thousand one hundred twenty-three")]
    [TestCase(long.MinValue, "minus nine quintillion two hundred twenty-three quadrillion three hundred seventy-two trillion thirty-six billion eight hundred fifty-four million seven hundred seventy-five thousand eight hundred eight")]
    public void British_English(long number, string words) => number.ToNumeral(TestCultures.en_GB).Should().Be(words);

    [TestCase(-1, "minus one")]
    [TestCase(0, "zero")]
    [TestCase(1, "one")]
    [TestCase(12, "twelve")]
    [TestCase(20, "twenty")]
    [TestCase(23, "twenty-three")]
    [TestCase(100, "one hundred")]
    [TestCase(1000, "one thousand")]
    [TestCase(221, "two hundred twenty-one")]
    [TestCase(456, "four hundred fifty-six")]
    [TestCase(999000, "nine hundred ninety-nine thousand")]
    [TestCase(2000407123, "two billion four hundred seven thousand one hundred twenty-three")]
    [TestCase(long.MinValue, "minus nine quintillion two hundred twenty-three quadrillion three hundred seventy-two trillion thirty-six billion eight hundred fifty-four million seven hundred seventy-five thousand eight hundred eight")]
    public void American_English(long number, string words) => number.ToNumeral(TestCultures.en_US).Should().Be(words);
}
