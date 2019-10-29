using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Text.Json.Serialization;

namespace Qowaiv.UnitTests.Text.Json.Serialization
{
    public class QowaivJsonConverterTest
    {
        [Test]
        public void CanConvert_Null_False()
        {
            var factory = new QowaivJsonConverter();
            Assert.IsFalse(factory.CanConvert(null));
        }

        [Test]
        public void CanConvert_Object_False()
        {
            var factory = new QowaivJsonConverter();
            Assert.IsFalse(factory.CanConvert(typeof(object)));
        }

        [Test]
        public void CanConvert_Iban_True()
        {
            var factory = new QowaivJsonConverter();
            Assert.IsTrue(factory.CanConvert(typeof(InternationalBankAccountNumber)));
        }

        [Test]
        public void CanConvert_NullableIban_True()
        {
            var factory = new QowaivJsonConverter();
            Assert.IsTrue(factory.CanConvert(typeof(InternationalBankAccountNumber?)));
        }
    }
}
