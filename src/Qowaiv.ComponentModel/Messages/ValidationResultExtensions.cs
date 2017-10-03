using Qowaiv;
using Qowaiv.ComponentModel.Messages;
using System.Collections.Generic;
using System.Linq;

namespace System.ComponentModel.DataAnnotations
{
	/// <summary>Extensions on <see cref="ValidationResult"/>.</summary>
	public static class ValidationResultExtensions
	{
		/// <summary>Gets the <see cref="ValidationSeverity"/> of the <see cref="ValidationResult"/>.</summary>
		public static ValidationSeverity GetSeverity(this ValidationResult validationResult)
		{
			if(validationResult is ValidationMessage message)
			{
				return message.Severity;
			}
			if(validationResult == null || validationResult == ValidationResult.Success)
			{
				return ValidationSeverity.None;
			}
			return ValidationSeverity.Error;
		}

		/// <summary>Gets all messages excluding <see cref="ValidationSeverity.None"/>.</summary>
		public static IEnumerable<ValidationResult> GetWithSeverity(this IEnumerable<ValidationResult> validationResults)
		{
			Guard.NotNull(validationResults, nameof(validationResults));
			return validationResults.Where(message => message.GetSeverity() != ValidationSeverity.None);
		}

		/// <summary>Gets all messages with <see cref="ValidationSeverity.Error"/>.</summary>
		public static IEnumerable<ValidationResult> GetErrors(this IEnumerable<ValidationResult> validationResults)
		{
			Guard.NotNull(validationResults, nameof(validationResults));
			return validationResults.Where(message => message.GetSeverity() == ValidationSeverity.Error);
		}

		/// <summary>Gets all messages with <see cref="ValidationSeverity.Warning"/>.</summary>
		public static IEnumerable<ValidationResult> GetWarnings(this IEnumerable<ValidationResult> validationResults)
		{
			Guard.NotNull(validationResults, nameof(validationResults));
			return validationResults.Where(message => message.GetSeverity() == ValidationSeverity.Warning);
		}

		/// <summary>Gets all messages with <see cref="ValidationSeverity.Info"/>.</summary>
		public static IEnumerable<ValidationResult> GetInfos(this IEnumerable<ValidationResult> validationResults)
		{
			Guard.NotNull(validationResults, nameof(validationResults));
			return validationResults.Where(message => message.GetSeverity() == ValidationSeverity.Info);
		}

		/// <summary>Gets the distinct member names.</summary>
		public static ISet<string> GetMemberNames(this IEnumerable<ValidationResult> validationResults)
		{
			Guard.NotNull(validationResults, nameof(validationResults));
			return new HashSet<string>(validationResults.SelectMany(validationResult => validationResult.MemberNames));
		}
	}
}
