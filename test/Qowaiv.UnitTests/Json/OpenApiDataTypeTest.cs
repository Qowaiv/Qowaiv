using NUnit.Framework;
using Qowaiv.Json;
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
                .Where(tp => tp.GetInterfaces().Contains(typeof(IJsonSerializable)))
                .ToArray();

            var actual = expected.Where(tp => tp.GetCustomAttribute<OpenApiDataTypeAttribute>() != null).ToArray();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Generate_ReadMeTable_AllSwaggerDataTypes()
        {
            var types = typeof(Date).Assembly
                .GetTypes()
                .Where(tp => tp.GetInterfaces().Contains(typeof(IJsonSerializable)))
                .OrderBy(tp => tp.Namespace)
                .ThenBy(tp => tp.Name)
                .ToArray();

            foreach(var tp in types)
            {
                var attribute = tp.GetCustomAttribute<OpenApiDataTypeAttribute>();
                Assert.NotNull(attribute, tp.FullName);

                var name = $"{tp.Namespace}.{tp.Name}".Replace("Qowaiv.", "");

                Console.WriteLine($"| {name,-40} | {attribute.Type,-7} | {attribute.Format,-19} | {attribute.Nullable.ToString().ToLowerInvariant(),-8} |");
            }
        }
    }
}
