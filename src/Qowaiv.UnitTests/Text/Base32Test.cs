using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Text;
using System;

namespace Qowaiv.UnitTests.Text
{
	[TestFixture]
	public class Base32Test
	{
		[Test]
		public void ToString_Null_StringEmpty()
		{
			var act = Base32.ToString(null);
			Assert.AreEqual(string.Empty, act);
		}
		[Test]
		public void ToString_EmptyArray_StringEmpty()
		{
			var act = Base32.ToString(new byte[0]);
			Assert.AreEqual(string.Empty, act);
		}

		[Test]
		public void ToString_0B_AA()
		{
			var act = Base32.ToString(new byte[1]);
			var exp = "AA";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_0B1B_AAAQ()
		{
			var act = Base32.ToString(new byte[] { 0, 1 });
			var exp = "AAAQ";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_0B1B2B_AAAQE()
		{
			var act = Base32.ToString(new byte[] { 0, 1, 2 });
			var exp = "AAAQE";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_0B1B2B3B_AAAQEAY()
		{
			var act = Base32.ToString(new byte[] { 0, 1, 2, 3 });
			var exp = "AAAQEAY";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_0B1B2B3B4B_AAAQEAYE()
		{
			var act = Base32.ToString(new byte[] { 0, 1, 2, 3, 4 });
			var exp = "AAAQEAYE";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_131B172B4B_StringEmpty()
		{
			var act = Base32.ToString(new byte[] { 131, 172, 004 });
			var exp = "QOWAI";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_ByteArray_TheQuickBrownFoxJumbsOverTheLazyDog2345674()
		{
			var act = Base32.ToString(new byte[] { 153, 201, 010, 032, 074, 012, 093, 102, 149, 215, 077, 024, 025, 058, 164, 140, 206, 069, 131, 056, 027, 141, 173, 243, 190, 255 });
			var exp = "THEQUICKBROWNFOXJUMBSOVERTHELAZYDOG2345674";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetBytes_Q0waiv_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = Assert.Throws<FormatException>(() =>
				{
					Base32.GetBytes("Q0waiv");
				});
				Assert.AreEqual("Not a valid Base32 string", act.Message);
			}
		}

		[Test]
		public void GetBytes_Null_EmptyArray()
		{
			var act = Base32.GetBytes(null);
			var exp = new byte[0];
			CollectionAssert.AreEqual(exp, act);
		}
		[Test]
		public void GetBytes_StringEmpty_EmptyArray()
		{
			var act = Base32.GetBytes(string.Empty);
			var exp = new byte[0];
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetBytes_Qowaiv_131B172B4B()
		{
			var act = Base32.GetBytes("Qowaiv");
			var exp = new byte []{ 131, 172, 004 };
			CollectionAssert.AreEqual(exp, act);
		}
		
		[Test]
		public void GetBytes_TheQuickBrownFoxJumbsOverTheLazyDog23456777_ByteArray()
		{
			var act = Base32.GetBytes("TheQuickBrownFoxJumbsOverTheLazyDog2345674");
			var exp = new byte[] { 153, 201, 010, 032, 074, 012, 093, 102, 149, 215, 077, 024, 025, 058, 164, 140, 206, 069, 131, 056, 027, 141, 173, 243, 190, 255 };
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
