using Qowaiv.ComponentModel.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>A validator to validate models based on their data annotations.</summary>
    public class AnnotatedModelValidator
    {
        /// <summary>Creates a new instance of a <see cref="AnnotatedModelValidator"/>.</summary>
        public AnnotatedModelValidator() : this(null, null) { }

        /// <summary>Creates a new instance of a <see cref="AnnotatedModelValidator"/>.</summary>
        /// <param name="serviceProvider">
        /// The object that implements the System.IServiceProvider interface. This parameter is optional.
        /// </param>
        public AnnotatedModelValidator(IServiceProvider serviceProvider)
            : this(serviceProvider, null) { }

        /// <summary>Creates a new instance of a <see cref="AnnotatedModelValidator"/>.</summary>
        /// <param name="items">
        /// A dictionary of key/value pairs to make available to the service consumers. This parameter is optional.
        /// </param>
        public AnnotatedModelValidator(IDictionary<object, object> items)
            : this(null, items) { }

        /// <summary>Creates a new instance of a <see cref="AnnotatedModelValidator"/>.</summary>
        /// <param name="serviceProvider">
        /// The object that implements the System.IServiceProvider interface. This parameter is optional.
        /// </param>
        /// <param name="items">
        /// A dictionary of key/value pairs to make available to the service consumers. This parameter is optional.
        /// </param>
        public AnnotatedModelValidator(IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            ServiceProvider = serviceProvider;
            Items = items;
        }

        /// <summary>Gets the <see cref="IServiceProvider"/>.</summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>Gets the <see cref="IServiceProvider"/>.</summary>
        protected IDictionary<object, object> Items { get; }

        /// <summary>Validates the model.</summary>
        /// <returns>
        /// A result including the model and the <see cref="ValidationResult"/>s.
        /// </returns>
        public Result<T> Validate<T>(T model)
        {
            var validationContext = new ValidationContext(model, ServiceProvider, Items);
            var annotatedModel = AnnotatedModelStore.Instance.GetAnnotededModel(model.GetType());
            return Validate(new Result<T>(model), validationContext, annotatedModel);
        }

        private Result<T> Validate<T>(Result<T> result, ValidationContext validationContext, AnnotatedModel annotations)
        {
            foreach (var property in annotations.Properties)
            {
                ValidateProperty(result, validationContext, property);
            }
            foreach (var attribute in annotations.TypeAttributes)
            {
                result.Add(attribute.GetValidationResult(result.Data, validationContext));
            }
            if (annotations.IsIValidatableObject)
            {
                result.AddRange(((IValidatableObject)result.Data).Validate(validationContext));
            }
            return result;
        }

        private static void ValidateProperty<T>(Result<T> result, ValidationContext validationContext, AnnotatedProperty property)
        {
            var value = property.GetValue(result.Data);
            var propertyContext = property.CreateValidationContext(result.Data, validationContext);

            // The required condition was not met.
            if (result.Add(property.RequiredAttribute.GetValidationResult(value, propertyContext)))
            {
                return;
            }
            foreach (var attribute in property.ValidationAttributes)
            {
                result.Add(attribute.GetValidationResult(value, propertyContext));
            }
        }
    }
}
