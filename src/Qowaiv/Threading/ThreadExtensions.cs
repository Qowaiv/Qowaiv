#pragma warning disable IDE0060 // Remove unused parameter
// ThreadDomain works per thread, and with this extension method, the behavior is virtually extended to System.Threading.Thread.

using System;
using System.Threading;

namespace Qowaiv.Threading
{
    /// <summary>Extensions on System.Threading.Thread.</summary>
    public static class ThreadExtensions
    {
        /// <summary>Gets the value of T.</summary>
        public static T GetValue<T>(this Thread thread) => ThreadDomain.Current.Get<T>();

        /// <summary>Sets the value for T.</summary>
        public static void SetValue<T>(this Thread thread, T value) => ThreadDomain.Current.Set(value);

        /// <summary>Removes the value of T.</summary>
        public static void RemoveValue(this Thread thread, Type type) => ThreadDomain.Current.Remove(type);
    }
}
