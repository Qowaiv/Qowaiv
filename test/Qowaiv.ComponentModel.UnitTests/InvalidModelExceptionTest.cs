using NUnit.Framework;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Tests.TestTools;
using Qowaiv.TestTools;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests
{
    public class InvalidModelExceptionTest
    {
        [Test]
        public void Serializable_ExceptionWithErrors_Successful()
        {
            var expected = InvalidModelException.For<int>(new ValidationResult[]
                {
                    ValidationMessage.None,
                    ValidationMessage.Error("Not a prime", "_value"),
                    ValidationMessage.Warning("Small", "_value"),
                    ValidationMessage.Info("Not my favorite", "_value"),
                    new ValidationResult("Not serializable", new []{ "this" })
                });

            var actual = SerializationTest.SerializeDeserialize(expected);

            Assert.AreEqual(expected.Message, actual.Message);
            Assert.IsNull(actual.InnerException);

            var actualErrors = actual.Errors.ForAssertion();

            Assert.AreEqual(new[]
            {
                ValidationTestMessage.Error("Not a prime", "_value"),
                ValidationTestMessage.Error("Not serializable", "this")
            },
            actualErrors);
        }
    }
}
