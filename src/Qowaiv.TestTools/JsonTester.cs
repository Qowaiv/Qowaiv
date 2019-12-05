using Qowaiv.Json;
using System;

namespace Qowaiv.TestTools
{
    /// <summary>Helper class for accessing the <see cref="IJsonSerializable"/> interface.</summary>
    public static class JsonTester
    {
        /// <summary>Applies <see cref="IJsonSerializable.FromJson()"/>.</summary>
        [Obsolete("Drop test case.")]
        public static T Read<T>() where T : IJsonSerializable
        {
            T instance = default;
            if (typeof(T).IsValueType)
            {
                instance.FromJson();
            }
            return instance;
        }

        /// <summary>Applies <see cref="IJsonSerializable.FromJson(string)"/>.</summary>
        [Obsolete("Use Read<T>(object) instead.")]
        public static T Read<T>(string val) where T : IJsonSerializable
        {
            T instance = NewInstance<T>();
            instance.FromJson(val);
            return instance;
        }

        /// <summary>Applies <see cref="IJsonSerializable.FromJson(long)"/>.</summary>
        [Obsolete("Use Read<T>(object) instead.")]
        public static T Read<T>(long val) where T : IJsonSerializable
        {
            T instance = NewInstance<T>();
            instance.FromJson(val);
            return instance;
        }

        /// <summary>Applies <see cref="IJsonSerializable.FromJson(double)"/>.</summary>
        [Obsolete("Use Read<T>(object) instead.")]
        public static T Read<T>(double val) where T : IJsonSerializable
        {
            T instance = NewInstance<T>();
            instance.FromJson(val);
            return instance;
        }

        /// <summary>Applies <see cref="IJsonSerializable.FromJson(DateTime)"/>.</summary>
        [Obsolete("Use Read<T>(object) instead.")]
        public static T Read<T>(DateTime val) where T : IJsonSerializable
        {
            T instance = NewInstance<T>();
            instance.FromJson(val);
            return instance;
        }

        /// <summary>Applies <see cref="IJsonSerializable.FromJson(object)"/>.</summary>
        public static T Read<T>(object val) where T : IJsonSerializable
        {
            T instance = NewInstance<T>();
            instance.FromJson(val);
            return instance;
        }

        /// <summary>Applies <see cref="IJsonSerializable.ToJson()"/>.</summary>
        public static object Write(IJsonSerializable val)
        {
            return val?.ToJson();
        }

        private static T NewInstance<T>() where T : IJsonSerializable
        {
            return typeof(T).IsValueType ? default : Activator.CreateInstance<T>();
        }

    }
}
