using NUnit.Framework;
using Qowaiv.UnitTests.TestTools;
using System;

namespace Qowaiv.UnitTests
{
	[TestFixture]
	public class GuardTest
	{
		[Test]
		public void NotNull_Null_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				Guard.NotNull<Object>(null, "testParam");
			},
			"testParam");
		}
		[Test]
		public void NotNull_Object_AreSame()
		{
			var exp = new Object();
			var act = Guard.NotNull(exp, "testParam");
			Assert.AreSame(exp, act);
		}

		[Test]
		public void NotNullOrEmpty_Null_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				Guard.NotNullOrEmpty(null, "testParam");
			},
			"testParam");
		}
		[Test]
		public void NotNullOrEmpty_StringEmpty_ThrowsArgumentException()
		{
			ExceptionAssert.ExpectArgumentException(() =>
			{
				Guard.NotNullOrEmpty(String.Empty, "testParam");
			},
			"testParam",
			"Value cannot be an empty string.");
		}
		[Test]
		public void NotNullOrEmpty_Test_AreEqual()
		{
			var exp = "test";
			var act = Guard.NotNullOrEmpty(exp, "testParam");
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void NotNullOrEmpty_NullArray_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				Guard.NotNullOrEmpty((byte[])null, "testParam");
			},
			"testParam");
		}
		[Test]
		public void NotNullOrEmpty_EmptyArray_ThrowsArgumentException()
		{
			ExceptionAssert.ExpectArgumentException(() =>
			{
				Guard.NotNullOrEmpty(new byte[0], "testParam");
			},
			"testParam",
			"Value cannot be an empty array.");
		}
		[Test]
		public void NotNullOrEmpty_TestArray_AreEqual()
		{
			var exp = new byte[] { 0, 1 };
			var act = Guard.NotNullOrEmpty(exp, "testParam");
			Assert.AreEqual(exp, act);
		}
	
	}
}
