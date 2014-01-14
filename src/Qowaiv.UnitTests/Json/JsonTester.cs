using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qowaiv.Json;

namespace Qowaiv.UnitTests.Json
{
    public class JsonTester
    {
        public static T Read<T>() where T : IJsonSerializable
        {
            T instance = default(T);
            instance.FromJson();
            return instance;
        }
        public static T Read<T>(String val) where T : IJsonSerializable
        {
            T instance = default(T);
            instance.FromJson(val);
            return instance;
        }
        public static T Read<T>(Int64 val) where T : IJsonSerializable
        {
            T instance = default(T);
            instance.FromJson(val);
            return instance;
        }
        public static T Read<T>(Double val) where T : IJsonSerializable
        {
            T instance = default(T);
            instance.FromJson(val);
            return instance;
        }
        public static T Read<T>(DateTime val) where T : IJsonSerializable
        {
            T instance = default(T);
            instance.FromJson(val);
            return instance;
        }

        public static object Write(IJsonSerializable val)
        {
            return val.ToJson();
        }
    }
}
