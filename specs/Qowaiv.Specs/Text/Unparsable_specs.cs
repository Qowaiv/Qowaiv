namespace Text.Unparsable_specs;

public class Communicates
{
    [Test]
    public void attempted_value_and_type()
    {
        using var _ = TestCultures.en_GB.Scoped();

        "no_guid".Invoking(Uuid.Parse)
            .Should().Throw<FormatException>()
            .WithMessage("Not a valid GUID")
            .And.InnerException.Should().BeOfType<Unparsable>()
            .And.Subject.Should().BeEquivalentTo(new
            {
                Value = "no_guid",
                Type = "Qowaiv.Uuid"
            });
    }
}
