using NUnit.Framework;
using Qowaiv.ComponentModel.Messages;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests.Messages
{
    public class ValidationResultExtensionsTest
    {
        internal static readonly ValidationMessage Error1 = ValidationMessage.Error("Error 1", "prop1", "prop2");
        internal static readonly ValidationMessage Error2 = ValidationMessage.Error("Error 2");
        internal static readonly ValidationMessage Warning1 = ValidationMessage.Warning("Warning 1", "prop1", "prop2");
        internal static readonly ValidationMessage Warning2 = ValidationMessage.Warning("Warning 2");
        internal static readonly ValidationMessage Info1 = ValidationMessage.Info("Info 1", "prop1", "prop2");
        internal static readonly ValidationMessage Info2 = ValidationMessage.Info("Info 2");
        internal static readonly ValidationResult[] TestMessages =
        {
            Error1, Error2, Warning1, Warning2, Info1, Info2, ValidationResult.Success
        };

        [Test]
        public void GetSeverity_ValidationResult_Error()
        {
            var message = new ValidationResult("Some message");
            var act = message.GetSeverity();
            var exp = ValidationSeverity.Error;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetSeverity_Succes_None()
        {
            var message = ValidationResult.Success;
            var act = message.GetSeverity();
            var exp = ValidationSeverity.None;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetSeverity_ValidationInfoMessage_Info()
        {
            ValidationResult message = ValidationMessage.Info("Some message");
            var act = message.GetSeverity();
            var exp = ValidationSeverity.Info;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetSeverity_ValidationWarningMessage_Warning()
        {
            ValidationResult message = ValidationMessage.Warning("Some message");
            var act = message.GetSeverity();
            var exp = ValidationSeverity.Warning;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetSeverity_ValidationErrorMessage_Error()
        {
            ValidationResult message = ValidationMessage.Error("Some message");
            var act = message.GetSeverity();
            var exp = ValidationSeverity.Error;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetErrors_TestMessages_2Messages()
        {
            var act = TestMessages.GetErrors();
            var exp = new ValidationMessage[] { Error1, Error2 };

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetWarnings_TestMessages_2Messages()
        {
            var act = TestMessages.GetWarnings();
            var exp = new ValidationMessage[] { Warning1, Warning2 };

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetInfos_TestMessages_2Messages()
        {
            var act = TestMessages.GetInfos();
            var exp = new ValidationResult[] { Info1, Info2 };

            Assert.AreEqual(exp, act);
        }
    }
}
