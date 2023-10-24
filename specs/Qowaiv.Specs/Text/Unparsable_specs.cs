namespace Text.Unparsable_specs;

public class Communicates
{
    [Test]
    public void attempted_value_and_type()
    {
        using var _ = TestCultures.En_GB.Scoped();
        
        "no_guid".Invoking(CustomGuid.Parse)
            .Should().Throw<FormatException>()
            .WithMessage("Not a valid identifier.")
            .And.InnerException.Should().BeOfType<Unparsable>()
            .And.Subject.Should().BeEquivalentTo(new 
            {
                Value = "no_guid",
                Type = "Qowaiv.Identifiers.Id<Qowaiv.TestTools.ForGuid>"
            });
    }
}
