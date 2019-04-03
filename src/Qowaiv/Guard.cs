using System;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv
{
    /// <summary>Supplies input parameter guarding.</summary>
    /// <remarks>
    /// Supplying a Guard mechanism is not something that belongs in Qowaiv. So, 
    /// although is a nice feature, we don't provide it anymore as we would have to
    /// add methods just because the sake of being complete.
    /// </remarks>
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
            if (param is null)
            {
                throw new ArgumentNullException(paramName);
            }
            return param;
        }

        /// <summary>Guards the parameter if not the default value, otherwise throws an argument exception.</summary>
        /// <typeparam name="T">
        /// The type to guard, can not be a class.
        /// </typeparam>
        /// <param name="param">
        /// The parameter to guard.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter.
        /// </param>
        [DebuggerStepThrough]
        public static T NotDefault<T>(T param, string paramName) where T : struct
        {
            if (default(T).Equals(param))
            {
                throw new ArgumentException(QowaivMessages.ArgumentException_IsDefaultValue, paramName);
            }
            return param;
        }

        /// <summary>Guards the parameter if not <see cref="Guid.Empty"/>, otherwise throws an argument exception.</summary>
        /// <param name="param">
        /// The parameter to guard.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter.
        /// </param>
        [DebuggerStepThrough]
        public static Guid NotEmpty(Guid param, string paramName)
        {
            if (Guid.Empty.Equals(param))
            {
                throw new ArgumentException(QowaivMessages.ArgumentException_IsGuidEmpty, paramName);
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
            if (string.Empty == param)
            {
                throw new ArgumentException(QowaivMessages.ArgumentException_StringEmpty, paramName);
            }
            return param;
        }

        /// <summary>Guards the parameter if not null or an empty array, otherwise throws an argument (null) exception.</summary>
        /// <param name="param">
        /// The parameter to guard.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter.
        /// </param>
        [DebuggerStepThrough]
        public static T[] HasAny<T>([ValidatedNotNull]T[] param, string paramName)
        {
            NotNull(param, paramName);
            if (param.Length == 0)
            {
                throw new ArgumentException(QowaivMessages.ArgumentException_EmptyArray, paramName);
            }
            return param;
        }

        /// <summary>Guards the parameter to be of the specified type, otherwise throws an argument exception.</summary>
        /// <param name="param">
        /// The parameter to guard.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter.
        /// </param>
        [DebuggerStepThrough]
        public static T IsTypeOf<T>([ValidatedNotNull]object param, string paramName)
        {
            if (param is T)
            {
                return (T)param;
            }
            throw new ArgumentException(string.Format(QowaivMessages.ArgumentException_Must, typeof(T)), paramName);
        }

        /// <summary>Guards the parameter if the type is not null and implements the specified interface,
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
            NotNull(param, paramName);

            if (!param.GetInterfaces().Contains(iface))
            {
                throw new ArgumentException(message, paramName);
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
