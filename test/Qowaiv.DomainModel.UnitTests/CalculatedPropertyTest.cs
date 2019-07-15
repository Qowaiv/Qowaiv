using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.TestTools.Validiation;

namespace Qowaiv.DomainModel.UnitTests
{
    public class CalculatedPropertyTest
    {
        [Test]
        public void IsValid_CalculatedProperty_IsValid()
        {
            var model = new EntityWithCaculatedProperty();
            var result = model.TrackChanges((m) =>
            {
                m.Repertitions = 5;
                m.Value = 8;
            });

            ValidationMessageAssert.IsValid(result);
        }

        [Test]
        public void IsValid_CalculatedProperty_WithErrors()
        {
            var model = new EntityWithCaculatedProperty();
            
            var result = model.TrackChanges((m) =>
            {
                m.Repertitions = 50;
                m.Value = 8.17m;
            });

            Assert.IsFalse(result.IsValid);
        }
    }
}
