using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
        /// <param name="iface">
        /// The interface to test for.
        /// </param>
        /// <param name="message">
        /// The message to show if the interface is not implemented.
        /// </param>
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
    }
}
