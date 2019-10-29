using Newtonsoft.Json;
using NUnit.Framework;
using Qowaiv.Json;

namespace Qowaiv.UnitTests.Json.Newtonsoft
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

            var act = JsonConvert.SerializeObject(obj, global::Newtonsoft.Json.Formatting.None);
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


        [Test]
        public void DeserializeObject_WithQowaivJsonConverter_SupportsJsonDateNode()
        {
            QowaivJsonConverter.Register();

            var json = "{ Id: 3, Date: \"2012-04-23T10:25:43.015-05:00\", Val: \"Hello World!\" }";
            var act = JsonConvert.DeserializeObject<JsonDateObject>(json);

            Assert.AreEqual(3, act.Id);
            Assert.AreEqual(new Date(2012, 04, 23), act.Date);
            Assert.AreEqual("Hello World!", act.Val);
        }

        internal class ReadModel
        {
            public Gender Gender { get; set; }
        }

        internal class JsonDateObject
        {
            public int Id { get; set; }
            public Date? Date { get; set; }
            public string Val { get; set; }
        }

    }
}
