using NUnit.Framework;
using Qowaiv.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace Qowaiv.TestTools
{
    /// <summary>Test base class for testing a Qowaiv assembly.</summary>
    [TestFixture(Category = "Assembly tests")]
    public abstract class AssemblyTestBase
    {
        /// <summary>The public key of Qowaiv.</summary>
        public static readonly string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100EF35DF58AA7FEC73A11E70572E6B3791601006EF3FB1C6C1F1A402BA83BB2EDC975C61E8A32D792EDB864127F0D2C67EB7A64A9D3A0CDB0B1BB37FF2D0FCFD7990304623C044439D04DAC49624CC6D7937581419D995C2689F9898EC09C941B3EB3CAB8E4FC8F90B4AE5D45AB03D691D4D1F4B68450DAD41FED46671376934B0";

        /// <summary>Gets the assembly to test.</summary>
        public Assembly TestAssembly => AssemblyType.Assembly;

        /// <summary>Gets (a) type of the assembly to test.</summary>
        /// <remarks>
        /// Just for convenience reasons the test assembly is specified via a
        /// type defined in that assembly.
        /// </remarks>
        protected abstract Type AssemblyType { get; }

        /// <summary>The SVO's expected to exist in the TestAssembly.</summary>
        protected abstract Type[] ExpectedSvos { get; }

        /// <summary>Tests if the assembly has the Qowaiv public key.</summary>
        [Test]
        public virtual void HasPublicKey()
        {
            var key = TestAssembly.GetName().GetPublicKey();
            var act = BitConverter.ToString(key).Replace("-", "");

            Console.WriteLine(act);
            Assert.AreEqual(PublicKey, act);
        }

        /// <summary>Test the implicit contract of all SVO's of the assembly.</summary>
        [Test]
        public void Analize_AllSvos_MatchAttribute()
        {
            var svos = TestAssembly.GetTypes()
               .Where(tp => QowaivType.IsSingleValueObject(tp))
               .OrderBy(tp => tp.Namespace)
               .ThenBy(tp => tp.Name)
               .ToArray();

            foreach (var svo in svos)
            {
                Console.WriteLine(svo);
            }

            CollectionAssert.AreEqual(ExpectedSvos, svos);

            foreach (var svo in svos)
            {
                var attr = QowaivType.GetSingleValueObjectAttribute(svo);

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
