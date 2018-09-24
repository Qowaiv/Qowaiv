using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Qowaiv.ComponentModel.Rules
{
    /// <summary>Represents a validation rule that is intended to be validated
    /// during <see cref="IValidatableObject.Validate(ValidationContext)"/>.
    /// </summary>
    public abstract class ValidationRule<TModel> : IValidationRule<TModel>
        where TModel : class
    {
        /// <summary>Creates a new instance of a <see cref="ValidationRule"/>.</summary>
        /// <param name="propertyNames">
        /// The involved properties.
        /// </param>
        protected ValidationRule(params string[] propertyNames)
            : this(propertyNames ?? Enumerable.Empty<string>()) { }

        /// <summary>Creates a new instance of a <see cref="ValidationRule"/>.</summary>
        /// <param name="propertyNames">
        /// The involved properties.
        /// </param>
        protected ValidationRule(IEnumerable<string> propertyNames)
        {
            PropertyNames = (propertyNames ?? Enumerable.Empty<string>()).ToArray();
        }

        /// <summary>Gets the involved property names.</summary>
        public string[] PropertyNames { get; }

        /// <summary>Gets and set an error message to if validation fails.</summary>
        public string ErrorMessage { get; set; }

        /// <summary>Gets and set the error message resource name to use in order to look up the System.ComponentModel.DataAnnotations.ValidationAttribute.ErrorMessageResourceType
        /// property value if validation fails.
        /// </summary>
        public string ErrorMessageResourceName { get; set; }

        /// <summary>Gets or sets the resource type to use for error-message lookup if validation fails.</summary>
        public Type ErrorMessageResourceType { get; set; }

        /// <summary>Gets the localized validation error message.</summary>
        protected virtual Func<string> ErrorMessageString => GetResourceErrorMessage() ?? (() => ErrorMessage);

        /// <summary>Gets the message based on the resource.</summary>
        /// <remarks>
        /// This is more or less one-to-on copied from <see cref="ValidationAttribute"/>.
        /// </remarks>
        protected Func<string> GetResourceErrorMessage()
        {
            if (ErrorMessageResourceType == null || string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                return null;
            }
            var property = ErrorMessageResourceType.GetProperty(ErrorMessageResourceName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            if (property != null)
            {
                var propertyGetter = property.GetGetMethod(true /*nonPublic*/);
                // We only support internal and public properties
                if (propertyGetter == null || (!propertyGetter.IsAssembly && !propertyGetter.IsPublic))
                {
                    // Set the property to null so the exception is thrown as if the property wasn't found
                    property = null;
                }
                if (property != null)
                {
                    if (property.PropertyType != typeof(string))
                    {
                        throw new InvalidOperationException(
                            string.Format(
                            CultureInfo.CurrentCulture,
                            QowaivComponentModelMessages.ValidationRule_ResourcePropertyNotStringType,
                            ErrorMessageResourceType.FullName,
                            property.Name));
                    }
                    return () => (string)property.GetValue(null, null);
                }
            }
            throw new InvalidOperationException(
                string.Format(
                CultureInfo.CurrentCulture,
                QowaivComponentModelMessages.ValidationRule_ResourceTypeDoesNotHaveProperty,
                ErrorMessageResourceType.FullName,
                ErrorMessageResourceName));
        }

        /// <inheritdoc />
        public abstract IEnumerable<ValidationResult> Validate(ValidationContext<TModel> validationContext);
    }
}
