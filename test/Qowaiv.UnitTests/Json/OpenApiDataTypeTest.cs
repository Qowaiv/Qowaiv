using Newtonsoft.Json;
using NUnit.Framework;
using Qowaiv.Json;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Qowaiv.UnitTests.Json
{
    public class OpenApiDataTypeTest
    {
        [Test]
        public void IJsonSerializable_All_DefineSwaggerDataType()
        {
            var expected = typeof(Date).Assembly
                .GetTypes()
                .Where(IsJsonSerializable)
                .ToArray();

            var actual = expected.Where(tp => tp.GetCustomAttribute<OpenApiDataTypeAttribute>() != null).ToArray();

            Assert.AreEqual(expected, actual);
        }

        private bool IsJsonSerializable(Type type)
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Any(m => m.Name == "FromJson"
                    && m.ReturnType == type
                    && m.GetParameters().Length == 1
                    && m.GetParameters()[0].ParameterType == typeof(string));
        }

        [Test]
        public void Generate_ReadMeTable_AllSwaggerDataTypes()
        {
            var attributes = OpenApiDataTypeAttribute.From(typeof(Date).Assembly)
                .OrderBy(attr => attr.DataType.Namespace)
                .ThenBy(attr=>attr.DataType.Name);

            var all = new Dictionary<string, OpenApiDataType>();

            foreach(var attribute in attributes)
            {
                var name = $"{attribute.DataType.Namespace}.{attribute.DataType.Name}".Replace("Qowaiv.", "", StringComparison.InvariantCulture);

                all[name] = new OpenApiDataType
                {
                    description = attribute.Description,
                    type = attribute.Type,
                    format = attribute.Format,
                    pattern = attribute.Pattern,
                    nullabe = attribute.Nullable,
                    @enum = attribute.Enum,
                };
            }

            Console.WriteLine(JsonConvert.SerializeObject(all, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));

            Assert.IsTrue(true, "Test should pass.");
        }
    }
    internal class OpenApiDataType
    {
        public string description { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string pattern { get; set; }
        public bool nullabe { get; set; }
        public string[] @enum { get; set; }
    }
}
