namespace Financial.Currencies_specs;

public class All
{
    [Test]
    public void get_existing_by_date()
        => Currency.GetExisting(new Date(2016, 01, 01)).Should().HaveCount(179);

#if NET6_0_OR_GREATER
    [Test]
    public void get_existing_by_date_only()
        => Currency.GetExisting(new DateOnly(2016, 01, 01)).Should().HaveCount(179);
#endif
}
