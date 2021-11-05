using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Json.Open_API_specs
{
    public class SVOs
    {
        [TestCaseSource(nameof(JsonSerializable))]
        public void Has_OpenApiDataType_attribute(Type svo)
            => svo.Should().BeDecoratedWith<OpenApiDataTypeAttribute>();

        [Test]
        public void For_README_md()
        {
            var attributes = OpenApiDataTypeAttribute.From(typeof(Date).Assembly)
               .OrderBy(attr => attr.DataType.Namespace)
               .ThenBy(attr => attr.DataType.Name);

            var all = new Dictionary<string, OpenApiDataType>();

            foreach (var attribute in attributes)
            {
                var name = $"{attribute.DataType.Namespace}.{attribute.DataType.Name}".Replace("Qowaiv.", "", StringComparison.InvariantCulture);

                all[name] = new OpenApiDataType
                {
                    description = attribute.Description,
                    example = attribute.Example,
                    type = attribute.Type,
                    format = attribute.Format,
                    pattern = attribute.Pattern,
                    nullabe = attribute.Nullable,
                    @enum = attribute.Enum,
                };
            }

            Console.WriteLine(JsonConvert.SerializeObject(all, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            }));

            Assert.IsTrue(true, "Test should pass.");
        }

        private static IEnumerable<Type> JsonSerializable
            => typeof(Date).Assembly
            .GetTypes()
            .Where(IsJsonSerializable)
            .Except(new []{ typeof(Qowaiv.Security.Secret) });

        private static bool IsJsonSerializable(Type type)
            => type.GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Any(m => m.Name == nameof(Date.FromJson)
                && m.ReturnType == type
                && m.GetParameters().Length == 1
                && m.GetParameters()[0].ParameterType == typeof(string));

        private class OpenApiDataType
        {
            public string description { get; set; }
            public object example { get; set; }
            public string type { get; set; }
            public string format { get; set; }
            public string pattern { get; set; }
            public bool nullabe { get; set; }
            public string[] @enum { get; set; }
        }
    }
}

