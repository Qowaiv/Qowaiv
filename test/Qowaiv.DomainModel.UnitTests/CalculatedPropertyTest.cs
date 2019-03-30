using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using System;

namespace Qowaiv.DomainModel.UnitTests
{
    public class CalculatedPropertyTest
    {
        [Test]
        public void IsValid_CalculatedProperty_IsValid()
        {
            var model = new EntityWithCaculatedProperty();
            model.TrackChanges((m) =>
            {
                m.Id = Guid.NewGuid();
                m.Repertitions = 5;
                m.Value = 8;
            });
        }

        [Test]
        public void IsValid_CalculatedProperty_WithErrors()
        {
            var model = new EntityWithCaculatedProperty();
            
            var result = model.TrackChanges((m) =>
            {
                m.Id = Guid.NewGuid();
                m.Repertitions = 50;
                m.Value = 8.17m;
            });

            Assert.IsFalse(result.IsValid);
        }
    }
}
