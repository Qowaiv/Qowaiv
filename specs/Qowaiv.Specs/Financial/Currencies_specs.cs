namespace Financial.Currencies_specs;

public class Can_be_parsed
{
    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(Currency.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid currency");
        }
    }
}

public class All
{
    [Test]
    public void get_existing_by_Date()
        => Currency.GetExisting(new Date(2016, 01, 01)).Should().HaveCount(179);

#if NET8_0_OR_GREATER
    [Test]
    public void get_existing_by_DateOnly()
        => Currency.GetExisting(new DateOnly(2016, 01, 01)).Should().HaveCount(179);
#endif
}
