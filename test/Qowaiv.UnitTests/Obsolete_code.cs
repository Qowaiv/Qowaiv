using NUnit.Framework;
using Qowaiv;
using Qowaiv.Conversion;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.Json;
using Qowaiv.Reflection;
using Qowaiv.TestTools.Globalization;
using Qowaiv.UnitTests;
using Qowaiv.Web;
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
        public void GuidTypeConverter()
        {
            var converter = new GuidTypeConverter();
            Assert.NotNull(converter.ConvertFrom(Guid.NewGuid().ToString()));
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

        [Test]
        public void Gender_LessThen() => Assert.IsFalse(Svo.Gender < Gender.Female);
        
        [Test]
        public void Gender_LessThenOrEqual() => Assert.IsTrue(Svo.Gender <= Gender.Female);

        [Test]
        public void Gender_GreaterThen() => Assert.IsFalse(Svo.Gender > Gender.Female);

        [Test]
        public void Gender_GreaterThenOrEqual() => Assert.IsTrue(Svo.Gender >= Gender.Female);

        [Test]
        public void QowaivType_IsSingleValueObject() => Assert.IsTrue(QowaivType.IsSingleValueObject(typeof(Percentage)));

        [Test]
        public void OpenApiDataType_ctor_without_example()=> Assert.NotNull(new OpenApiDataTypeAttribute(description: "Postal code notation.", type: "string", format: "postal-code", nullable: true));
    }

    [Obsolete("Will become private when the next major version is released.")]
    public class Will_become_private
    {
        [Test]
        public void BIC() => Assert.NotNull(BusinessIdentifierCode.Pattern);

        [Test]
        public void internet_media_type() => Assert.NotNull(InternetMediaType.Pattern);

        [Test]
        public void date_span() => Assert.NotNull(DateSpan.Pattern);

        [Test]
        public void house_number() => Assert.NotNull(HouseNumber.Pattern);

        [Test]
        public void month() => Assert.NotNull(Month.Pattern);

        [Test]
        public void postal_code() => Assert.NotNull(PostalCode.Pattern);

        [Test]
        public void UUID() => Assert.NotNull(Uuid.Pattern);
        
        [Test]
        public void week_date() => Assert.NotNull(WeekDate.Pattern);

        [Test]
        public void year() => Assert.NotNull(Year.Pattern);
    }
}
