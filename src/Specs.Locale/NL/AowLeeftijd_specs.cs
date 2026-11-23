using Qowaiv;
using Qowaiv.Humanizer;
using Qowaiv.Locale.NL;

namespace Specs.Locale.NL.AowLeeftijd_specs;

public class UpToDate
{
    [Test]
    public void Tot() => (Clock.Today() <= new Date(2027, 11, 01)).Should().BeTrue();
}

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
        aow.Should().Be(new AowLeeftijd(
            van: new(1978, 10, 01),
            tot: new(1982, 06, 30),
            leeftijd:  68.Years() + 6.Months(),
            definitief: YesNo.No));
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

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.AowLeeftijd.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.AowLeeftijd.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.AowLeeftijd.Equals(AowLeeftijd.Geboortedatum(new(1944, 10, 13))).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.AowLeeftijd.Equals(AowLeeftijd.Geboortedatum(new(1964, 10, 13))).Should().BeTrue();

    [Test]
    public void hash_code_is_value_based()
        => HashCodes.For(AowLeeftijd.DefinedValues).Should().BeConsistent();
}

public class DebuggerExperience
{
    [Test]
    [Arguments("70Y+0M", true, "[2000-01, 2000-11] = 70 jaar")]
    [Arguments("70Y+1M", true, "[2000-01, 2000-11] = 70 jaar en 1 maand")]
    [Arguments("70Y+4M", false, "[2000-01, 2000-11] = 70 jaar en 4 maanden*")]
    public void Displays(MonthSpan leeftijd, YesNo definitief, string display)
        => new AowLeeftijd(new(2000, 01, 01), new(2000, 11, 30), leeftijd, definitief)
        .Should().HaveDebuggerDisplay(display);
}
