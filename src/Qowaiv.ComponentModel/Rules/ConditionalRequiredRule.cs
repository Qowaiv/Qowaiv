using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.ComponentModel.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Qowaiv.ComponentModel.Rules
{
    /// <summary>Helper for <see cref="ConditionalRequiredRule{TModel}"/>.</summary>
    public static class ConditionalRequiredRule
    {
        /// <summary>Creates a conditional reqruied rule for the specified model.</summary>
        /// <typeparam name="TModel">
        /// The model to validate.
        /// </typeparam>
        /// <param name="condition">
        /// The condition that makes the linked property/properties required.
        /// </param>
        /// <param name="properties">
        /// The expressions to select properties that should be required.
        /// </param>
        public static ConditionalRequiredRule<TModel> For<TModel>
        (
            Func<ValidationContext<TModel>, bool> condition,
            params Expression<Func<TModel, object>>[] properties
        ) where TModel: class
        {
            Guard.NotNull(condition, nameof(condition));
            Guard.HasAny(properties, nameof(properties));

            var names = properties
                .Select(prop => ExpressionRule.GetMemberName(prop))
                .ToArray();

            var getValues = properties
                .Select(prop => prop.Compile())
                .ToArray();

            return new ConditionalRequiredRule<TModel>(condition, getValues, names);
        }
    }

    /// <summary>Represents a conditional rule, that marks properties as being required based on a condition.</summary>
    public class ConditionalRequiredRule<TModel> : ValidationRule<TModel>
        where TModel : class
    {
        /// <summary>Creates a new instance of a <see cref="ConditionalRequiredRule{TModel}"/>.</summary>
        internal ConditionalRequiredRule
        (
            Func<ValidationContext<TModel>, bool> condition, 
            Func<TModel, object>[] getValues, 
            string[] propertyNames
        )
            : base(propertyNames)
        {
            getCondition = condition;
            GetValues = getValues;
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext<TModel> validationContext)
        {
            if(getCondition(validationContext))
            {
                for(var index = 0; index < PropertyNames.Length; index++)
                {
                    var value = GetValues[index](validationContext.Model);
                    var name = PropertyNames[index];

                    if ((MandatoryAttribute.IsApplicable(value) && !mandatory.IsValid(value))
                        || (AnyAttribute.IsApplicable(value) && !any.IsValid(value))
                        || !required.IsValid(value))
                    {
                        yield return ValidationMessage.Error(required.FormatErrorMessage(name), name);
                    }
                }
            }
        }

        private readonly Func<ValidationContext<TModel>, bool> getCondition;
        private readonly Func<TModel, object>[] GetValues;

        private readonly AnyAttribute any = new AnyAttribute();
        private readonly MandatoryAttribute mandatory = new MandatoryAttribute();
        private readonly RequiredAttribute required = new RequiredAttribute();
    }
}
