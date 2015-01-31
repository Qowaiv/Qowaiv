using System;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv
{
	/// <summary>Supplies input parameter guarding.</summary>
	public static class Guard
	{
		/// <summary>Guards the parameter if not null, otherwise throws an argument null exception.</summary>
		/// <typeparam name="T">
		/// The type to guard, can not be a struct.
		/// </typeparam>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static T NotNull<T>(T param, string paramName) where T : class
		{
			if (object.ReferenceEquals(param, null))
			{
				throw new ArgumentNullException(paramName);
			}
			return param;
		}

		/// <summary>Guards the parameter if not null or string empty, otherwise throws an argument null exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static string NotNullOrEmpty(string param, string paramName)
		{
			NotNull(param, paramName);
			if (string.Empty.Equals(param))
			{
				throw new ArgumentException(QowaivMessages.ArgumentException_NotStringEmpty, paramName);
			}
			return param;
		}

		/// <summary>Guards the paramater if the type is not null and implements the specified interface,
		/// otherwise throws an argument (null) exception.
		/// </summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		/// <param name="iface">
		/// The interface to test for.
		/// </param>
		/// <param name="message">
		/// The message to show if the interface is not implemented.
		/// </param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static Type ImplementsInterface(Type param, string paramName, Type iface, string message)
		{
			Guard.NotNull(param, paramName);

			if (!param.GetInterfaces().Contains(iface))
			{
				throw new ArgumentException(message, paramName);
			}
			return param;
		}
	}
}
