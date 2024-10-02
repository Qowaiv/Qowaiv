namespace TestTools.Should.Have_public_key_specs;

public class Detects
{
    [Test]
    public void public_key()
        => typeof(Qowaiv.DateSpan).Assembly.Should().HavePublicKey(
            "0024000004800000940000000602000000240000525341310004000001000100EF35DF58AA7FEC73A11E70572E6B3791601006EF3FB1C6C1F1A402BA83BB2EDC975C61E8A32D792EDB864127F0D2C67EB7A64A9D3A0CDB0B1BB37FF2D0FCFD7990304623C044439D04DAC49624CC6D7937581419D995C2689F9898EC09C941B3EB3CAB8E4FC8F90B4AE5D45AB03D691D4D1F4B68450DAD41FED46671376934B0");
}

public class Fails_on
{
    [Test]
    public void different_value()
        => typeof(Qowaiv.DateSpan).Assembly
        .Invoking(a => a.Should().HavePublicKey("wrong value"))
        .Should().Throw<AssertionException>()
        .WithMessage("Expected '*' to have public key: *");

    [Test]
    public void missing_public_key()
        => GetType().Assembly
        .Invoking(a => a.Should().HavePublicKey("wrong value"))
        .Should().Throw<AssertionException>()
        .WithMessage("Expected '*' to have public key: *");
}
