using NUnit.Framework;
using Qowaiv.DomainModel.Tracking;
using Qowaiv.Validation.DataAnnotations;
using System;

namespace Qowaiv.DomainModel.UnitTests.Tracking
{
    public class ChangeTrackerTrackerTest
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
    }
}
