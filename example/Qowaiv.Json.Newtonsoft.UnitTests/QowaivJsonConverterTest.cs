using Newtonsoft.Json;
using NUnit.Framework;

namespace Qowaiv.Json.UnitTests
{
    public class QowaivJsonConverterTest
    {
        public QowaivJsonConverterTest()
        {
            if (JsonConvert.DefaultSettings == null)
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings() { Converters = { new QowaivJsonConverter() } };
            }
            QowaivJsonConverter.Register();
        }

        [Test]
        public void Register_ClearAll_Successful()
        {
            var settings = JsonConvert.DefaultSettings.Invoke();
            settings.Converters.Clear();

            Assert.AreEqual(0, settings.Converters.Count, "Default converters should be empty.");

            QowaivJsonConverter.Register();

            settings = JsonConvert.DefaultSettings.Invoke();

            Assert.AreEqual(1, settings.Converters.Count, "Default converters should contain one converter.");
            Assert.AreEqual(typeof(QowaivJsonConverter), settings.Converters[0].GetType(), "Default converters should contain a QowaivJsonConverter.");
        }

        [Test]
        public void CanConvert_Null_IsFalse()
        {
            var converter = new QowaivJsonConverter();

            Assert.IsFalse(converter.CanConvert(null));
        }

        [Test]
        public void CanConvert_Boolean_IsFalse()
        {
            var converter = new QowaivJsonConverter();

            Assert.IsFalse(converter.CanConvert(typeof(bool)));
        }

        [Test]
        public void CanConvert_NullableBoolean_IsFalse()
        {
            var converter = new QowaivJsonConverter();

            Assert.IsFalse(converter.CanConvert(typeof(bool?)));
        }

        [Test]
        public void SerializeObject_GenderNotApplicable_AreEqual()
        {
            var obj = Gender.NotApplicable;

            var act = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None);
            var exp = "\"NotApplicable\"";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DeserializeObject_GenderInvalid_Throws()
        {
            var exception = Assert.Throws<JsonSerializationException>
            (
                ()=> JsonConvert.DeserializeObject<ReadModel>("{ \"Gender\": \"invalid\" }")
            );
            Assert.AreEqual("Not a valid gender", exception.Message);
        }

        [Test]
        public void DeserializeObject_GenderDouble_Throws()
        {
            var exception = Assert.Throws<JsonSerializationException>
            (
                () => JsonConvert.DeserializeObject<ReadModel>("{ \"Gender\": 0.0 }")
            );
            Assert.AreEqual("JSON deserialization from a number is not supported.", exception.Message);
        }

        internal class ReadModel
        {
            public Gender Gender { get; set; }
        }

    }
}
