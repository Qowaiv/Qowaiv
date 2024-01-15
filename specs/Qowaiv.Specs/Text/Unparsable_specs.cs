namespace Text.Unparsable_specs;

public class Communicates
{
    [Test]
    public void attempted_value_and_type()
    {
        using var _ = TestCultures.en_GB.Scoped();

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

#if NET8_0_OR_GREATER
#else
public class Is_serializable
{
    internal static readonly Exception Exception = Unparsable.ForValue<int>("fourty-two", "Not a number,").InnerException!;

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void Binary()
    {
        var roundtrip = SerializeDeserialize.Binary(Exception);
        roundtrip.Should().NotBeSameAs(Exception)
            .And.Subject.Should().BeEquivalentTo(Exception);
    }
}
#endif
