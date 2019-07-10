using NUnit.Framework;
using Qowaiv.TestTools;

namespace Qowaiv.Validation.Abstractions.UnitTests
{
    public class InvalidModelExceptionTest
    {
        [Test]
        public void Serializable_ExceptionWithErrors_Successful()
        {
            var expected = InvalidModelException.For<int>(new IValidationMessage[]
                {
                    ValidationMessage.None,
                    ValidationMessage.Error("Not a prime", "_value"),
                    ValidationMessage.Warn("Small", "_value"),
                    ValidationMessage.Info("Not my favorite", "_value"),
                });

            var actual = SerializationTest.SerializeDeserialize(expected);

            Assert.AreEqual(expected.Message, actual.Message);
            Assert.IsNull(actual.InnerException);

            CollectionAssert.AreEqual(expected.Errors, actual.Errors);
        }
    }
}
