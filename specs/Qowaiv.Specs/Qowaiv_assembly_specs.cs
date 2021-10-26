﻿using FluentAssertions;
using NUnit.Framework;

namespace Qowaiv_assembly_specs

{
    public class Signed
    {
        [TestCase("0024000004800000940000000602000000240000525341310004000001000100EF35DF58AA7FEC73A11E70572E6B3791601006EF3FB1C6C1F1A402BA83BB2EDC975C61E8A32D792EDB864127F0D2C67EB7A64A9D3A0CDB0B1BB37FF2D0FCFD7990304623C044439D04DAC49624CC6D7937581419D995C2689F9898EC09C941B3EB3CAB8E4FC8F90B4AE5D45AB03D691D4D1F4B68450DAD41FED46671376934B0")]
        public void with_key(string key)
            => typeof(Qowaiv.SingleValueObjectAttribute).Assembly.Should().HavePublicKey(key);
    }
}