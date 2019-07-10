using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools.Validiation;
using Qowaiv.Validation.DataAnnotations.UnitTests.Models;
using System;

namespace Qowaiv.Validation.DataAnnotations.UnitTests
{
    public class DataAnnotationsValidatorTest
    {
        [Test]
        public void Validate_ModelWithMandatoryProperties_WithErrors()
        {
            using (CultureInfoScope.NewInvariant())
            {
                var model = new ModelWithMandatoryProperties();

                DataAnnotationsAssert.WithErrors(model,
                    ValidationMessage.Error("The E-mail address field is required.", "Email"),
                    ValidationMessage.Error("The SomeString field is required.", "SomeString")
                );
            }
        }
        [Test]
        public void Validate_ModelWithMandatoryProperties_IsValid()
        {
            var model = new ModelWithMandatoryProperties
            {
                Email = EmailAddress.Parse("info@qowaiv.org"),
                SomeString = "Some value",
            };
            DataAnnotationsAssert.IsValid(model);
        }

        [Test]
        public void Validate_ModelWithAllowedValues_WithError()
        {
            var model = new ModelWithAllowedValues
            {
                Country = Country.TR
            };

            DataAnnotationsAssert.WithErrors(model,
                ValidationMessage.Error("The value of the Country field is not allowed.", "Country")
            );
        }
        [Test]
        public void Validate_ModelWithAllowedValues_IsValid()
        {
            var model = new ModelWithAllowedValues();
            DataAnnotationsAssert.IsValid(model);
        }

        [Test]
        public void Validate_ModelWithForbiddenValues_WithError()
        {
            var model = new ModelWithForbiddenValues
            {
                Email = EmailAddress.Parse("spam@qowaiv.org"),
            };
            DataAnnotationsAssert.WithErrors(model,
                ValidationMessage.Error("The value of the Email field is not allowed.", "Email"));
        }
        [Test]
        public void Validate_ModelWithForbiddenValues_IsValid()
        {
            var model = new ModelWithForbiddenValues
            {
                Email = EmailAddress.Parse("info@qowaiv.org"),
            };
            DataAnnotationsAssert.IsValid(model);
        }

        [Test]
        public void Validate_ModelWithCustomizedResource_WithError()
        {
            var model = new ModelWithCustomizedResource();
            DataAnnotationsAssert.WithErrors(model,
                ValidationMessage.Error("This IBAN is wrong.", "Iban"));
        }

        [Test]
        public void Validate_NestedModelWithNullChild_With1Error()
        {
            var model = new NestedModel()
            {
                Id = Guid.NewGuid()
            };
            DataAnnotationsAssert.WithErrors(model,
                ValidationMessage.Error("The Child field is required.", "Child"));
        }

        [Test]
        public void Validate_NestedModelWithInvalidChild_With1Error()
        {
            var model = new NestedModel()
            {
                Id = Guid.NewGuid(),
                Child = new NestedModel.ChildModel()
            };
            DataAnnotationsAssert.WithErrors(model,
                ValidationMessage.Error("The Name field is required.", "Child.Name"));
        }

        [Test]
        public void Validate_NestedModelWithInvalidChildren_With1Error()
        {
            var model = new NestedModelWithChildren()
            {
                Id = Guid.NewGuid(),
                Children = new[]
                {
                    new NestedModelWithChildren.ChildModel{ Name = "Valid Name" },
                    new NestedModelWithChildren.ChildModel()
                }
            };
            DataAnnotationsAssert.WithErrors(model,
                ValidationMessage.Error("The Name field is required.", "Children[1].Name"));
        }

        [Test]
        public void Validate_NestedModelWithLoop_With1Error()
        {
            var model = new NestedModelWithLoop()
            {
                Id = Guid.NewGuid(),
                Child = new NestedModelWithLoop.ChildModel(),
            };
            model.Child.Parent = model;

            DataAnnotationsAssert.WithErrors(model,
                ValidationMessage.Error("The Name field is required.", "Child.Name"));
        }
    }
}
