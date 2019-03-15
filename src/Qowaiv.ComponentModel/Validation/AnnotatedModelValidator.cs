using Qowaiv.ComponentModel.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            var annotations = AnnotatedModel.Get(model.GetType());
            var messages = new List<ValidationResult>();
            messages.AddRange(ValidateModel(model, validationContext, annotations));

            return Result.For(model, messages);
        }

        private IEnumerable<ValidationResult> ValidateModel(object model, ValidationContext validationContext, AnnotatedModel annotations)
        {
            return ValidateProperties(model, annotations)
                .Concat(ValidateType(model, annotations, validationContext))
                .Concat(ValidateValidatableObject(model, annotations, validationContext));
        }

        /// <summary>Gets the results for validating the (annotated )properties.</summary>
        private IEnumerable<ValidationResult> ValidateProperties(object model, AnnotatedModel annotations)
        {
            return annotations.Properties.SelectMany(prop => ValidateProperty(prop, model));
        }

        /// <summary>Gets the results for validating a single annotated property.</summary>
        /// <remarks>
        /// It creates a sub validation context.
        /// </remarks>
        private IEnumerable<ValidationResult> ValidateProperty(AnnotatedProperty property, object model)
        {
            var value = property.GetValue(model);
            var propertyContext = CreatePropertyContext(model, property);

            var isRequiredMessage = property.RequiredAttribute.GetValidationResult(value, propertyContext);
            yield return isRequiredMessage;

            // Only validate the other properties if the required condition was not met.
            if (isRequiredMessage.GetSeverity() != ValidationSeverity.Error)
            {
                foreach (var attribute in property.ValidationAttributes)
                {
                    yield return attribute.GetValidationResult(value, propertyContext);
                }

                if (value != null && property.TypeIsAnnotatedModel)
                {
                    ValidationContext context = new ValidationContext(value, ServiceProvider, Items);
                    var annotations = AnnotatedModel.Get(value.GetType());
                    var messages = ValidateModel(value, context, annotations);

                    //// validated nested.
                    foreach (var message in messages)
                    {
                        yield return message;
                    }
                }

            }
        }

        /// <summary>Gets the results for validating the attributes declared on the type of the model.</summary>
        private static IEnumerable<ValidationResult> ValidateType(object model, AnnotatedModel annotations, ValidationContext validationContext)
        {
            return annotations.TypeAttributes.Select(attr => attr.GetValidationResult(model, validationContext));
        }

        /// <summary>Gets the results for validating <see cref="IValidatableObject.Validate(ValidationContext)"/>.</summary>
        /// <remarks>
        /// If the model is not <see cref="IValidatableObject"/> nothing is done.
        /// </remarks>
        private static IEnumerable<ValidationResult> ValidateValidatableObject(object model, AnnotatedModel annotations, ValidationContext validationContext)
        {
            return annotations.IsIValidatableObject
                ? ((IValidatableObject)model).Validate(validationContext)
                : Enumerable.Empty<ValidationResult>();
        }

        private ValidationContext CreatePropertyContext(object model, AnnotatedProperty property)
        {
            var propertyContext = new ValidationContext(model, ServiceProvider, Items)
            {
                MemberName = property.Descriptor.Name
            };
            return propertyContext;
        }
    }
}
