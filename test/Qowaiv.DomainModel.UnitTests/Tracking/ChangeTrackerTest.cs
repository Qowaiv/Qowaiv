using NUnit.Framework;
using Qowaiv.DomainModel.Tracking;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.Validation.DataAnnotations;
using System;

namespace Qowaiv.DomainModel.UnitTests.Tracking
{
    public class ChangeTrackerTest
    {
        [Test]
        public void Process_Uninitialized_Throws()
        {
            var tracker = new ChangeTracker<object>();
            Assert.Throws<InvalidOperationException>(() => tracker.Process());
        }

        [Test]
        public void Init_Twice_Throws()
        {
            var tracker = new ChangeTracker<object>();
            tracker.Init(new object(), new AnnotatedModelValidator<object>());
            Assert.Throws<InvalidOperationException>(() => tracker.Init(new object(), new AnnotatedModelValidator<object>()));
        }

        [Test]
        public void Validate_Throws_Rollsback()
        {
            var model = new EntityThatThrows();

            Assert.AreEqual(17, model.Value);
            Assert.Throws<DivideByZeroException>(() => model.Value = 666);
            Assert.AreEqual(17, model.Value);
        }
    }
}
