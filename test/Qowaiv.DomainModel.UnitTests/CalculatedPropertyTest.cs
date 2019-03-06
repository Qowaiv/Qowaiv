using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    public class CalculatedPropertyTest
    {
        [Test]
        public void IsValid_CalculatedProperty_IsValid()
        {
            var model = new EntityWithCaculatedProperty();
            model.SetProperties(() =>
            {
                model.Id = 17;
                model.Repertitions = 5;
                model.Value = 8;
            });
        }

        [Test]
        public void IsValid_CalculatedProperty_WithErrors()
        {
            var model = new EntityWithCaculatedProperty();
            Assert.Throws<ValidationException>(() =>
            {
                model.SetProperties(() =>
                {
                    model.Id = 17;
                    model.Repertitions = 50;
                    model.Value = 8.17m;
                });
            });
        }
    }
}
