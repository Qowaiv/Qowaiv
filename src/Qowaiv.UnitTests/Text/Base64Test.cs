using NUnit.Framework;
using Qowaiv.Text;
using System;

namespace Qowaiv.UnitTests.Text
{
	[TestFixture]
	public class Base64Test
	{
		[Test]
		public void ToString_Null_StringEmpty()
		{
			Assert.AreEqual(string.Empty, Base64.ToString(null));
		}
		
		[Test]
		public void ToString_EmptyArray_StringEmpty()
		{
			Assert.AreEqual(string.Empty, Base64.ToString(new byte[0]));
		}
		
		[Test]
		public void ToString_Array_QowaigEqualSignEqualSign()
		{
			Assert.AreEqual("Qowaig==", Base64.ToString(new byte[] { 66, 140, 26, 138 }));
		}


		[Test]
		public void TryGetBytes_null_IsTrue()
		{
			byte[] act;
			byte[] exp = new byte[0];

			Assert.IsTrue(Base64.TryGetBytes(null, out act));
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TryGetBytes_StringEmpty_IsTrue()
		{
			byte[] act;
			byte[] exp = new byte[0];

			Assert.IsTrue(Base64.TryGetBytes(String.Empty, out act));
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TryGetBytes_Questionmark_IsFalse()
		{
			byte[] act;
			byte[] exp = new byte[0];

			Assert.IsFalse(Base64.TryGetBytes("?", out act));
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TryGetBytes_EqualSign_IsFalse()
		{
			byte[] act;
			byte[] exp = new byte[0];

			Assert.IsFalse(Base64.TryGetBytes("=", out act));
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TryGetBytes_A_IsFalse()
		{
			byte[] act;
			byte[] exp = new byte[0];

			Assert.IsFalse(Base64.TryGetBytes("A", out act));
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TryGetBytes_AapEqualSign_IsTrue()
		{
			byte[] act;
			byte[] exp = new byte[] { 1, 170 };

			Assert.IsTrue(Base64.TryGetBytes("Aap=", out act));
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TryGetBytes_Qowaiv_IsTrue()
		{
			byte[] act;
			byte[] exp = new byte[] { 66, 140, 26, 138 };

			Assert.IsTrue(Base64.TryGetBytes("Qowaiv==", out act));
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
