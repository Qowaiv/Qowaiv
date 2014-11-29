using NUnit.Framework;
using Qowaiv.UnitTests.TestTools;
using System;
using System.Linq;
using System.Reflection;

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
			var assemblies = new Assembly[] { typeof(Qowaiv.Country).Assembly, typeof(Qowaiv.Web.InternetMediaType).Assembly };

			var svos = assemblies.SelectMany(assembly => assembly.GetTypes())
			   .Where(tp => tp.GetCustomAttributes(typeof(SingleValueObjectAttribute), false).Any())
			   .OrderBy(tp => tp.Name)
			   .OrderBy(tp => tp.Namespace)
			   .ToArray();

			var exp = new Type[]
			{
				typeof(Qowaiv.BankIdentifierCode),
				typeof(Qowaiv.Country),
				typeof(Qowaiv.Currency),
				typeof(Qowaiv.Date),
				typeof(Qowaiv.EmailAddress),
				typeof(Qowaiv.FileSize),
				typeof(Qowaiv.Gender),
				typeof(Qowaiv.HouseNumber),
				typeof(Qowaiv.InternationalBankAccountNumber),
				typeof(Qowaiv.Month),
				typeof(Qowaiv.Percentage),
				typeof(Qowaiv.PostalCode),
				typeof(Qowaiv.WeekDate),
				typeof(Qowaiv.Year),
				typeof(Qowaiv.Web.InternetMediaType)
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
