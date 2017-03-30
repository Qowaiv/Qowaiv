using Qowaiv.Json;
using System;

namespace Qowaiv.UnitTests.Json
{
	public static class JsonTester
	{
		public static T Read<T>() where T : IJsonSerializable
		{
			T instance = default(T);
			if(typeof(T).IsValueType)
			{
				instance.FromJson();
			}
			return instance;
		}
		public static T Read<T>(string val) where T : IJsonSerializable
		{
			T instance = NewInstance<T>();
			instance.FromJson(val);
			return instance;
		}
		public static T Read<T>(long val) where T : IJsonSerializable
		{
			T instance = NewInstance<T>();
			instance.FromJson(val);
			return instance;
		}
		public static T Read<T>(double val) where T : IJsonSerializable
		{
			T instance = NewInstance<T>();
			instance.FromJson(val);
			return instance;
		}
		public static T Read<T>(DateTime val) where T : IJsonSerializable
		{
			T instance = NewInstance<T>();
			instance.FromJson(val);
			return instance;
		}

		private static T NewInstance<T>() where T : IJsonSerializable
		{
			return typeof(T).IsValueType ? default(T) : Activator.CreateInstance<T>();
		}

		public static object Write(IJsonSerializable val)
		{
			return val.ToJson();
		}
	}
}
