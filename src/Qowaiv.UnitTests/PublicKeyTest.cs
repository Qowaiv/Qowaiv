using NUnit.Framework;
using System;

namespace Qowaiv.UnitTests
{
	[TestFixture]
	public class PublicKeyTest
	{
		const string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100EF35DF58AA7FEC73A11E70572E6B3791601006EF3FB1C6C1F1A402BA83BB2EDC975C61E8A32D792EDB864127F0D2C67EB7A64A9D3A0CDB0B1BB37FF2D0FCFD7990304623C044439D04DAC49624CC6D7937581419D995C2689F9898EC09C941B3EB3CAB8E4FC8F90B4AE5D45AB03D691D4D1F4B68450DAD41FED46671376934B0";
		[Test]
		public void GetPublicKey_Qowaiv_320CharLongKey()
		{
			AssertPublicKey(typeof(Date), PublicKey);
		}

		[Test]
		public void GetPublicKey_QowaivJsonNewtonsoft_320CharLongKey()
		{
			AssertPublicKey(typeof(Qowaiv.Json.QowaivJsonConverter), PublicKey);
		}

		[Test]
		public void GetPublicKey_QowaivWeb_320CharLongKey()
		{
			AssertPublicKey(typeof(Qowaiv.Web.InternetMediaType), PublicKey);
		}

		private static void AssertPublicKey(Type tp, string exp)
		{
			var key = tp.Assembly.GetName().GetPublicKey();
			var act = BitConverter.ToString(key).Replace("-", "");

			Console.WriteLine(act);
			Assert.AreEqual(exp, act);
		}
	}
}
