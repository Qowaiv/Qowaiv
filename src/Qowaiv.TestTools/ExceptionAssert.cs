using System;
using System.Diagnostics;

namespace Qowaiv.TestTools
{
    /// <summary>Helper class for (argument) exceptions.</summary>
    public static class ExceptionAssert
    {
        /// <summary>Catches an argument null exception.</summary>
        /// <param name="code">
        /// The code to execute.
        /// </param>
        /// <param name="paramName">
        /// the expected name of the parameter.
        /// </param>
        /// <returns>
        /// The catch exception.
        /// </returns>
        [DebuggerStepThrough]
        public static ArgumentNullException CatchArgumentNullException(Action code, string paramName)
        {
            var exception = Assert.Catch<ArgumentNullException>(code);
            Assert.AreEqual(paramName, exception.ParamName);
            return exception;
        }

        /// <summary>Catches an argument exception.</summary>
        /// <param name="code">
        /// The code to execute.
        /// </param>
        /// <param name="paramName">
        /// the expected name of the parameter.
        /// </param>
        /// <returns>
        /// <param name="exceptionMessage">
        /// The expected exception message.
        /// </param>
        /// <param name="args">
        /// Array of objects to be used in formatting the message.
        /// </param>
        /// The caught exception.
        /// </returns>
        [DebuggerStepThrough]
        public static ArgumentException CatchArgumentException(Action code, string paramName, string exceptionMessage, params object[] args)
        {
            var exception = Assert.Catch<ArgumentException>(code);
            Assert.AreEqual(paramName, exception.ParamName);
            Assert.AreEqual(GetMessage(paramName, exceptionMessage, args), exception.Message);
            return exception;
        }

        /// <summary>Catches an argument out of range exception.</summary>
        /// <param name="code">
        /// The code to execute.
        /// </param>
        /// <param name="paramName">
        /// the expected name of the parameter.
        /// </param>
        /// <returns>
        /// <param name="exceptionMessage">
        /// The expected exception message.
        /// </param>  
        /// <param name="args">
        /// Array of objects to be used in formatting the message.
        /// </param>
        /// The caught exception.
        /// </returns>
        [DebuggerStepThrough]
        public static ArgumentOutOfRangeException CatchArgumentOutOfRangeException(Action code, string paramName, string exceptionMessage, params object[] args)
        {
            var exception = Assert.Catch<ArgumentOutOfRangeException>(code);
            Assert.AreEqual(paramName, exception.ParamName);
            Assert.AreEqual(GetMessage(paramName, exceptionMessage, args), exception.Message);
            return exception;
        }

        /// <summary>Gets the argument message.</summary>
        private static string GetMessage(string paramName, string exceptionMessage, params object[] args)
        {
            var message = string.Format(exceptionMessage, args);
            if (!string.IsNullOrEmpty(paramName))
            {
                message += string.Format(" (Parameter '{0}')", paramName);
            }
            return message;
        }
    }
}
