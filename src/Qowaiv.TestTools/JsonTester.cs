﻿using System;
using System.Linq;
using System.Reflection;

namespace Qowaiv.TestTools
{
    /// <summary>Helper class for testing JSON conversion.</summary>
    public static class JsonTester
    {
        /// <summary>Applies multiple FromJson scenario's.</summary>
        public static T Read<T>(object val)
        {
            var parameterType = val?.GetType();
            var fromJson = typeof(T)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => FromJson<T>(m, parameterType));

            if (fromJson is null)
            {
                throw new InvalidOperationException($"Could not find {typeof(T).Name}.FromJson({parameterType.Name}).");
            }
            try
            {
                return (T)fromJson.Invoke(null, new[] { val });
            }
            catch (TargetInvocationException x)
            {
                if (x.InnerException is null)
                {
                    throw;
                }
                throw x.InnerException;
            }
        }

        /// <summary>Applies <code>ToJson()</code>.</summary>
        public static object Write<T>(T val)
        {
            var toJson = typeof(T)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(ToJson);

            if (toJson is null)
            {
                throw new InvalidOperationException($"Could not find {typeof(T).Name}.ToJson().");
            }
            return toJson.Invoke(val, Array.Empty<object>());
        }

        private static bool FromJson<T>(MethodInfo method, Type parameterType)
        {
            return method.Name == nameof(FromJson)
                && method.GetParameters().Length == 1
                && method.GetParameters()[0].ParameterType == parameterType
                && method.ReturnType == typeof(T)
            ;
        }

        private static bool ToJson(MethodInfo method)
        {
            return method.Name == nameof(ToJson)
                && method.GetParameters().Length == 0
                && method.ReturnType != null
            ;
        }
    }
}
