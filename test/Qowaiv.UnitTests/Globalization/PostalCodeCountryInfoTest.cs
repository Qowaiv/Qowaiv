using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.UnitTests.TestTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.UnitTests.Globalization
{
	[TestFixture]
	public class PostalCodeCountryInfoTest
	{
		[Test]
		public void GetCountriesWithoutPostalCode_None_77Countries()
		{
			var exp = new Country[] { Country.AE, Country.AG, Country.AO, Country.AQ, Country.AW, Country.BF, Country.BI, Country.BJ, Country.BQ, Country.BS, Country.BV, Country.BW, Country.BZ, Country.CD, Country.CF, Country.CG, Country.CI, Country.CK, Country.CM, Country.CW, Country.DJ, Country.DM, Country.DO, Country.EH, Country.ER, Country.FJ, Country.GD, Country.GH, Country.GM, Country.GN, Country.GQ, Country.GY, Country.HK, Country.IE, Country.JM, Country.KE, Country.KI, Country.KM, Country.KN, Country.KP, Country.KW, Country.LC, Country.ML, Country.MO, Country.MR, Country.MS, Country.MU, Country.MV, Country.MW, Country.NR, Country.NU, Country.QA, Country.RW, Country.SB, Country.SC, Country.SJ, Country.SL, Country.SO, Country.SR, Country.SS, Country.ST, Country.SX, Country.SY, Country.TF, Country.TG, Country.TK, Country.TL, Country.TO, Country.TV, Country.TZ, Country.UG, Country.UM, Country.UZ, Country.VU, Country.WS, Country.YE, Country.ZW };
			var act = PostalCodeCountryInfo.GetCountriesWithoutPostalCode().ToArray();

			foreach (var item in act)
			{
				Console.WriteLine(item);
			}

			Assert.AreEqual(exp.Length, act.Length, "act.Length");

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetCountriesWithFormatting_None_59Countries()
		{
			var exp = new Country[] { Country.AD, Country.AI, Country.AR, Country.AS, Country.AX, Country.AZ, Country.BB, Country.BM, Country.BR, Country.CA, Country.CL, Country.CU, Country.CZ, Country.DK, Country.FI, Country.FK, Country.FM, Country.FO, Country.GA, Country.GB, Country.GG, Country.GI, Country.GL, Country.GR, Country.GS, Country.GU, Country.IM, Country.IO, Country.IR, Country.JE, Country.JP, Country.KR, Country.KY, Country.LB, Country.LT, Country.LV, Country.MA, Country.MC, Country.MD, Country.MH, Country.MP, Country.MT, Country.NL, Country.PL, Country.PN, Country.PT, Country.PW, Country.SA, Country.SE, Country.SH, Country.SI, Country.SK, Country.SN, Country.TC, Country.US, Country.VC, Country.VE, Country.VG, Country.VI };
			var act = PostalCodeCountryInfo.GetCountriesWithFormatting().ToArray();

			foreach (var item in act)
			{
				Console.WriteLine(item);
			}

			Assert.AreEqual(exp.Length, act.Length, "act.Length");

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetCountriesWithSingleValue_None_10Countries()
		{
			var exp = new Dictionary<Country, string>()
			{
				{ Country.AI, "AI-2640" },
				{ Country.FK, "FIQQ 1ZZ" },
				{ Country.GI, "GX11 1AA" },
				{ Country.GS, "SIQQ 1ZZ" },
				{ Country.IO, "BBND 1ZZ" },
				{ Country.PN, "PCRN 1ZZ" },
				{ Country.SH, "STHL 1ZZ" },
				{ Country.SV, "01101" },
				{ Country.TC, "TKCA 1ZZ" },
				{ Country.VA, "00120" },

			};
			var act = PostalCodeCountryInfo.GetCountriesWithSingleValue().ToArray();

			foreach (var item in act)
			{
				Console.WriteLine(item);
			}

			Assert.AreEqual(exp.Keys.Count, act.Length, "act.Length");

			CollectionAssert.AreEqual(exp.Keys, act);

			foreach (var kvp in exp)
			{
				var info = PostalCodeCountryInfo.GetInstance(kvp.Key);

				Assert.AreEqual(kvp.Value, info.GetSingleValue(), "GetSingleValue(), {0}.", kvp.Key);
			}
		}

		[Test]
		public void GetSingleValue_Belgium_StringEmpty()
		{
			var info = PostalCodeCountryInfo.GetInstance(Country.BE);
			
			var act = info.GetSingleValue();
			var exp = string.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_Empty_AreEqual()
		{
			DebuggerDisplayAssert.HasResult("Postal code[], none", PostalCodeCountryInfo.GetInstance(Country.Empty));
		}

		[Test]
		public void DebuggerDisplay_BE_AreEqual()
		{
			DebuggerDisplayAssert.HasResult("Postal code[BE], Pattern: ^[1-9][0-9]{3}$", PostalCodeCountryInfo.GetInstance(Country.BE));
		}

		[Test]
		public void DebuggerDisplay_CA_AreEqual()
		{
			DebuggerDisplayAssert.HasResult("Postal code[CA], Pattern: ^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$, Format: $1 $2", PostalCodeCountryInfo.GetInstance(Country.CA));
		}

		[Test]
		public void DebuggerDisplay_VA_AreEqual()
		{
			DebuggerDisplayAssert.HasResult("Postal code[VA], Value: 00120", PostalCodeCountryInfo.GetInstance(Country.VA));
		}
	}
}
