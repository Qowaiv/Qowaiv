using NUnit.Framework;
using Qowaiv;
using Qowaiv.Globalization;
using Qowaiv.TestTools.Globalization;
using Qowaiv.UnitTests;
using System;

namespace Obsolete_code
{
    [Obsolete("Will be dropped when the next major version is released.")]
    public class Will_be_dropped
    {
        [Test]
        public void Decimal_rounding_misspelled_IsNearestRouding()
        {
            Assert.IsTrue(DecimalRounding.AwayFromZero.IsNearestRouding());
        }

        [Test]
        public void Email_address_regex_pattern_is_slow_and_incomplete()
        {
            Assert.NotNull(EmailAddress.Pattern);
        }

        [Test]
        public void PercentageMark() => Assert.NotNull(Percentage.PercentageMark);

        [Test]
        public void PerMilleMark() => Assert.NotNull(Percentage.PerMilleMark);

        [Test]
        public void PerTenThousandMark() => Assert.NotNull(Percentage.PerTenThousandMark);

        [Test]
        public void ToPerMilleString()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual("175.1‰", Svo.Percentage.ToPerMilleString());
            }
        }

        [Test]
        public void ToPerTenThousandMarkString()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual("1751‱", Svo.Percentage.ToPerTenThousandMarkString());
            }
        }
    }
}
