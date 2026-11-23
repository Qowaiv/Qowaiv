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
        => aow.Van.Day.Should().Be(1);

    [Test]
    [MethodDataSource(nameof(DefinedValues))]
    public void Tot_last_of_Month(AowLeeftijd aow)
        => aow.Tot.Day.Should().Be(Month.Create(aow.Tot.Month).Days(aow.Tot.Year.CE()));
}

public class Selects
{
    [Test]
    public void For_geboortedatum_returns_entry()
    {
        var aow = AowLeeftijd.Geboortedatum(new(1980, 02, 14));
        aow.Should().BeEquivalentTo(new
        {
            Van = new Date(1978, 10, 01),
            Tot = new Date(1982, 06, 30),
            Leeftijd = 68.Years() + 6.Months(),
            Definitief = YesNo.No,
        });
    }

    [Test]
    [Arguments("1952-03-31", "65Y+9M", true)]
    [Arguments("1952-04-01", "66Y+0M", true)]
    [Arguments("1952-12-31", "66Y+0M", true)]
    [Arguments("1953-01-01", "66Y+4M", true)]
    [Arguments("1965-09-30", "67Y+3M", true)]
    [Arguments("1965-10-01", "67Y+3M", false)]
    [Arguments("2001-01-01", "70Y+0M", false)]
    public void For_geboortedatum_returns_age_and_status(Date geboortedatum, MonthSpan leeftijd, YesNo definitief)
        => AowLeeftijd.Geboortedatum(geboortedatum)
        .Should()
        .BeEquivalentTo(new
        {
            Leeftijd = leeftijd,
            Definitief = definitief,
        });
}
