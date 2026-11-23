using Qowaiv;
using Qowaiv.Humanizer;
using Qowaiv.Locale.NL;

namespace Specs.Locale.NL.AowLeeftijd_specs;

public class Defines
{
    public static IReadOnlyCollection<AowLeeftijd> DefinedValues() => AowLeeftijd.DefinedValues;

    [Test]
    [MethodDataSource(nameof(DefinedValues))]
    public void Vanaf_first_of_Month(AowLeeftijd aow)
        => aow.Vanaf.Day.Should().Be(1);

    [Test]
    [MethodDataSource(nameof(DefinedValues))]
    public void Tot_last_of_Month(AowLeeftijd aow)
        => aow.Tot.Day.Should().Be(Month.Create(aow.Tot.Month).Days(aow.Tot.Year.CE()));
}

public class Selects
{
    [Test]
    public void For_geboortedatum()
    {
        var aow = AowLeeftijd.Geboortedatum(new(1980, 02, 14));
        aow.Should().BeEquivalentTo(new
        {
            Vanaf = new Date(1978, 10, 01),
            Tot = new Date(1982, 06, 30),
            Leeftijd = 68.Years() + 6.Months(),
            Definitief = YesNo.No,
        });
    }
}
