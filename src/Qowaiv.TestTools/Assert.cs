using System;
using System.Diagnostics;
using System.Text;

namespace Qowaiv.TestTools
{
    /// <summary>Minimized assert helper class, to prevent dependencies on test frameworks.</summary>
    internal static class Assert
    {
        public static TException Catch<TException>(Action code) where TException : Exception
        {
            try
            {
                code();
            }
            catch (Exception x)
            {
                if (x is TException caught)
                {
                    return caught;
                }
                Fail($"Expected a {typeof(TException)} to be thrown, but a {x.GetType()} has been.");
            }
            Fail($"Expected a {typeof(TException)} to be thrown, but no exception has been.");
            return default;
        }

        [DebuggerStepThrough]
        public static void IsNotNull(object obj, string message = null)
        {
            if (obj is null)
            {
                Fail(message);
            }
        }

        [DebuggerStepThrough]
        public static void IsNull(object obj, string message)
        {
            if (!(obj is null))
            {
                Fail(message);
            }
        }

        [DebuggerStepThrough]
        public static void AreEqual(object expected, object actual, string message = null)
        {
            if (!Equals(expected, actual))
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Expected: {0}", expected).AppendLine();
                sb.AppendFormat("Actual:   {0}", actual).AppendLine();

                if (!string.IsNullOrEmpty(message))
                {
                    sb.AppendLine(message);
                }
                Fail(sb.ToString());
            }
        }

        [DebuggerStepThrough]
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                Fail(message);
            }
        }

        [DebuggerStepThrough]
        public static void Fail(string message)
        {
            throw new AssertException(message);
        }
    }
}
