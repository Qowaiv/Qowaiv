using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.Formatting;
using Qowaiv.UnitTests.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Qowaiv.UnitTests.Formatting
{
    [TestClass]
    public class StringFormatterTest
    {
        [TestMethod]
        public void Apply_NullObject_ThrowsArugmentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                StringFormatter.Apply<object>(null, null, null, null);
            },
            "obj");
        }

        [TestMethod]
        public void Apply_StringEmptyFormat_ThrowsArugmentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                StringFormatter.Apply(new object(), String.Empty, null, null);
            },
            "format");
        }

        [TestMethod]
        public void Apply_NullFormat_ThrowsArugmentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                StringFormatter.Apply<object>(new object(), null, null, null);
            },
            "format");
        }
        
        [TestMethod]
        public void Apply_NullFormatProvider_ThrowsArugmentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                StringFormatter.Apply(Int32.MinValue, "0", null, null);
            },
            "formatProvider");
        }
        
        [TestMethod]
        public void Apply_NullTokens_ThrowsArugmentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                StringFormatter.Apply(Int32.MinValue, "0", CultureInfo.InvariantCulture, null);
            },
            "tokens");
        }

        [TestMethod]
        public void Apply_InvalidFormat_ThrowsFormatException()
        {
            ExceptionAssert.ExpectException<FormatException>(() =>
            {
                StringFormatter.Apply(Int32.MinValue, "\\", CultureInfo.InvariantCulture, new Dictionary<char, Func<Int32, IFormatProvider, string>>());
            },
            "Format is invalid.");
        }

		[TestMethod]
		public void ToNonDiacritic_Null_AreEqual()
		{
			var str = (string)null;

			var exp = (string)null;
			var act = StringFormatter.ToNonDiacritic(str);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void ToNonDiacritic_StringEmpty_AreEqual()
		{
			var str = String.Empty;

			var exp = String.Empty;
			var act = StringFormatter.ToNonDiacritic(str);

			Assert.AreEqual(exp, act);
		}
		
		[TestMethod]
		public void ToNonDiacritic_CafeUndStrasse_AreEqual()
		{
			var str = "Café & Straße";

			var exp = "Cafe & Strasze";
			var act = StringFormatter.ToNonDiacritic(str);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void ToNonDiacritic_CafeUndStrasseIgnoreE_AreEqual()
		{
			var str = "Café & Straße";

			var exp = "Café & Strasze";
			var act = StringFormatter.ToNonDiacritic(str, "é");

			Assert.AreEqual(exp, act);
		}

	}
}
