using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.TestTools.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    public class CalculatedPropertyTest
    {
        [Test]
        public void IsValid_CalculatedProperty_IsValid()
        {
            var model = new EntityWithCaculatedProperty();
            model.SetProperties((m) =>
            {
                m.Id = 17;
                m.Repertitions = 5;
                m.Value = 8;
            });
        }

        [Test]
        public void IsValid_CalculatedProperty_WithErrors()
        {
            var model = new EntityWithCaculatedProperty();
            
            var result = model.SetProperties((m) =>
            {
                m.Id = 17;
                m.Repertitions = 50;
                m.Value = 8.17m;
            });

            Assert.IsFalse(result.IsValid);
        }
    }
}
