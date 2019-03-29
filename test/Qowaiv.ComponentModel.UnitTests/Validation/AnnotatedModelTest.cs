using NUnit.Framework;
using Qowaiv.ComponentModel.UnitTests.Validation.Models;
using Qowaiv.ComponentModel.Validation;

namespace Qowaiv.ComponentModel.UnitTests.Validation
{
    public class AnnotatedModelTest
    {
        [Test]
        public void Get_ModelWithPotentialRecursion_IsNotValidatable()
        {
            Assert.IsFalse(AnnotatedModel.Get(typeof(ModelWithPotentialRecursion)).IsValidatable);
        }
    }
}
