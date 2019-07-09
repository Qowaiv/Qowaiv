using NUnit.Framework;
using Qowaiv.TestTools;

namespace Qowaiv.Tests
{
    public class AssemblyTest
    {
        [Test]
        public void HasPublicKey()
        {
            AssemblyAssert.HasPublicKey<Date>("0024000004800000940000000602000000240000525341310004000001000100EF35DF58AA7FEC73A11E70572E6B3791601006EF3FB1C6C1F1A402BA83BB2EDC975C61E8A32D792EDB864127F0D2C67EB7A64A9D3A0CDB0B1BB37FF2D0FCFD7990304623C044439D04DAC49624CC6D7937581419D995C2689F9898EC09C941B3EB3CAB8E4FC8F90B4AE5D45AB03D691D4D1F4B68450DAD41FED46671376934B0");
        }

        [Test]
        public void ContainsSvos_None()
        {
            AssemblyAssert.ContainsSvos<Date>(
                typeof(Date),
                typeof(EmailAddress),
                typeof(Gender),
                typeof(HouseNumber),
                typeof(LocalDateTime),
                typeof(Month),
                typeof(Percentage),
                typeof(PostalCode),
                typeof(Uuid),
                typeof(WeekDate),
                typeof(Year),
                typeof(YesNo),
                typeof(Financial.Amount),
                typeof(Financial.BankIdentifierCode),
                typeof(Financial.Currency),
                typeof(Financial.InternationalBankAccountNumber),
                typeof(Financial.Money),
                typeof(Globalization.Country),
                typeof(IO.StreamSize),
                typeof(Security.Cryptography.CryptographicSeed),
                typeof(Statistics.Elo),
                typeof(Web.InternetMediaType)
            );
        }
    }
}
