using Microsoft.AspNetCore.Mvc.ModelBinding;
using NUnit.Framework;
using Qowaiv.TestTools.Globalization;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Qowaiv.AspNetCore.Mvc.ModelBinding.UnitTests
{
    public class TypeConverterModelBinderTest
    {
        private const string TheModelName = nameof(TheModelName);

        [Test]
        public async Task BindModelAsync_InvalidDate_ErrorMessage()
        {
            using (TestCultures.Nl_NL.Scoped())
            {
                var context = GetBindingContext(typeof(Date));
                context.ValueProvider = new SimpleValueProvider { { TheModelName, "Rubbish" } };

                var binder = new TypeConverterModelBinder();

                await binder.BindModelAsync(context);

                Assert.AreEqual(ModelValidationState.Invalid, context.ModelState.ValidationState);

                var error = context.ModelState[TheModelName].Errors.FirstOrDefault();

                Assert.AreEqual("Geen geldige datum", error.ErrorMessage);
                Assert.IsNull(error.Exception);
            }
        }

        [Test]
        public async Task BindModelAsync_StringEmptyToNullable_Bound()
        {
            var context = GetBindingContext(typeof(Date?));
            context.ValueProvider = new SimpleValueProvider { { TheModelName, "" } };

            var binder = new TypeConverterModelBinder();

            await binder.BindModelAsync(context);

            Assert.AreEqual(ModelValidationState.Unvalidated, context.ModelState.ValidationState);
            Assert.IsNull(context.Result.Model);
        }

        [Test]
        public async Task BindModelAsync_ValidDate_Bound()
        {
            var context = GetBindingContext(typeof(Date?));
            context.ValueProvider = new SimpleValueProvider { { TheModelName, "2017-06-11" } };

            var binder = new TypeConverterModelBinder();

            await binder.BindModelAsync(context);

            Assert.AreEqual(ModelValidationState.Unvalidated, context.ModelState.ValidationState);
            Assert.AreEqual(new Date(2017, 06, 11), context.Result.Model);
        }

        private static DefaultModelBindingContext GetBindingContext(Type modelType)
        {
            return new DefaultModelBindingContext
            {
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = TheModelName,
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider() // empty
            };
        }
    }
}
