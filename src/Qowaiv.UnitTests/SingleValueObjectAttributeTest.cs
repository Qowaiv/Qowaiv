using NUnit.Framework;
using Qowaiv.UnitTests.TestTools;
using System;
using System.Linq;

namespace Qowaiv.UnitTests
{
    [TestFixture]
    public class SingleValueObjectAttributeTest
    {
        [Test]
        public void Ctor_Params_AreEqual()
        {
            var act = new SingleValueObjectAttribute(SingleValueStaticOptions.All, typeof(String));

            Assert.AreEqual(SingleValueStaticOptions.All, act.StaticOptions, "act.StaticOptions");
            Assert.AreEqual(typeof(String), act.UnderlyingType, "act.UnderlyingType");
        }

        [Test]
        public void Analize_AllSvos_MatchAttribute()
        {
            var svos = typeof(Qowaiv.Country).Assembly.GetTypes()
               .Where(tp => tp.GetCustomAttributes(typeof(SingleValueObjectAttribute), false).Any())
               .ToArray();

            var exp = new Type[]
            {
                typeof(BankIdentifierCode),
                typeof(Country),
				typeof(Date),
                typeof(EmailAddress),
                typeof(Gender),
                typeof(HouseNumber),
                typeof(InternationalBankAccountNumber),
                typeof(Month),
                typeof(Percentage),
                typeof(PostalCode),
                typeof(WeekDate),
                typeof(Year)
            };

            foreach (var svo in svos)
            {
                Console.WriteLine(svo);
            }

            Assert.AreEqual(exp.Length, svos.Length);

            CollectionAssert.AreEqual(exp, svos);

            foreach (var svo in svos)
            {
                var attr = (SingleValueObjectAttribute)svo.GetCustomAttributes(typeof(SingleValueObjectAttribute), false).First();

                SvoAssert.UnderlyingTypeMatches(svo, attr);
                SvoAssert.ParseMatches(svo, attr);
                SvoAssert.TryParseMatches(svo, attr);
                SvoAssert.IsValidMatches(svo, attr);
                SvoAssert.EmptyAndUnknownMatches(svo, attr);
                SvoAssert.ImplementsInterfaces(svo);
                SvoAssert.AttributesMatches(svo);
            }
        }
    }
}
