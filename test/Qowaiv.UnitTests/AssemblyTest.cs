using Qowaiv.TestTools;
using System;

namespace Qowaiv.Tests
{
    public class AssemblyTest : AssemblyTestBase
    {
        protected override Type AssemblyType => typeof(Date);

        protected override Type[] Svos => new[]
        {
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
        };
    }
}
