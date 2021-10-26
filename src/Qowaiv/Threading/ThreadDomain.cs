using Qowaiv.Financial;
using Qowaiv.Globalization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Qowaiv.Threading
{
    /// <summary>Represents domain of typed instances that can be used as the
    /// default values based on the current thread.
    /// </summary>
    public class ThreadDomain
    {
        /// <summary>Initializes creators.</summary>
        static ThreadDomain()
        {
            Register(typeof(Country), (Thread) => Country.Create(Thread.CurrentCulture));
            Register(typeof(Currency), (Thread) => Thread.GetValue<Country>().GetCurrency(Date.Today));
        }

        /// <summary>Gets the current thread domain.</summary>
        /// <remarks>
        /// There is one domain per thread.
        /// </remarks>
        public static ThreadDomain Current
        {
            get
            {
                if (s_Current == null)
                {
                    s_Current = new ThreadDomain();
                }
                return s_Current;
            }
        }
        [ThreadStatic]
        private static ThreadDomain s_Current;

        /// <summary>Registers a creator function for the type that can create 
        /// an instance of type based on a thread.
        /// </summary>
        /// <param name="type">
        /// The type to register.
        /// </param>
        /// <param name="creator">
        /// The creator function.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the type or the creator is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// If the type is generic, or has no converter, or a convertor that can not
        /// convert from string.
        /// </exception>
        /// <remarks>
        /// These creators are used to generate a value, if no one is
        /// assigned or configured.
        /// </remarks>
        public static void Register(Type type, Func<Thread, object> creator)
        {
            Qowaiv.Guard.NotNull(type, nameof(type));
            Qowaiv.Guard.NotNull(creator, nameof(creator));
            Guard(type, Creators.ContainsKey(type));

            Creators.TryAdd(type, creator);
        }
        private static readonly ConcurrentDictionary<Type, Func<Thread, object>> Creators = new ConcurrentDictionary<Type, Func<Thread, object>>();

        /// <summary>Constructor.</summary>
        /// <remarks>
        /// No public accessor.
        /// </remarks>
        protected ThreadDomain()
        {
            Values = new Dictionary<Type, object>();
        }

        /// <summary>The underlying dictionary.</summary>
        protected Dictionary<Type, object> Values { get; private set; }

        /// <summary>Gets the current value of T.</summary>
        /// <typeparam name="T">
        /// The type of T.
        /// </typeparam>
        /// <exception cref="NotSupportedException">
        /// If the type is generic, or has no converter, or a convertor that can not
        /// convert from string.
        /// </exception>
        [Pure]
        public T Get<T>()
        {
            var type = Guard(typeof(T), Values.ContainsKey(typeof(T)));


            if (!Values.TryGetValue(type, out object value))
            {
                if (Creators.TryGetValue(type, out Func<Thread, object> func))
                {
                    value = func.Invoke(Thread.CurrentThread);
                }
                else
                {
                    value = default(T);
                }
            }
            return (T)value;
        }

        /// <summary>Sets the value for T.</summary>
        /// <typeparam name="T">
        /// The type of T.
        /// </typeparam>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// If the type is generic, or has no converter, or a convertor that can not
        /// convert from string.
        /// </exception>
        public void Set<T>(T value)
        {
            var type = Guard(typeof(T), Values.ContainsKey(typeof(T)));
            Values[type] = value;
        }

        /// <summary>Removes the value from the thread domain.</summary>
        /// <param name="type">
        /// The type to remove the value from.
        /// </param>
        public void Remove(Type type)
        {
            Qowaiv.Guard.NotNull(type, nameof(type));
            Values.Remove(type);
        }

        /// <summary>Clears all values from the thread domain.</summary>
        public void Clear() => Values.Clear();

        /// <summary>Guards the type.</summary>
        /// <exception cref="NotSupportedException">
        /// If the type is generic, or has no converter, or a convertor that can not
        /// convert from string.
        /// </exception>
        [Pure]
        private static Type Guard(Type type, bool inCollection)
        {
            if (!inCollection)
            {
                if (type.IsGenericType) { throw new NotSupportedException(QowaivMessages.NotSupportedException_NoGenericType); }
                var converter = TypeDescriptor.GetConverter(type);
                if (!converter.CanConvertFrom(typeof(string))) { throw new NotSupportedException(QowaivMessages.NotSupportedException_ConverterCanNotConvertFromString); }
            }
            return type;
        }
    }
}
