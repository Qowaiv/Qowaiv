using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using Qowaiv.Json;

namespace Qowaiv.UnitTests.Json
{
    [TestClass]
    public class QowaivJsonConverterTest
    {
        //[TestInitialize]
        //public void InitializeTest()
        //{
        //    QowaivJsonConverter.Register();
        //}

        //[TestMethod]
        //public void Register_ClearAll_Successful()
        //{
        //    var settings = Newtonsoft.Json.JsonConvert.DefaultSettings.Invoke();
        //    settings.Converters.Clear();

        //    Assert.AreEqual(0, settings.Converters.Count, "Default converters should be empty.");

        //    QowaivJsonConverter.Register();

        //    settings = Newtonsoft.Json.JsonConvert.DefaultSettings.Invoke();

        //    Assert.AreEqual(1, settings.Converters.Count, "Default converters should contain one converter.");
        //    Assert.AreEqual(typeof(QowaivJsonConverter), settings.Converters[0].GetType(), "Default converters should contain a QowaivJsonConverter.");
        //}

        //[TestMethod]
        //public void CanConvert_Null_IsFalse()
        //{
        //    var converter = new QowaivJsonConverter();

        //    Assert.IsFalse(converter.CanConvert(null));
        //}

        //[TestMethod]
        //public void CanConvert_Boolean_IsFalse()
        //{
        //    var converter = new QowaivJsonConverter();

        //    Assert.IsFalse(converter.CanConvert(typeof(Boolean)));
        //}
        //[TestMethod]
        //public void CanConvert_NullableMoney_IsTrue()
        //{
        //    var converter = new QowaivJsonConverter();

        //    Assert.IsTrue(converter.CanConvert(typeof(Money?)));
        //}

        //[TestMethod]
        //public void CanConvert_Money_IsFalse()
        //{
        //    var converter = new QowaivJsonConverter();

        //    Assert.IsTrue(converter.CanConvert(typeof(Money)));

        //}
        //[TestMethod]
        //public void CanConvert_NullableBoolean_IsFalse()
        //{
        //    var converter = new QowaivJsonConverter();

        //    Assert.IsFalse(converter.CanConvert(typeof(Boolean?)));
        //}

        //[TestMethod]
        //public void SerializeObject_NullableMoney_AreEqual()
        //{
        //    var obj = (Money?)null;

        //    var act = JsonConvert.SerializeObject(obj, Formatting.None);
        //    var exp = "null";

        //    Assert.AreEqual(exp, act);
        //}

        //[TestMethod]
        //public void SerializeObject_BurgerServiceNummerMinValue_AreEqual()
        //{
        //    var obj = BurgerServiceNummer.MinValue;

        //    var act = JsonConvert.SerializeObject(obj, Formatting.None);
        //    var exp = "\"010000008\"";

        //    Assert.AreEqual(exp, act);
        //}

        //[TestMethod]
        //public void SerializeObject_GenderNotApplicable_AreEqual()
        //{
        //    var obj = Gender.NotApplicable;

        //    var act = JsonConvert.SerializeObject(obj, Formatting.None);
        //    var exp = "\"NotApplicable\"";

        //    Assert.AreEqual(exp, act);
        //}
        
        //[TestMethod]
        //public void SerializeObject_Money123Dot501_AreEqual()
        //{
        //    var obj = (Money)132.501m;

        //    var act = JsonConvert.SerializeObject(obj, Formatting.None);
        //    var exp = "132.501";

        //    Assert.AreEqual(exp, act);
        //}
    }
}
