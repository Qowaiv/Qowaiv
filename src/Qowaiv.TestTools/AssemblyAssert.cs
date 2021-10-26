using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Qowaiv.TestTools
{
    /// <summary>Contains assert methods on <see cref="Assembly"/>'s.</summary>
    public static class AssemblyAssert
    {
        /// <summary>Asserts that the assembly (of the specified <see cref="Type"/>) has the expected public key.</summary>
        [Obsolete("Use .Should().HavePublicKey() instead.")]
        [DebuggerStepThrough]
        public static void HasPublicKey<T>(string publicKey)
        {
            var assembly = typeof(T).Assembly;
            var bytes = assembly.GetName().GetPublicKey();
            var act = BitConverter.ToString(bytes).Replace("-", "");

            Console.WriteLine(act);
            Assert.AreEqual(publicKey, act);
        }

        /// <summary>Asserts that the assembly (of the specified <see cref="Type"/>) has the expected SVO's.</summary>
        [DebuggerStepThrough]
        public static void ContainsSvos<T>(params Type[] expected)
        {
            var assembly = typeof(T).Assembly;
            var svos = assembly.GetTypes()
               .Where(tp => tp.GetCustomAttribute<SingleValueObjectAttribute>() != null)
               .OrderBy(tp => tp.Namespace)
               .ThenBy(tp => tp.Name)
               .ToArray();

            foreach (var svo in svos)
            {
                Console.WriteLine(svo);
            }

            var missing = expected.Except(svos).ToArray();
            var extra = svos.Except(expected).ToArray();

            if (missing.Any() || extra.Any())
            {
                var sb = new StringBuilder();
                if (missing.Any())
                {
                    sb.AppendLine("Missing SVO's:");
                    foreach (var type in missing)
                    {
                        sb.AppendLine($"- {type}");
                    }
                }
                if (extra.Any())
                {
                    sb.AppendLine("Unexpected SVO's:");
                    foreach (var type in extra)
                    {
                        sb.AppendLine($"- {type}");
                    }
                }

                Assert.Fail(sb.ToString());
            }

            foreach (var svo in svos)
            {
                var attr = svo.GetCustomAttribute<SingleValueObjectAttribute>();

                SvoAssert.UnderlyingTypeMatches(svo, attr);
                SvoAssert.ParseMatches(svo, attr);
                SvoAssert.TryParseMatches(svo, attr);
                SvoAssert.IsValidMatches(svo, attr);
                SvoAssert.EmptyAndUnknownMatches(svo, attr);
                SvoAssert.ImplementsInterfaces(svo);
                SvoAssert.AttributesMatches(svo);
            }
        }
    }
}
