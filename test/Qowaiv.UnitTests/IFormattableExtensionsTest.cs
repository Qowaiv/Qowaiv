using NUnit.Framework;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.TestTools.Globalization;
using System;
using System.Globalization;

namespace Qowaiv.UnitTests
{
    [TestFixture]
    public class FormattableExtensionsTest
    {
        [Test]
        public void ToString_NullWithFormat_FormattedString()
        {
            IFormattable formattable = null;
            string act = formattable.ToString(new FormattingArguments("0.000", new CultureInfo("es-ES")));
            Assert.IsNull(act);
        }

        [Test]
        public void ToString_DecimalWithFormat_FormattedString()
        {
            var act = (123.45m).ToString(new FormattingArguments("0.000", new CultureInfo("es-ES")));
            var exp = "123,450";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_NullWithFormatCollection_FormattedString()
        {
            IFormattable formattable = null;
            string act = formattable.ToString(new FormattingArgumentsCollection());
            string exp = null;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_NullWithNullCollection_FormattedString()
        {
            IFormattable formattable = null;
            string act = formattable.ToString((FormattingArgumentsCollection)null);
            string exp = null;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_DecimalWithFormatCollection_FormattedString()
        {
            using (TestCultures.Es_EC.Scoped())
            {
                var collection = new FormattingArgumentsCollection();
                collection.Add(typeof(Decimal), "0.000");
                var act = (123.45m).ToString(collection);
                var exp = "123,450";

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_DecimalWithNullCollection_FormattedString()
        {
            using (TestCultures.Es_EC.Scoped())
            {
                var collection = (FormattingArgumentsCollection)null;

                var act = (123.45m).ToString(collection);
                var exp = "123,45";

                Assert.AreEqual(exp, act);
            }
        }
    }
}
