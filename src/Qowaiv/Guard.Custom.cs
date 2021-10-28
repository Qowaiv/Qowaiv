using System;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv
{
    internal static partial class Guard
    {
        /// <summary>Guards the parameter if the type is not null and implements the specified interface,
        /// otherwise throws an argument (null) exception.
        /// </summary>
        /// <param name="param">
        /// The parameter to guard.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter.
        /// </param>
        /// <param name="interface">
        /// The interface to test for.
        /// </param>
        /// <param name="message">
        /// The message to show if the interface is not implemented.
        /// </param>
        [DebuggerStepThrough]
        public static Type ImplementsInterface(Type param, string paramName, Type @interface, string message)
            => NotNull(param, paramName).GetInterfaces().Contains(@interface)
            ? param 
            : throw new ArgumentException(message, paramName);

        /// <summary>Guards the parameter implements <see cref="IFormattable"/>,
        /// otherwise throws an argument (null) exception.
        /// </summary>
        /// <param name="param">
        /// The parameter to guard.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter.
        /// </param>
        [DebuggerStepThrough]
        public static Type ImplementsIFormattable(Type param, string paramName)
            => NotNull(param, paramName).GetInterfaces().Contains(typeof(IFormattable))
            ? param
            : throw new ArgumentException(QowaivMessages.ArgumentException_NotIFormattable, paramName);
    }
}
