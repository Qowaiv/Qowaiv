using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Text.Json.Serialization;
using System;
using System.Text.Json;

namespace Qowaiv.UnitTests.Text.Json.Serialization
{
    public class QowaivJsonConverterTest
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions { Converters = { new QowaivJsonConverter() } };

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

        [Test]
        public void Deserialize_Null_Successful()
        {
            var obj = JsonSerializer.Deserialize<Svo>(@"null", options);
            Assert.IsNull(obj.Value);
        }

        [Test]
        public void Deserialize_NullNullable_Successful()
        {
            var obj = JsonSerializer.Deserialize<Svo?>(@"null", options);
            Assert.IsFalse(obj.HasValue);
        }

        [Test]
        public void Deserialize_String_Successful()
        {
            var obj = JsonSerializer.Deserialize<Svo>(@"""test""", options);
            Assert.AreEqual("test", obj.Value);
        }

        [Test]
        public void Deserialize_Long_Successful()
        {
            var obj = JsonSerializer.Deserialize<Svo>("666", options);
            Assert.AreEqual(666L, obj.Value);
        }

        [Test]
        public void Deserialize_Double_Successful()
        {
            var obj = JsonSerializer.Deserialize<Svo>("2.5", options);
            Assert.AreEqual(2.5, obj.Value);
        }

        [Test]
        public void Deserialize_Boolean_Throws()
        {
            var x = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Svo>("true", options));
            Assert.AreEqual("QowaivJsonConverter does not support token type True.", x.Message);
        }


        [Test]
        public void Serialize_Null_Successful()
        {
            var json = JsonSerializer.Serialize(new Svo(), options);
            Assert.AreEqual("null", json);
        }

        [Test]
        public void Serialize_String_Successful()
        {
            var json = JsonSerializer.Serialize(new Svo { Value = "test" }, options);
            Assert.AreEqual(@"""test""", json);
        }

        [Test]
        public void Serialize_Long_Successful()
        {
            var json = JsonSerializer.Serialize(new Svo { Value = 666L }, options);
            Assert.AreEqual(@"666", json);
        }

        [Test]
        public void Serialize_Double_Successful()
        {
            var json = JsonSerializer.Serialize(new Svo { Value = 2.5 }, options);
            Assert.AreEqual(@"2.5", json);
        }

        [Test]
        public void Serialize_Decimal_Successful()
        {
            var json = JsonSerializer.Serialize(new Svo { Value = 2.5m }, options);
            Assert.AreEqual(@"2.5", json);
        }

        [Test]
        public void Serialize_DateTime_Successful()
        {
            var json = JsonSerializer.Serialize(new Svo { Value = new DateTime(2017, 06, 11) }, options);
            Assert.AreEqual(@"""2017-06-11T00:00:00""", json);
        }

        [Test]
        public void Serialize_Boolean_Throws()
        {
            var x = Assert.Catch<JsonException>(() => JsonSerializer.Serialize(new Svo { Value = true }, options));
            Assert.AreEqual("QowaivJsonConverter does not support writing to System.Boolean.", x.Message);
        }
    }

    public struct Svo : Qowaiv.Json.IJsonSerializable
    {
        public object Value { get; set; }

        public void FromJson() => Value = null;

        public void FromJson(string jsonString) => Value = jsonString;

        public void FromJson(long jsonInteger) => Value = jsonInteger;

        public void FromJson(double jsonNumber) => Value = jsonNumber;

        public void FromJson(DateTime jsonDate) => Value = jsonDate;

        public object ToJson() => Value;
    }
}
