using Qowaiv.ComponentModel.Messages;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.Rules
{
    /// <summary>Represents a set of <see cref="IValidationRule{TModel}"/>s.</summary>
    public class ValidationRuleSet<TModel> : IEnumerable<IValidationRule<TModel>> where TModel : class
    {
        /// <summary>Creates a new instance of a <see cref="ValidationRuleSet{TModel}"/>.</summary>
        public ValidationRuleSet(params IValidationRule<TModel>[] rules)
        {
            this.rules = Guard.NotNull(rules, nameof(rules)).ToArray();
        }

        /// <summary>Gets the number of rules in the rule set.</summary>
        public int Count => rules.Length;

        /// <summary>Validates all <see cref="IValidationRule{TModel}"/>s.</summary>

        /// <param name="validationContext">
        /// The <see cref="ValidationContext"/> 
        /// The validation context
        /// </param>
        /// <returns>
        /// All <see cref="ValidationResult"/> that are not equal to <see cref="ValidationMessage.None"/>.
        /// </returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Guard.NotNull(validationContext, nameof(validationContext));

            return rules
                .SelectMany(rule => rule.Validate(validationContext.Typed<TModel>()))
                .Where(message => message.GetSeverity() > ValidationSeverity.None);
        }

        #region IEnumerable

        private readonly IValidationRule<TModel>[] rules;

        /// <inheritdoc />
        public IEnumerator<IValidationRule<TModel>> GetEnumerator()
        {
            return ((IEnumerable<IValidationRule<TModel>>)rules).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
