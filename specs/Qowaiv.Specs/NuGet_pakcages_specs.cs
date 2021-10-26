using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace NuGet_pakcages_specs
{
    public class All_NuGet_Packages
    {
        [TestCaseSource(nameof(Packages))]
        public void Are_signed(Assembly package)
            => package.Should().HavePublicKey(
                "0024000004800000940000000602000000240000525341310004000001000100EF35DF58AA7FEC73A11E70572E6B3791601006EF3FB1C6C1F1A402BA83BB2EDC975C61E8A32D792EDB864127F0D2C67EB7A64A9D3A0CDB0B1BB37FF2D0FCFD7990304623C044439D04DAC49624CC6D7937581419D995C2689F9898EC09C941B3EB3CAB8E4FC8F90B4AE5D45AB03D691D4D1F4B68450DAD41FED46671376934B0");

        [TestCaseSource(nameof(PureFunctions))]
        public void Pure_functions_are_marked_as_such(MethodInfo function)
            => function.Should().BeDecoratedWith<PureAttribute>(because: "The're part of immutable codebase");

        private static IEnumerable<Assembly> Packages
        {
            get
            {
                yield return typeof(Qowaiv.SingleValueObjectAttribute).Assembly;
                yield return typeof(Qowaiv.Data.SvoParameter).Assembly;
                yield return typeof(Qowaiv.TestTools.JsonTester).Assembly;
            }
        }

        private static IEnumerable<Type> AllTypes 
            => Packages
            .SelectMany(type => type.GetTypes())
            .Where(type
                => type.Name[0] != '<'
                && type.Name != "Guard"
                && NotRefStruct(type)
                && !type.GetCustomAttributes<ObsoleteAttribute>().Any());

        private static IEnumerable<MethodInfo> AllMethods
            => AllTypes.SelectMany(type => type.GetMethods(
                BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance
                | BindingFlags.Static));

        private static IEnumerable<MethodInfo> PureFunctions
            => AllMethods.Where(method 
                => PureReturn(method.ReturnType)
                && !method.IsSpecialName
                && !method.GetCustomAttributes<ObsoleteAttribute>().Any()
                && !method.GetCustomAttributes<ImpureAttribute>().Any()
                && method.GetParameters().All(p => !p.IsOut)
                && AllTypes.Contains(method.DeclaringType));

        private static bool NotRefStruct(Type type)
            => !type.IsValueType
            || !type.IsByRef;

        private static bool PureReturn(Type type)
            => type != typeof(void)
            && type != typeof(IDisposable);
    }
}
