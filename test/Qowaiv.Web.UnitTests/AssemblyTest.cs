using Qowaiv.TestTools;
using System;

namespace Qowaiv.Web.Tests
{
    public class AssemblyTest : AssemblyTestBase
    {
        protected override Type AssemblyType => typeof(InternetMediaType);

        protected override Type[] Svos => new[]
        {
            typeof(InternetMediaType),
        };
    }
}
