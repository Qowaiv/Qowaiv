using NUnit.Framework;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.TestTools;

namespace Qowaiv.ComponentModel.UnitTests.Messages
{
    public class ValidationMessageTest
    {

        [Test]
        public void Serializable_SomeMessage_Successful()
        {
            var message = ValidationMessage.Error("Can be serialized", "ErrorMessage", "MemberNames");

            var actual = SerializationTest.SerializeDeserialize(message);

            Assert.AreEqual(message.ErrorMessage, actual.ErrorMessage);
            Assert.AreEqual(message.MemberNames, actual.MemberNames);
        }
    }
}
