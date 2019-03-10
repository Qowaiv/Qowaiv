using NUnit.Framework;
using Qowaiv.ComponentModel.Messages;
using System;

namespace Qowaiv.ComponentModel.Tests
{
    public class ResultTest
    {
        internal static readonly ValidationMessage Error1 = ValidationMessage.Error("Error 1");
        internal static readonly ValidationMessage Error2 = ValidationMessage.Error("Error 2");
        internal static readonly ValidationMessage Warning1 = ValidationMessage.Warning("Warning 1");
        internal static readonly ValidationMessage Warning2 = ValidationMessage.Warning("Warning 2");
        internal static readonly ValidationMessage Info1 = ValidationMessage.Info("Info 1");
        internal static readonly ValidationMessage Info2 = ValidationMessage.Info("Info 2");
        internal static readonly ValidationMessage[] TestMessages =
        {
            Error1, Error2, Warning1, Warning2, Info1, Info2
        };

        [Test]
        public void IsValid_TestMesages_False()
        {
            var result = Result.WithMessages(TestMessages);
            var act = result.IsValid;
            Assert.IsFalse(act);
        }

        [Test]
        public void IsValid_SomeNoneErrorsMesages_True()
        {
            var result = Result.WithMessages<int>(Warning1, Info1, Info2);
            var act = result.IsValid;
            Assert.True(act);
        }

        [Test]
        public void WithError_DoesNotAllowAccessToTheModel()
        {
            var result = Result.For(new object(), ValidationMessage.Error("Not OK"));

            Assert.Throws<InvalidModelException>(() => Console.WriteLine(result.Data));
        }

        [Test]
        public void Errors_2Items()
        {
            var result = Result.WithMessages(TestMessages);
            var act = result.Errors;
            var exp = new[] { Error1, Error2 };
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Warnings_2Items()
        {
            var result = Result.WithMessages(TestMessages);
            var act = result.Warnings;
            var exp = new[] { Warning1, Warning2 };
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Infos_2Items()
        {
            var result = Result.WithMessages(TestMessages);
            var act = result.Infos;
            var exp = new[] { Info1, Info2 };
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Ctor_WithData_HasBeenSet()
        {
            var exp = 2;
            var result = Result.For(exp);
            var act = result.Data;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Implicit_Result_ToType()
        {
            Result<bool> result = true;
            Assert.IsTrue(result.Data);
        }

        [Test]
        public void Implicit_Null_ToType()
        {
            Result<bool> result = false;
            Assert.IsFalse(result.Data);
        }

        [Test]
        public void Explicit_ToResultOfType()
        {
            var result = (Result<bool>)true;
            Assert.IsTrue((bool)result);
        }

        [Test]
        public void WithError_IsNotValid()
        {
            var actual = Result.WithMessages(ValidationMessage.Error("This should not be valid"));
            Assert.False(actual.IsValid);
        }

        [Test]
        public void WithWarning_IsValid()
        {
            var actual = Result.WithMessages(ValidationMessage.Warning("This should valid"));
            Assert.True(actual.IsValid);
        }

        [Test]
        public void WithInfo_IsValid()
        {
            var actual = Result.WithMessages(ValidationMessage.Info("This should be valid"));
            Assert.True(actual.IsValid);
        }
    }
}
