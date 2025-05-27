using AwesomeAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Diagnostics_Contracts_Specs
{
    public class Generator
    {
        private static bool IsGenerated(Type t)
            => !t.IsAbstract
            && t.IsAssignableTo(typeof(Attribute))
            && t.Namespace == "Qowaiv.Diagnostics.Contracts";

        [Test]
        public void Contains_attributes()
            => typeof(Generator).Assembly.GetTypes()
            .Where(IsGenerated)
            .Should().BeEquivalentTo(new[]
            {
                typeof(Qowaiv.Diagnostics.Contracts.CollectionMutationAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.EmptyClassAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.EmptyEnumAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.EmptyInterfaceAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.EmptyStructAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.EmptyTestClassAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.EmptyTestEnumAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.EmptyTestInterfaceAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.EmptyTestStructAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.FluentSyntaxAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.ImpureAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.InheritableAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.MutableAttribute),
                typeof(Qowaiv.Diagnostics.Contracts.WillBeSealedAttribute),
            });
    }
}


