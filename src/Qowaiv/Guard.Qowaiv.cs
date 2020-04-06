using System;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv
{
    internal static partial class Guard
    {
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
        {
            NotNull(param, paramName);

            if (!param.GetInterfaces().Contains(typeof(IFormattable)))
            {
                throw new ArgumentException(QowaivMessages.ArgumentException_NotIFormattable, paramName);
            }
            return param;
        }
    }
}
