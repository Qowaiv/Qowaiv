namespace Financial.Currencies_specs;

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
