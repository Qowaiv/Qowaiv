using NUnit.Framework;
using Qowaiv.Json.UnitTests.Models;
using System;

namespace Qowaiv.Json.UnitTests
{
    /// <remarks>
    /// This abstract base class should help to guarantee that different
    /// implementations have the same behaviour.
    /// </remarks>
    public abstract class JsonSerializerTestBase<TException>  where TException: Exception
    {
        protected abstract bool CanConvert(Type type);

        protected abstract T Deserialize<T>(string json);

        protected abstract string Serialize(object obj);

        [Test]
        public void CanConvert_Null_False()
        {
            Assert.IsFalse(CanConvert(null));
        }

        [Test]
        public void CanConvert_Object_False()
        {
            Assert.IsFalse(CanConvert(typeof(object)));
        }

        [Test]
        public void CanConvert_Svo_True()
        {
            Assert.IsTrue(CanConvert(typeof(Svo)));
        }

        [Test]
        public void CanConvert_SvoIban_True()
        {
            Assert.IsTrue(CanConvert(typeof(Svo?)));
        }

        [Test]
        public void CanConvert_SvoClass_True()
        {
            Assert.IsTrue(CanConvert(typeof(SvoClass)));
        }

        [Test]
        public void Deserialize_Null_Successful()
        {
            var obj = Deserialize<Svo>(@"null");
            Assert.IsNull(obj.Value);
        }

        [Test]
        public void Deserialize_NullNullable_Successful()
        {
            var obj = Deserialize<Svo?>(@"null");
            Assert.IsFalse(obj.HasValue);
        }

        [Test]
        public void Deserialize_NullClass_Successful()
        {
            var obj = Deserialize<SvoClass>(@"null");
            Assert.IsNull(obj);
        }

        [Test]
        public void Deserialize_String_Successful()
        {
            var obj = Deserialize<Svo>(@"""test""");
            Assert.AreEqual("test", obj.Value);
        }

        [Test]
        public void Deserialize_Long_Successful()
        {
            var obj = Deserialize<Svo>("666");
            Assert.AreEqual(666L, obj.Value);
        }

        [Test]
        public void Deserialize_Double_Successful()
        {
            var obj = Deserialize<Svo>("2.5");
            Assert.AreEqual(2.5, obj.Value);
        }

        [Test]
        public void Deseralize_SvoClass_Successful()
        {
            var obj = Deserialize<SvoClass>("2.5");
            Assert.AreEqual(2.5, obj.Value);
        }

        [Test]
        public void Deserialize_Object_Succesful()
        {
            var json = @"{ ""Id"": 3, ""Date"": ""2012-04-23T10:25:43.015-05:00"", ""Val"": ""Hello World!"" }";
            var dto = Deserialize<DtoClass>(json);

            Assert.AreEqual(3, dto.Id);
            Assert.AreEqual(new Date(2012, 04, 23), dto.Date);
            Assert.AreEqual("Hello World!", dto.Val);
        }

        [Test]
        public void Deserialize_Boolean_Yes()
        {
            var actual = Deserialize<YesNo>("true");
            Assert.AreEqual(YesNo.Yes, actual);
        }

        [Test]
        public void Deserialize_NotSupported_Throws()
        {
            var x = Assert.Throws<TException>(() => Deserialize<Gender>("25.0"));
            Assert.AreEqual("JSON deserialization from a number is not supported.", x.Message);
        }

        [Test]
        public void Deserialize_Unparseable_Throws()
        {
            var x = Assert.Throws<TException>(() => Deserialize<Gender>(@"""invalid"""));
            Assert.AreEqual("Not a valid gender", x.Message);
        }


        [Test]
        public void Serialize_Null_Successful()
        {
            var json = Serialize(new Svo());
            Assert.AreEqual("null", json);
        }

        [Test]
        public void Serialize_String_Successful()
        {
            var json = Serialize(new Svo("test"));
            Assert.AreEqual(@"""test""", json);
        }

        [Test]
        public void Serialize_Long_Successful()
        {
            var json = Serialize(new Svo(666L));
            Assert.AreEqual(@"666", json);
        }

        [Test]
        public void Serialize_Double_Successful()
        {
            var json = Serialize(new Svo(2.5));
            Assert.AreEqual(@"2.5", json);
        }

        [Test]
        public void Serialize_Decimal_Successful()
        {
            var json = Serialize(new Svo(2.5m));
            Assert.AreEqual(@"2.5", json);
        }

        [Test]
        public void Serialize_DateTime_Successful()
        {
            var json = Serialize(new Svo(new DateTime(2017, 06, 11)));
            Assert.AreEqual(@"""2017-06-11T00:00:00""", json);
        }
    }
}
