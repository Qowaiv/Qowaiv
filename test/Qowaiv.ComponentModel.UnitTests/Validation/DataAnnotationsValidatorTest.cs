﻿using NUnit.Framework;
using Qowaiv.ComponentModel.Tests.TestTools;
using Qowaiv.ComponentModel.UnitTests.Validation.Models;
using Qowaiv.ComponentModel.Validation;
using Qowaiv.Globalization;
using System;

namespace Qowaiv.ComponentModel.Tests.Validation
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
                    ValidationTestMessage.Error("The E-mail address field is required.", "Email"),
                    ValidationTestMessage.Error("The SomeString field is required.", "SomeString")
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
                ValidationTestMessage.Error("The value of the Country field is not allowed.", "Country")
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
                ValidationTestMessage.Error("The value of the Email field is not allowed.", "Email"));
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
        public void Validate_PostalCodeModel_Valid()
        {
            var model = new PostalCodeModel
            {
                Country = Country.NL,
                PostalCode = PostalCode.Parse("2629 JD"),
            };

            DataAnnotationsAssert.IsValid(model);
        }

        [Test]
        public void Validate_PostalCodeModelWithEmptyValues_With2Errors()
        {
            var model = new PostalCodeModel
            {
                Country = Country.Empty,
                PostalCode = PostalCode.Empty,
            };

            DataAnnotationsAssert.WithErrors(model,
                ValidationTestMessage.Error("The Country field is required.", "Country"),
                ValidationTestMessage.Error("The PostalCode field is required.", "PostalCode")
            );
        }

        [Test]
        public void Validate_PostalCodeModelWithInvalidPostalCode_WithError()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var model = new PostalCodeModel
                {
                    Country = Country.BE,
                    PostalCode = PostalCode.Parse("2629 JD"),
                };

                DataAnnotationsAssert.WithErrors(model,
                    ValidationTestMessage.Error("De postcode 2629JD is niet geldig voor België.", "PostalCode", "Country")
                );
            }
        }

        [Test]
        public void Validate_PostalCodeModelWithErrorByService_WithError()
        {
            var validator = new AnnotatedModelValidator(new TestServiceProvider
            {
                { typeof(AddressService), new AddressService() },
            });

            var model = new PostalCodeModel
            {
                Country = Country.NL,
                PostalCode = PostalCode.Parse("2629 XP"),
            };

            DataAnnotationsAssert.WithErrors(model, validator,
                ValidationTestMessage.Error("Postal code does not exist.", "PostalCode")
            );
        }

        [Test]
        public void Validate_ModelWithCustomizedResource_WithError()
        {
            var model = new ModelWithCustomizedResource();
            DataAnnotationsAssert.WithErrors(model,
                ValidationTestMessage.Error("This IBAN is wrong.", "Iban"));
        }

        [Test]
        public void Validate_NestedModelWithNullChild_With1Error()
        {
            var model = new NestedModel()
            {
                Id = Guid.NewGuid()
            };
            DataAnnotationsAssert.WithErrors(model,
                ValidationTestMessage.Error("The Child field is required.", "Child"));
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
                ValidationTestMessage.Error("The Name field is required.", "Child.Name"));
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
                ValidationTestMessage.Error("The Name field is required.", "Children[1].Name"));
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
                ValidationTestMessage.Error("The Name field is required.", "Child.Name"));
        }
    }
}
