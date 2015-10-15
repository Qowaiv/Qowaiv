using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qowaiv.CodeGenerator
{
	/// <summary>Supplies input parameter guarding.</summary>
	internal static class Guard
	{
		/// <summary>Guards the parameter if not null, otherwise throws an argument (null) exception.</summary>
		/// <typeparam name="T">
		/// The type to guard, can not be a structure.
		/// </typeparam>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static T NotNull<T>([ValidatedNotNull]T param, string paramName) where T : class
		{
			if (object.ReferenceEquals(param, null))
			{
				throw new ArgumentNullException(paramName);
			}
			return param;
		}

		/// <summary>Guards the parameter if not null or an empty string, otherwise throws an argument (null) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static string NotNullOrEmpty([ValidatedNotNull]string param, string paramName)
		{
			NotNull(param, paramName);
			if (string.Empty.Equals(param))
			{
				throw new ArgumentException(QowaivMessages.ArgumentException_StringEmpty, paramName);
			}
			return param;
		}

		/// <summary>Marks the NotNull argument as being validated for not being null,
		/// to satisfy the static code analysis.
		/// </summary>
		/// <remarks>
		/// Notice that it does not matter what this attribute does, as long as
		/// it is named ValidatedNotNullAttribute.
		/// </remarks>
		[AttributeUsage(AttributeTargets.Parameter)]
		sealed class ValidatedNotNullAttribute : Attribute { }
	}
}
