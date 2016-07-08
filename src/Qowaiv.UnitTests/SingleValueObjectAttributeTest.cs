using NUnit.Framework;
using Qowaiv.Reflection;
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
			var act = new SingleValueObjectAttribute(SingleValueStaticOptions.All, typeof(string));

			Assert.AreEqual(SingleValueStaticOptions.All, act.StaticOptions, "act.StaticOptions");
			Assert.AreEqual(typeof(string), act.UnderlyingType, "act.UnderlyingType");
		}

		[Test]
		public void Analize_AllSvos_MatchAttribute()
		{
			var assemblies = new[] { typeof(Country).Assembly, typeof(Qowaiv.Web.InternetMediaType).Assembly };

			var svos = assemblies.SelectMany(assembly => assembly.GetTypes())
			   .Where(tp => QowaivType.IsSingleValueObject(tp))
			   .OrderBy(tp => tp.Namespace)
			   .ThenBy(tp => tp.Name)
			   .ToArray();

			var exp = new []
			{
				typeof(Country),
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
				typeof(Qowaiv.Financial.Amount),
				typeof(Qowaiv.Financial.BankIdentifierCode),
				typeof(Qowaiv.Financial.Currency),
				typeof(Qowaiv.Financial.InternationalBankAccountNumber),
				typeof(Qowaiv.IO.StreamSize),
				typeof(Qowaiv.Security.Cryptography.CryptographicSeed),
				typeof(Qowaiv.Sql.Timestamp),
				typeof(Qowaiv.Statistics.Elo),
				typeof(Qowaiv.Web.InternetMediaType)
			};

			foreach (var svo in svos)
			{
				Console.WriteLine(svo);
			}
					
			CollectionAssert.AreEqual(exp, svos);

			foreach (var svo in svos)
			{
				var attr = QowaivType.GetSingleValueObjectAttribute(svo);

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
