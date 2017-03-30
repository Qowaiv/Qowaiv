using Qowaiv.Globalization;
using System;
using System.Globalization;
using System.Linq;

namespace Test
{
	class Program
	{
		public static void Main()
		{
			Culture.Register();


			var framework = CultureInfo.GetCultures(CultureTypes.FrameworkCultures);
			var all = CultureInfo.GetCultures(CultureTypes.AllCultures);
			var win32 = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures);
			var windows = CultureInfo.GetCultures(CultureTypes.WindowsOnlyCultures);
			var replaced = CultureInfo.GetCultures(CultureTypes.ReplacementCultures);
			var neutral = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
			var specific = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			var custom = CultureInfo.GetCultures(CultureTypes.UserCustomCulture);

			CultureAndRegionInfoBuilder cib = null;
			RegionInfo region;
			try
			{
				var cs = Country.All.Where(c => !c.EndDate.HasValue).ToList();

				var country = Country.AQ;
				region = new RegionInfo(country.Name);

				//foreach (var c in cs)
				//{

				//	try
				//	{
				//		region = new RegionInfo(c.Name);
				//	}
				//	catch
				//	{
				//		country = c;
				//		break;
				//	}
				//}

				CultureAndRegionInfoBuilder.Unregister("en-AQ");

				cib = new CultureAndRegionInfoBuilder("dk-" + country.Name, CultureAndRegionModifiers.None);
				cib.LoadDataFromRegionInfo(new RegionInfo("DK"));
				cib.LoadDataFromCultureInfo(new CultureInfo("dk-DK"));
				cib.RegionEnglishName = country.EnglishName;
				cib.ThreeLetterISORegionName = country.IsoAlpha3Code;
				cib.TwoLetterISORegionName = country.IsoAlpha2Code;
				cib.GeoId = country.IsoNumericCode;
				cib.Register();

				region = new RegionInfo(country.Name);
				//cib = new CultureAndRegionInfoBuilder("en-NL", CultureAndRegionModifiers.None);

				//	// Create a CultureAndRegionInfoBuilder object named "x-en-US-sample".
				//	Console.WriteLine("Create and explore the CultureAndRegionInfoBuilder...\n");
				//	cib = new CultureAndRegionInfoBuilder("en-NL", CultureAndRegionModifiers.None);

				//	// Populate the new CultureAndRegionInfoBuilder object with culture information.
				//	CultureInfo ci = new CultureInfo("en-US");
				//	cib.LoadDataFromCultureInfo(ci);

				//	// Populate the new CultureAndRegionInfoBuilder object with region information.
				//	RegionInfo ri = new RegionInfo("NL");
				//	cib.LoadDataFromRegionInfo(ri);

				//	// Display some of the properties of the CultureAndRegionInfoBuilder object.
				//	Console.WriteLine("CultureName:. . . . . . . . . . {0}", cib.CultureName);
				//	Console.WriteLine("CultureEnglishName: . . . . . . {0}", cib.CultureEnglishName);
				//	Console.WriteLine("CultureNativeName:. . . . . . . {0}", cib.CultureNativeName);
				//	Console.WriteLine("GeoId:. . . . . . . . . . . . . {0}", cib.GeoId);
				//	Console.WriteLine("IsMetric: . . . . . . . . . . . {0}", cib.IsMetric);
				//	Console.WriteLine("ISOCurrencySymbol:. . . . . . . {0}", cib.ISOCurrencySymbol);
				//	Console.WriteLine("RegionEnglishName:. . . . . . . {0}", cib.RegionEnglishName);
				//	Console.WriteLine("RegionName: . . . . . . . . . . {0}", cib.RegionName);
				//	Console.WriteLine("RegionNativeName: . . . . . . . {0}", cib.RegionNativeName);
				//	Console.WriteLine("ThreeLetterISOLanguageName: . . {0}", cib.ThreeLetterISOLanguageName);
				//	Console.WriteLine("ThreeLetterISORegionName: . . . {0}", cib.ThreeLetterISORegionName);
				//	Console.WriteLine("ThreeLetterWindowsLanguageName: {0}", cib.ThreeLetterWindowsLanguageName);
				//	Console.WriteLine("ThreeLetterWindowsRegionName: . {0}", cib.ThreeLetterWindowsRegionName);
				//	Console.WriteLine("TwoLetterISOLanguageName: . . . {0}", cib.TwoLetterISOLanguageName);
				//	Console.WriteLine("TwoLetterISORegionName: . . . . {0}", cib.TwoLetterISORegionName);
				//	Console.WriteLine();

				//	// Register the custom culture.
				//	Console.WriteLine("Register the custom culture...");
				//	cib.Register();

				//	// Display some of the properties of the custom culture.
				//	Console.WriteLine("Create and explore the custom culture...\n");
				//	ci = new CultureInfo("x-en-US-sample");

				//	Console.WriteLine("Name: . . . . . . . . . . . . . {0}", ci.Name);
				//	Console.WriteLine("EnglishName:. . . . . . . . . . {0}", ci.EnglishName);
				//	Console.WriteLine("NativeName: . . . . . . . . . . {0}", ci.NativeName);
				//	Console.WriteLine("TwoLetterISOLanguageName: . . . {0}", ci.TwoLetterISOLanguageName);
				//	Console.WriteLine("ThreeLetterISOLanguageName: . . {0}", ci.ThreeLetterISOLanguageName);
				//	Console.WriteLine("ThreeLetterWindowsLanguageName: {0}", ci.ThreeLetterWindowsLanguageName);

				//	Console.WriteLine("\nNote:\n" +
				//		"Use the example in the Unregister method topic to remove the custom culture.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			Console.ReadLine();
		}
	}
}
