using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Qowaiv.Threading
{
    /// <summary>Extensions on System.Threading.Thread.</summary>
    public static class ThreadExtensions
    {
        /// <summary>Gets the value of T.</summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "thread",
            Justification = "ThreadDomain works per thread, and with this extension method, the behavior is virtually extended to System.Threading.Thread.")]
        public static T GetValue<T>(this Thread thread)
        {
            return ThreadDomain.Current.Get<T>();
        }

        /// <summary>Sets the value for T.</summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "thread",
            Justification = "ThreadDomain works per thread, and with this extension method, the behavior is virtually extended to System.Threading.Thread.")]
        public static void SetValue<T>(this Thread thread, T value)
        {
            ThreadDomain.Current.Set(value);
        }

        /// <summary>Removes the value of T.</summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "thread",
            Justification = "ThreadDomain works per thread, and with this extension method, the behavior is virtually extended to System.Threading.Thread.")]
        public static void RemoveValue(this Thread thread, Type type)
        {
            ThreadDomain.Current.Remove(type);
        }
    }
}
