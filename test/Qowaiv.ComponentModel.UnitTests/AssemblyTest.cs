using Qowaiv.TestTools;
using System;

namespace Qowaiv.ComponentModel.Tests
{
    public class AssemblyTest : AssemblyTestBase
    {
        protected override Type AssemblyType => typeof(ComponentModel.Validation.AnnotatedModel);

        protected override Type[] ExpectedSvos => new Type[0];
    }
}
