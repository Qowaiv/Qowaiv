using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Qowaiv.UnitTests.TestTools
{
    public static class ExceptionAssert
    {
        private static TException ExpectExceptionTestHelper<TException>(Action del) where TException : Exception
        {
            return ExpectExceptionTestHelper<TException>(del, false);
        }

        private static TException ExpectExceptionTestHelper<TException>(Action del, bool allowDerivedExceptions)
            where TException : Exception
        {
            try
            {
                del();
                Assert.Fail("Expected exception of type " + typeof(TException) + ".");
                throw new InvalidOperationException("can't happen");
            }
            // This catch is NOT executed in DEBUG mode !! 
            // This is fixed in a generic catch below...
            catch (TException e)
            {
                if (!allowDerivedExceptions)
                {
                    Assert.AreEqual(typeof(TException), e.GetType());
                }
                return e;
            }
            catch (TargetInvocationException e)
            {
                TException te = e.InnerException as TException;
                if (te == null)
                {
                    // Rethrow if it's not the right type
                    throw;
                }
                if (!allowDerivedExceptions)
                {
                    Assert.AreEqual(typeof(TException), te.GetType());
                }
                return te;
            }
            catch (Exception e)
            {
                TException ex = e as TException;
                if (ex != null)
                {
                    // This only happens in DEBUG mode; see comment above
                    if (!allowDerivedExceptions)
                    {
                        Assert.AreEqual(typeof(TException), e.GetType());
                    }
                    return ex;
                }
                throw;
            }
        }

        /// <summary>
        /// Assert een exception van type TException, maar geen Exceptions die afgeleid zijn van TException
        /// </summary>
        /// <typeparam name="TException">Het type van de verwachte exception.</typeparam>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <returns>De gegooide exception.</returns>
        public static TException ExpectException<TException>(Action del) where TException : Exception
        {
            return ExpectException<TException>(del, false);
        }

        /// <summary>
        /// Assert een exception van type TException.
        /// </summary>
        /// <typeparam name="TException">Het type van de verwachte exception.</typeparam>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="allowDerivedExceptions">Sta toe dat <c>del</c> een afgeleide exception van TException gooit.</param>
        /// <returns>De gegooide exception.</returns>
        public static TException ExpectException<TException>(Action del, bool allowDerivedExceptions)
            where TException : Exception
        {
            if (typeof(ArgumentNullException).IsAssignableFrom(typeof(TException)))
            {
                throw new InvalidOperationException(
                    "ExpectException<TException>() cannot be used with exceptions of type 'ArgumentNullException'. " +
                    "Use ExpectArgumentNullException() instead.");
            }
            else if (typeof(ArgumentException).IsAssignableFrom(typeof(TException)))
            {
                throw new InvalidOperationException(
                    "ExpectException<TException>() cannot be used with exceptions of type 'ArgumentException'. " +
                    "Use ExpectArgumentException() instead.");
            }
            return ExpectExceptionTestHelper<TException>(del, allowDerivedExceptions);
        }

        /// <summary>
        /// Assert een exception van type TException met een specifieke message, maar geen Exceptions die afgeleid zijn van TException.
        /// </summary>
        /// <typeparam name="TException">Het type van de verwachte exception.</typeparam>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="exceptionMessage">De verwachte exceptionmessage.</param>
        /// <param name="args">Format arguments</param>
        /// <returns>De gegooide exception.</returns>
        public static TException ExpectException<TException>(Action del,
            string exceptionMessage, params object[] args)
            where TException : Exception
        {
            TException e = ExpectException<TException>(del);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, exceptionMessage, args), e.Message, "Incorrect exception message.");
            return e;
        }

        /// <summary>
        /// Assert een exception van type TException, maar geen Exceptions die afgeleid zijn van TException,
        /// waarbij <paramref  name="exceptionMessage"/> voorkomt in de exceptionmessage.
        /// </summary>
        /// <typeparam name="TException">Het type van de verwachte exception.</typeparam>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="args">Format arguments</param>
        /// <returns>De gegooide exception.</returns>
        public static TException ExpectExceptionContains<TException>(Action del,
            string exceptionMessage, params object[] args)
            where TException : Exception
        {
            TException e = ExpectException<TException>(del);
            StringAssert.Contains(e.Message, string.Format(CultureInfo.CurrentCulture, exceptionMessage, args), "Incorrect exception message.");
            return e;
        }

        /// <summary>
        /// Assert een ArgumentException op <paramref name="paramName"/> met message exceptionMessage.
        /// </summary>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="paramName">Naam van de parameter die een foutieve waarde heeft meegekregen.</param>
        /// <param name="exceptionMessage">De exceptionmessage.</param>
        /// <param name="args">Format arguments</param>
        /// <returns>De gegooide exception.</returns>
        public static ArgumentException ExpectArgumentException(Action del, string paramName,
            string exceptionMessage, params object[] args)
        {
            ArgumentException e = ExpectExceptionTestHelper<ArgumentException>(del);
           
            exceptionMessage = string.Format(CultureInfo.CurrentCulture, exceptionMessage, args);
            string expectedExceptionMessage = paramName == null ? exceptionMessage :
                string.Format(CultureInfo.InvariantCulture, "{0}\r\nParameter name: {1}", exceptionMessage, paramName);
            Assert.AreEqual(expectedExceptionMessage, e.Message, "Incorrect exception message.");
            return e;
        }

        /// <summary>
        /// Assert een ArgumentNullException waarbij de message "Value cannot be null or empty." is op <paramref name="paramName"/>.
        /// </summary>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="paramName">Naam van de parameter die niet null of leeg mag zijn.</param>
        /// <returns>De gegooide exception.</returns>
        public static ArgumentException ExpectArgumentExceptionNullOrEmpty(Action del, string paramName)
        {
            return ExpectArgumentException(del, paramName, "Value cannot be null or empty.");
        }

        /// <summary>
        /// Assert een ArgumentNullException op <paramref name="paramName"/>.
        /// </summary>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="paramName">Naam van de parameter die niet null mag zijn.</param>
        /// <returns>De gegooide exception.</returns>
        public static ArgumentNullException ExpectArgumentNullException(Action del, string paramName)
        {
            ArgumentNullException e = ExpectExceptionTestHelper<ArgumentNullException>(del);
            Assert.AreEqual(paramName, e.ParamName, "Incorrect exception parameter name.");
            return e;
        }

        /// <summary>
        /// Assert een ArgumentException op <paramref name="paramName"/> when empty.
        /// </summary>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="paramName">Naam van de parameter die niet null mag zijn.</param>
        /// <returns>De gegooide exception.</returns>
        public static ArgumentException ExpectArgumentEmptyException(Action del, string paramName)
        {
            ArgumentException e = ExpectExceptionTestHelper<ArgumentException>(del);
            Assert.AreEqual(paramName, e.ParamName, "Empty string parameter not allowed");
            return e;
        }

        /// <summary>
        /// Assert een ArgumentOutOfRangeException op <paramref name="paramName"/>.
        /// </summary>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="paramName">Naam van de parameter die niet binnen een bepaald bereik valt.</param>
        /// <param name="exceptionMessage">De exceptionMessage.</param>
        /// <param name="args">Format arguments</param>
        /// <returns>De gegooide exception.</returns>
        public static ArgumentOutOfRangeException ExpectArgumentOutOfRangeException(Action del,
            string paramName, string exceptionMessage, params object[] args)
        {
            ArgumentOutOfRangeException e = ExpectExceptionTestHelper<ArgumentOutOfRangeException>(del);
            if (exceptionMessage != null)
            {
                exceptionMessage = string.Format(CultureInfo.CurrentCulture, exceptionMessage, args);
                string expectedExceptionMessage = paramName == null ? exceptionMessage :
                    string.Format(CultureInfo.InvariantCulture, "{0}\r\nParameter name: {1}", exceptionMessage, paramName);
                Assert.AreEqual(expectedExceptionMessage, e.Message, "Incorrect exception message.");
            }
            return e;
        }

        /// <summary>
        /// Assert een InvalidOperationException.
        /// </summary>
        /// <param name="del">Een delegate (of lambda) die de exception gooit.</param>
        /// <param name="exceptionMessage">De exceptionMessage.</param>
        /// <param name="args">Format arguments</param>
        /// <returns>De gegooide exception.</returns>
        public static InvalidOperationException ExpectInvalidOperationException(Action del,
            string exceptionMessage, params object[] args)
        {
            InvalidOperationException e = ExpectExceptionTestHelper<InvalidOperationException>(del);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, exceptionMessage, args), e.Message, "Incorrect exception message.");
            return e;
        }
    }
}
