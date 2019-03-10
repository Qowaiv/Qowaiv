using NUnit.Framework;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.TestTools;

namespace Qowaiv.ComponentModel.UnitTests.Messages
{
    public class ValidationMessageTest
    {
        [Test]
        public void Serializable_SomeInfoMessage_Successful()
        {
            var message = ValidationMessage.Info("Can be serialized", "ErrorMessage", "MemberNames");

            var actual = SerializationTest.SerializeDeserialize(message);

            Assert.AreEqual(message.ErrorMessage, actual.ErrorMessage);
            Assert.AreEqual(message.MemberNames, actual.MemberNames);
        }

        [Test]
        public void Serializable_SomeWarningMessage_Successful()
        {
            var message = ValidationMessage.Warning("Can be serialized", "ErrorMessage", "MemberNames");

            var actual = SerializationTest.SerializeDeserialize(message);

            Assert.AreEqual(message.ErrorMessage, actual.ErrorMessage);
            Assert.AreEqual(message.MemberNames, actual.MemberNames);
        }

        [Test]
        public void Serializable_SomeErrorMessage_Successful()
        {
            var message = ValidationMessage.Error("Can be serialized", "ErrorMessage", "MemberNames");

            var actual = SerializationTest.SerializeDeserialize(message);

            Assert.AreEqual(message.ErrorMessage, actual.ErrorMessage);
            Assert.AreEqual(message.MemberNames, actual.MemberNames);
        }
    }
}
