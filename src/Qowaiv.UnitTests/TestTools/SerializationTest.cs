using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.TestTools
{
	/// <summary>Helps with serialization testing.</summary>
	public static class SerializationTest
	{
		/// <summary>Serializes and deserializes an instance.</summary>
		/// <typeparam name="T">
		/// Type of the instance.
		/// </typeparam>
		/// <param name="instance">
		/// The instance to serialize and deserialize.
		/// </param>
		public static T SerializeDeserialize<T>(T instance)
		{
			using (var buffer = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(buffer, instance);

				// reset position.
				buffer.Position = 0;

				var result = (T)formatter.Deserialize(buffer);
				return result;
			}
		}

		/// <summary>Serializes and deserializes an instance usigin an XmlSerializer.</summary>
		/// <typeparam name="T">
		/// Type of the instance.
		/// </typeparam>
		/// <param name="instance">
		/// The instance to (xml) serialize and (xml) deserialize.
		/// </param>
		public static T XmlSerializeDeserialize<T>(T instance)
		{
			using (var stream = new MemoryStream())
			{
				var writer = new XmlTextWriter(stream, Encoding.UTF8);
				var serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(writer, instance);
				stream.Position = 0;
				return (T)serializer.Deserialize(stream);
			}
		}

		/// <summary>Serializes and deserializes an instance usigin an DataContractSerializer.</summary>
		/// <typeparam name="T">
		/// Type of the instance.
		/// </typeparam>
		/// <param name="instance">
		/// The instance to (xml) serialize and (xml) deserialize.
		/// </param>
		public static T DataContractSerializeDeserialize<T>(T instance)
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new DataContractSerializer(typeof(T));
				serializer.WriteObject(stream, instance);
				stream.Position = 0;
				return (T)serializer.ReadObject(stream);
			}
		}

		/// <summary>Invokes the Deserialize Constructor ( T(SerializationInfo, StreamingContext) ).</summary>
		/// <typeparam name="T">
		/// Type of the constructor to invoke.
		/// </typeparam>
		/// <param name="info">
		/// The serialization info.
		/// </param>
		/// <param name="context">
		/// The streaming context.
		/// </param>
		/// <remarks>
		/// This can throw target invocation exceptions, if so, the inner exception is thrown.
		/// </remarks>
		public static T DeserializeUsingConstructor<T>(SerializationInfo info, StreamingContext context)
		{
			var ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
			Assert.IsNotNull(ctor, "No {0}(SerializationInfo, StreamingContext) constructor found.", typeof(T).Name);

			try
			{
				return (T)ctor.Invoke(new object[] { info, context });
			}
			catch (TargetInvocationException e)
			{
				throw e.InnerException;
			}
		}

		/// <summary>Gets new serialization info using System.Runtime.Serialization.FormatterConverter.</summary>
		/// <typeparam name="T">
		/// The type to (de) serialize.
		/// </typeparam>
		public static SerializationInfo GetSerializationInfo<T>()
		{
			var info = new SerializationInfo(typeof(T), new FormatterConverter());
			return info;
		}
	}
}
